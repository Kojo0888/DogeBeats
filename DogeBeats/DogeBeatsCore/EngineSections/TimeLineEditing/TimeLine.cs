using DogeBeats.EngineSections.AnimationObjects;
using DogeBeats.EngineSections.Shared;
using DogeBeats.Misc;
using DogeBeats.Modules.TimeLines;
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
        public DStopper Stopper = new DStopper();

        [NonSerialized]
        public Queue<IAnimationElement> Storyboard = new Queue<IAnimationElement>();

        public List<IAnimationElement> CurrentlyAnimatingGroups = new List<IAnimationElement>();

        public List<IAnimationElement> PassedAnimationGroups = new List<IAnimationElement>();

        public List<IAnimationElement> AnimationElements  = new List<IAnimationElement>();

        public TimeSpan LoopTime;

        public BeatGuider BeatGuider { get; set; } = new BeatGuider();

        public string Name { get; set; }

        private TimeSpan _playToTimeSpan { get; set; }

        public string MusicName { get; set; }

        public static void ManualUpdate(TimeLine timeline, NameValueCollection values)
        {
            if (!string.IsNullOrEmpty(values["TimeLineName"].ToString()))
                timeline.Name = values["TimeLineName"].ToString();
            if (!string.IsNullOrEmpty(values["MusicName"].ToString()))
                timeline.MusicName = values["MusicName"].ToString();
        }

        public static List<string> GetKeysManualUpdate()
        {
            return new List<string>() { "MusicName", "TimeLineName" };
        }

        public void StartStoryboardAnimation()
        {
            if (Stopper.IsRunning)
                Stopper.Stop();
            Stopper.Reset();
            Stopper.Start();
        }

        public void ResumeStoryboardAnimation()
        {
            Stopper.Start();
        }

        public void PauseStoryboardAnimation(bool resetStoryboard = true)
        {
            Stopper.Stop();
            if (resetStoryboard)
                ResetStoryboard();
        }

        public void ResetStoryboard()
        {
            InitializeStoryboardQueue();
        }

        public void ProgressStoryboardAnimation()
        {
            CheckPlayEnd();

            if (!Stopper.IsRunning)
                return;

            //this could be invoked once a time
            MovePassedElements();

            //this could be invoked every time
            CheckStoryboard();
            UpdateAnimationElements();
        }

        private void CheckPlayEnd()
        {
            if (Stopper.Elapsed > _playToTimeSpan)
                Stopper.Stop();
        }

        private void UpdateAnimationElements()
        {
            foreach (var element in CurrentlyAnimatingGroups)
            {
                element.Update(Stopper.Elapsed, new Placement());
            }
        }

        public void RenderStoryboardAnimation()
        {
            foreach (var group in CurrentlyAnimatingGroups)
            {
                group.Render();
            }
        }

        private void MovePassedElements()
        {
            PassedAnimationGroups.AddRange(CurrentlyAnimatingGroups.Where(w => w.Route.AnimationEndTime < Stopper.Elapsed).ToList());
            CurrentlyAnimatingGroups.RemoveAll(r => r.Route.AnimationEndTime < Stopper.Elapsed);
        }

        private void CheckStoryboard()
        {
            while (Storyboard.Peek().Route.AnimationStartTime < Stopper.Elapsed)
            {
                var element = Storyboard.Dequeue();
                CurrentlyAnimatingGroups.Add(element);
            }
        }

        public void InitializeStoryboardQueue()
        {
            Storyboard = new Queue<IAnimationElement>();
            foreach (var element in AnimationElements.OrderBy(o => o.Route.AnimationStartTime))
            {
                Storyboard.Enqueue(element);
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

        public void RefreshCurrentlyAnimatingElementList()
        {
            Stopper.Stop();

            InitializeStoryboardQueue();

            ProgressStoryboardAnimation();
            //ProgressStoryboardAnimation();//you could be used again to improve performance

            //CurrentlyAnimatingGroups = new List<AnimationGroupElement>();
            //CurrentlyAnimatingGroups = AnimationGroupElements.Where(w => w.GroupRoute.AnimationStartTime >= Stopper.Elapsed && w.GroupRoute.AnimationEndTime <= Stopper.Elapsed).ToList();
            //PassedAnimationGroups = new List<AnimationGroupElement>();

            Stopper.Start();
        }

        public void OrderAllAnimationElements()
        {
            AnimationElements = AnimationElements.OrderBy(o => o.Route.AnimationStartTime).ToList();
        }

        internal void RegisterPlayToTimeSpan(TimeSpan to)
        {
            _playToTimeSpan = to;
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
