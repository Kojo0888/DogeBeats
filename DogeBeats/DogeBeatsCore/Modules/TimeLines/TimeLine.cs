using DogeBeats.Misc;
using DogeBeats.Modules.TimeLines;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Testowy.Model
{
    public class TimeLine
    {
        public DStopper Stopper = new DStopper();

        public Queue<AnimationGroupElement> Storyboard = new Queue<AnimationGroupElement>();

        public List<AnimationGroupElement> CurrentlyAnimatingGroups = new List<AnimationGroupElement>();

        public List<AnimationGroupElement> PassedAnimationGroups = new List<AnimationGroupElement>();

        public List<AnimationGroupElement> AnimationGroupElements = new List<AnimationGroupElement>();

        public TimeSpan LoopTime;

        public BeatGuider BeatGuider { get; set; } = new BeatGuider();

        public string TimeLineName { get; internal set; }

        private TimeSpan _playToTimeSpan { get; set; }

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
                element.Update(Stopper.Elapsed);
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
            PassedAnimationGroups.AddRange(CurrentlyAnimatingGroups.Where(w => w.GroupRoute.AnimationEndTime < Stopper.Elapsed).ToList());
            CurrentlyAnimatingGroups.RemoveAll(r => r.GroupRoute.AnimationEndTime < Stopper.Elapsed);
        }

        private void CheckStoryboard()
        {
            while (Storyboard.Peek().GroupRoute.AnimationStartTime < Stopper.Elapsed)
            {
                var element = Storyboard.Dequeue();
                CurrentlyAnimatingGroups.Add(element);
            }
        }

        public void InitializeStoryboardQueue()
        {
            Storyboard = new Queue<AnimationGroupElement>();
            foreach (var element in AnimationGroupElements.OrderBy(o => o.GroupRoute.AnimationStartTime))
            {
                Storyboard.Enqueue(element);
            }
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
            AnimationGroupElements = AnimationGroupElements.OrderBy(o => o.GroupRoute.AnimationStartTime).ToList();
        }

        internal void RegisterPlayToTimeSpan(TimeSpan to)
        {
            _playToTimeSpan = to;
        }

        internal AnimationGroupElement SearchForAnimationGroupElement(string graphicName)
        {
            foreach (var group in AnimationGroupElements)
            {
                if (group.GraphicName == graphicName)
                    return group;
            }
            return null;
        }

        internal AnimationElement SearchForAnimationElement(string graphicName)
        {
            foreach (var group in AnimationGroupElements)
            {
                foreach (var element in group.Elements)
                {
                    if (element.GraphicName == graphicName)
                        return element;
                }
            }
            return null;
        }

        internal AnimationRouteFrame SearchForRouteFrame(string graphicName)
        {
            foreach (var group in AnimationGroupElements)
            {
                foreach (var element in group.Elements)
                {
                    foreach (var frame in element.Route.Frames)
                    {
                        if (frame.GraphicName == graphicName)
                            return frame;
                    }
                }
            }
            return null;
        }
    }
}
