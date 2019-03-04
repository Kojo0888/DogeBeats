using DogeBeats.EngineSections.Resources;
using DogeBeats.EngineSections.Shared;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;
using Xunit;

namespace DogeBeatsTests.EngineSections.TimeLines
{
    public class TimeLineTests
    {
        Testowy.Model.TimeLine timeLine = new Testowy.Model.TimeLine();

        private void InitAnimationElements()
        {
            timeLine.AnimationElements = MockObjects.GetTimeLine2().AnimationElements;
        }

        [Theory]
        //[InlineData(2, 1, 0, 2)]
        //[InlineData(11, 1, 1, 1)]
        [InlineData(30, 1, 2, 0)]
        public void CheckCurrentAnimatingElements(int sec, int currentlyAnimatingElements, int PassedAnimationElements, int storyboardqueue)
        {
            InitAnimationElements();
            timeLine.InitializeStoryboardQueue();
            timeLine.Stopper = new DogeBeats.Misc.DStopper();
            timeLine.Stopper.Elapsed = new TimeSpan(0,0,0, sec);
            timeLine.StartStoryboard();

            timeLine.VerifyAnimationTimes();
            timeLine.ProgressStoryboard();

            timeLine.PauseStoryboard(false);

            Assert.Equal(currentlyAnimatingElements, timeLine.CurrentlyAnimatingElements.Count);
            Assert.Equal(PassedAnimationElements, timeLine.PassedAnimationElements.Count);
            Assert.Equal(storyboardqueue, timeLine.StoryboardQueue.Count);
        }

        [Fact]
        public void ManualUpdate()
        {
            string timeLineNAme = "Test123321";
            var values = new NameValueCollection();
            values.Add("Name", timeLineNAme);
            timeLine.ManualUpdate(values);

            if (timeLine.Name != timeLineNAme)
                throw new NesuException("Timeline name differs");
        }

        [Fact]
        public void FixGroupAnimationTime()
        {
            InitAnimationElements();
            var time = new TimeSpan(1, 0, 0);
            var group = timeLine.AnimationElements.OfType<AnimationGroupElement>().LastOrDefault();
            group.Elements.LastOrDefault().Route.Frames.LastOrDefault().FrameTime = time;
            var totalDurationTimeBefore = group.Elements.LastOrDefault().GetDurationTime();

            timeLine.FixGroupAnimationTime();

            var totalDurationTimeAfter = group.Elements.LastOrDefault().GetDurationTime();
            if (totalDurationTimeBefore != totalDurationTimeAfter)
                throw new NesuException("totalDurationTime does not match");

            group = timeLine.AnimationElements.OfType<AnimationGroupElement>().LastOrDefault();
            if (group.Route.AnimationEndTime - group.Route.AnimationStartTime != new TimeSpan(1,0,39))
                throw new NesuException("Time does not match");
        }

        [Fact]
        public void FixGroupAnimationTime2()
        {
            InitAnimationElements();
            var group = timeLine.AnimationElements.OfType<AnimationGroupElement>().LastOrDefault();

            timeLine.FixGroupAnimationTime();

            group = timeLine.AnimationElements.OfType<AnimationGroupElement>().LastOrDefault();
            if (group.Route.AnimationEndTime != new TimeSpan(0, 0, 57))
                throw new NesuException("Time does not match. it is: " + (group.Route.AnimationEndTime - group.Route.AnimationStartTime));
        }

        [Fact]
        public void FixGroupAnimationTime3()
        {
            InitAnimationElements();
            var time = new TimeSpan(0, 1, 0);
            var group = timeLine.AnimationElements.OfType<AnimationGroupElement>().LastOrDefault();
            var single = group.Elements.LastOrDefault();
            var newGroup = group.ConvertToGroup(single);
            var newSingleElement = new AnimationSingleElement();
            newSingleElement.Route.AnimationStartTime = new TimeSpan(0,0,10);
            newSingleElement.Route.Frames.Add(new AnimationRouteFrame() { FrameTime = time });
            newGroup.Elements.Add(newSingleElement);

            timeLine.FixGroupAnimationTime();

            group = timeLine.AnimationElements.OfType<AnimationGroupElement>().LastOrDefault();
            if (group.Route.AnimationEndTime != new TimeSpan(0, 2, 0))
                throw new NesuException("Time does not match. End time is " + group.Route.AnimationEndTime);
        }

        [Fact]
        public void FixGroupAnimationTime4()
        {
            InitAnimationElements();
            var time = new TimeSpan(0, 1, 0);
            var group = timeLine.AnimationElements.OfType<AnimationGroupElement>().LastOrDefault();
            var single = group.Elements.LastOrDefault();
            var newGroup = group.ConvertToGroup(single);
            newGroup.Route.AnimationStartTime = new TimeSpan(0,1,0);
            var newSingleElement = new AnimationSingleElement();
            newSingleElement.Route.AnimationStartTime = new TimeSpan(0, 0, 10);
            newSingleElement.Route.Frames.Add(new AnimationRouteFrame() { FrameTime = time });
            newGroup.Elements.Add(newSingleElement);

            timeLine.FixGroupAnimationTime();

            group = timeLine.AnimationElements.OfType<AnimationGroupElement>().LastOrDefault();
            if (group.Route.AnimationEndTime != new TimeSpan(0, 2, 23))
                throw new NesuException("Time does not match. End time is " + group.Route.AnimationEndTime);
        }

        [Fact]
        public void SearchParentAnimationElement_IAnimationElement()
        {
            InitAnimationElements();

            var group = timeLine.AnimationElements.OfType<AnimationGroupElement>().LastOrDefault();
            var child = group.Elements.LastOrDefault();

            var parent = timeLine.SearchParentAnimationElement(child);
            if (group != parent)
                throw new NesuException("Parent is invalid");
        }

        [Fact]
        public void SearchParentAnimationElement_AnimationRoute()
        {
            InitAnimationElements();

            var group = timeLine.AnimationElements.OfType<AnimationGroupElement>().LastOrDefault();
            var frame = group.Route.Frames.LastOrDefault();

            var parent = timeLine.SearchParentAnimationElement(frame);
            if (group != parent)
                throw new NesuException("Parent is invalid");
        }

        [Fact]
        public void GetAllAnimationGroupElements()
        {
            InitAnimationElements();

            var groups = timeLine.GetAllAnimationGroupElements();
            if (groups.Count != 1)
                throw new NesuException("Groups count is not 1");
        }

        [Fact]
        public void GetAnimationSingleElementFirstLayer()
        {
            InitAnimationElements();

            var singleElements = timeLine.GetAnimationSingleElementFirstLayer();
            if (singleElements.Count != 2)
                throw new NesuException("singleElement count != 2");
        }

        [Fact]
        public void GetAnimationGroupElementFirstLayer()
        {
            InitAnimationElements();

            var groupElements = timeLine.GetAnimationGroupElementFirstLayer();
            if (groupElements.Count != 1)
                throw new NesuException("groupElements count != 1");
        }
    }
}
