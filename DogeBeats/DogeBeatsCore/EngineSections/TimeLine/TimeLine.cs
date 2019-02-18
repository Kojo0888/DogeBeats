using DogeBeats.EngineSections.AnimationObjects;
using DogeBeats.EngineSections.Resources;
using DogeBeats.EngineSections.Shared;
using DogeBeats.Misc;
using DogeBeats.Modules.Music;
using DogeBeats.Modules.TimeLines;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Testowy.Model
{
    public class TimeLine : INamedElement
    {
        [JsonIgnore]
        public DStopper Stopper = new DStopper();

        [JsonIgnore]
        public Queue<IAnimationElement> StoryboardQueue = new Queue<IAnimationElement>();
        [JsonIgnore]
        public List<IAnimationElement> CurrentlyAnimatingElements = new List<IAnimationElement>();
        [JsonIgnore]
        public List<IAnimationElement> PassedAnimationElements = new List<IAnimationElement>();

        public List<IAnimationElement> AnimationElements  = new List<IAnimationElement>();

        public TimeSpan LoopTime;

        public BeatGuide BeatGuider { get; set; } = new BeatGuide();

        public string Name { get; set; }

        private TimeSpan _playToTimeSpan { get; set; }

        public string MusicName { get; set; }

        [JsonIgnore]
        public SoundItem MusicTrack { get; set; }

        public void ManualUpdate(NameValueCollection values)
        {
            StaticHub.TimeLineCentre.RenameElement(this, Name, values["Name"]);
            Name = ManualUpdaterParser.Parse(values["Name"], Name);

            MusicName = ManualUpdaterParser.Parse(values["MusicName"], MusicName);
            MusicTrack = StaticHub.SoundCentre.Get(MusicName);
        }

        public static List<string> GetKeysManualUpdate()
        {
            return new List<string>() {
                "Name",
                "MusicName",
            };
        }

        public void StartStoryboard()
        {
            if (Stopper.IsRunning)
                Stopper.Stop();
            Stopper.Reset();
            Stopper.Start();
        }

        public void ResumeStoryboard()
        {
            Stopper.Start();
        }

        public void PauseStoryboard(bool resetStoryboard = true)
        {
            Stopper.Stop();
            if (resetStoryboard)
                ResetStoryboard();
        }

        public void ResetStoryboard()
        {
            InitializeStoryboardQueue();
            ProgressStoryboard();
        }

        public void Verify()
        {
            FixGroupAnimationTime();
        }

        public void FixGroupAnimationTime()
        {
            foreach (var animationElement in AnimationElements)
            {
                if(animationElement is AnimationGroupElement)
                {
                    var group = animationElement as AnimationGroupElement;
                    group.FixParentAnimationTime();
                }
            }
        }

        public void ProgressStoryboard()
        {
            if (!Stopper.IsRunning)
                return;

            CheckEndPlayTime();

            //should run every frame
            PushAwaitingAnimationElements();
            UpdateAnimationElements();

            //can run once a time
            PushPassedElements();
        }

        private void CheckEndPlayTime()
        {
            if (Stopper.Elapsed > _playToTimeSpan)
                Stopper.Stop();
        }

        private void UpdateAnimationElements()
        {
            foreach (var element in CurrentlyAnimatingElements)
            {
                element.Update(Stopper.Elapsed, new Placement());
            }
        }

        public void RenderStoryboardAnimation()
        {
            foreach (var group in CurrentlyAnimatingElements)
            {
                group.Render();
            }
        }

        public IAnimationElement SearchParentAnimationElement(IAnimationElement singleAnimationElement)
        {
            foreach (var AnimationElement in AnimationElements)
            {
                if (AnimationElement is AnimationGroupElement)
                {
                    var group = AnimationElement as AnimationGroupElement;

                    AnimationGroupElement result = group.SearchParentAnimationElement(singleAnimationElement);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        public IAnimationElement SearchParentAnimationElement(AnimationRouteFrame routeFrame)
        {
            foreach (var AnimationElement in AnimationElements)
            {
                if (AnimationElement is AnimationSingleElement)
                {
                    var single = AnimationElement as AnimationSingleElement;
                    var singleResult = single.SearchParentAnimationElement(routeFrame);
                    if (singleResult != null)
                        return singleResult;
                }
                if (AnimationElement is AnimationGroupElement)
                {
                    var group = AnimationElement as AnimationGroupElement;
                    var groupResult = group.SearchParentAnimationElement(routeFrame);
                    if (groupResult != null)
                        return groupResult;
                }
            }
            return null;
        }

        public void InitializeStoryboardQueue()
        {
            StoryboardQueue = new Queue<IAnimationElement>();
            foreach (var element in AnimationElements.OrderBy(o => o.Route.AnimationStartTime))
            {
                StoryboardQueue.Enqueue(element);
            }
        }

        public List<AnimationGroupElement> GetAllAnimationGroupElements()
        {
            List<AnimationGroupElement> toReturn = new List<AnimationGroupElement>();

            foreach (var animationElemnt in AnimationElements)
            {
                if(animationElemnt is AnimationGroupElement)
                {
                    var aGroupElem = animationElemnt as AnimationGroupElement;
                    if(aGroupElem != null)
                    {
                        toReturn.Add(aGroupElem);
                        toReturn.AddRange(aGroupElem.GetAnimationGroupElements());
                    }
                }
            }

            return toReturn;
        }

        public void Refresh()
        {
            //Stopper.Stop();

            InitializeStoryboardQueue();

            ProgressStoryboard();
            //ProgressStoryboardAnimation();//you could be used again to improve performance

            //Stopper.Start();
        }

        public void RegisterPlayToTimeSpan(TimeSpan to)
        {
            _playToTimeSpan = to;
        }

        public List<AnimationSingleElement> GetAnimationSingleElementFirstLayer()
        {
            List<AnimationSingleElement> elementsFirstLayer = new List<AnimationSingleElement>();
            foreach (var element in AnimationElements)
            {
                if(element is AnimationSingleElement)
                    elementsFirstLayer.Add(element as AnimationSingleElement);
            }
            return elementsFirstLayer;
        }

        public List<AnimationGroupElement> GetAnimationGroupElementFirstLayer()
        {
            List<AnimationGroupElement> elementsFirstLayer = new List<AnimationGroupElement>();
            foreach (var element in AnimationElements)
            {
                if (element is AnimationGroupElement)
                    elementsFirstLayer.Add(element as AnimationGroupElement);
            }
            return elementsFirstLayer;
        }

        private void PushPassedElements()
        {
            PassedAnimationElements.AddRange(CurrentlyAnimatingElements.Where(w => w.Route.AnimationEndTime < Stopper.Elapsed).ToList());
            CurrentlyAnimatingElements.RemoveAll(r => r.Route.AnimationEndTime < Stopper.Elapsed);
        }

        private void PushAwaitingAnimationElements()
        {
            if(StoryboardQueue.Count > 0)
            {
                while (StoryboardQueue.Peek().Route.AnimationStartTime < Stopper.Elapsed)
                {
                    var element = StoryboardQueue.Dequeue();
                    CurrentlyAnimatingElements.Add(element);

                    if (StoryboardQueue.Count == 0)
                        break;
                }
            }
        }

        private void OrderAnimationElements()
        {
            AnimationElements = AnimationElements.OrderBy(o => o.Route.AnimationStartTime).ToList();
        }

        //internal AnimationGroupElement SearchForAnimationGroupElement(string graphicName)
        //{
        //    foreach (var group in AnimationGroupElements)
        //    {
        //        if (group.GraphicName == graphicName)
        //            return group;
        //    }
        //    return null;
        //}

        //internal AnimationElement SearchForAnimationElement(string graphicName)
        //{
        //    foreach (var group in AnimationGroupElements)
        //    {
        //        foreach (var element in group.Elements)
        //        {
        //            if (element.GraphicName == graphicName)
        //                return element;
        //        }
        //    }
        //    return null;
        //}

        //internal AnimationRouteFrame SearchForRouteFrame(string graphicName)
        //{
        //    foreach (var group in AnimationGroupElements)
        //    {
        //        foreach (var element in group.Elements)
        //        {
        //            foreach (var frame in element.Route.Frames)
        //            {
        //                if (frame.GraphicName == graphicName)
        //                    return frame;
        //            }
        //        }
        //    }
        //    return null;
        //}
    }
}
