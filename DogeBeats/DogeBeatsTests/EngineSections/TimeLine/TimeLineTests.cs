using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;
using Xunit;

namespace DogeBeatsTests.EngineSections.TimeLine
{
    public class TimeLineTests
    {
        Testowy.Model.TimeLine timeLine = new Testowy.Model.TimeLine();

        private void InitAnimationElements()
        {
            timeLine.AnimationElements = new List<DogeBeats.EngineSections.AnimationObjects.IAnimationElement>()
            {
                new AnimationSingleElement()
                {
                    Route = new AnimationRoute()
                    {
                        AnimationStartTime = new TimeSpan(0,0,0,10),
                        Frames = new List<AnimationRouteFrame>()
                        {
                            new AnimationRouteFrame()
                            {
                                FrameTime = new TimeSpan(0,0,0,12)
                            },
                            new AnimationRouteFrame()
                            {
                                FrameTime = new TimeSpan(0,0,0,1)
                            },
                            new AnimationRouteFrame()
                            {
                                FrameTime = new TimeSpan(0,0,0,5)
                            }
                        }
                    }
                },
                new AnimationSingleElement()
                {
                    Route = new AnimationRoute()
                    {
                        AnimationStartTime = new TimeSpan(0,0,0,2),
                        Frames = new List<AnimationRouteFrame>()
                        {
                            new AnimationRouteFrame()
                            {
                                FrameTime = new TimeSpan(0,0,0,1)
                            },
                            new AnimationRouteFrame()
                            {
                                FrameTime = new TimeSpan(0,0,0,1)
                            },
                            new AnimationRouteFrame()
                            {
                                FrameTime = new TimeSpan(0,0,0,5)
                            }
                        }
                    }
                },
                new AnimationGroupElement()
                {
                    Route = new AnimationRoute()
                    {
                        AnimationStartTime = new TimeSpan(0,0,0,13),
                        Frames = new List<AnimationRouteFrame>()
                        {
                            new AnimationRouteFrame()
                            {
                                FrameTime = new TimeSpan(0,0,0,12)
                            },
                            new AnimationRouteFrame()
                            {
                                FrameTime = new TimeSpan(0,0,0,1)
                            },
                            new AnimationRouteFrame()
                            {
                                FrameTime = new TimeSpan(0,0,0,5)
                            }
                        }
                    },
                    Elements = new List<DogeBeats.EngineSections.AnimationObjects.IAnimationElement>()
                    {
                        new AnimationSingleElement()
                        {
                            Route = new AnimationRoute()
                            {
                                AnimationStartTime = new TimeSpan(0,0,0,50),
                                Frames = new List<AnimationRouteFrame>()
                                {
                                    new AnimationRouteFrame()
                                    {
                                        FrameTime = new TimeSpan(0,0,0,1)
                                    },
                                    new AnimationRouteFrame()
                                    {
                                        FrameTime = new TimeSpan(0,0,0,1)
                                    },
                                    new AnimationRouteFrame()
                                    {
                                        FrameTime = new TimeSpan(0,0,0,5)
                                    }
                                }
                            }
                        },
                    }
                }
            };
        }

        [Theory]
        [InlineData(2, 1, 0, 2)]
        [InlineData(11, 1, 1, 1)]
        [InlineData(30, 1, 2, 0)]
        public void CheckCurrentAnimatingElements(int sec, int currentlyAnimatingElements, int PassedAnimationElements, int storyboardqueue)
        {
            InitAnimationElements();
            timeLine.InitializeStoryboardQueue();
            timeLine.Stopper = new DogeBeats.Misc.DStopper();
            timeLine.Stopper.Elapsed = new TimeSpan(0,0,0, sec);
            timeLine.StartStoryboard();

            timeLine.Verify();
            timeLine.ProgressStoryboard();

            timeLine.PauseStoryboard(false);

            Assert.Equal(currentlyAnimatingElements, timeLine.CurrentlyAnimatingElements.Count);
            Assert.Equal(PassedAnimationElements, timeLine.PassedAnimationElements.Count);
            Assert.Equal(storyboardqueue, timeLine.StoryboardQueue.Count);
        }

        [Fact]
        public void ManualUpdate()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void FixGroupAnimationTime()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void SearchParentAnimationElement_IAnimationElement()
        {
            throw new NotImplementedException();
        }


        [Fact]
        public void SearchParentAnimationElement_AnimationRoute()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void GetAllAnimationGroupElements()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void GetAnimationSingleElementFirstLayer()
        {
            throw new NotImplementedException();
        }
    }
}
