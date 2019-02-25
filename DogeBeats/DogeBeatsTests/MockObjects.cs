using DogeBeats.EngineSections.AnimationObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeatsTests
{
    public static class MockObjects
    {
        public static AnimationSingleElement GetAnimationElement()
        {
            var values = new System.Collections.Specialized.NameValueCollection();
            values.Add("Prediction", "False");
            values.Add("ShapeTypeName", "IdkYet");
            values.Add("Name", "TestName123");
            var aElem = new AnimationSingleElement();
            aElem.UpdateManual(values);
            return aElem;
        }

        public static AnimationSingleElement GetAnimationElement2()
        {
            var values = new System.Collections.Specialized.NameValueCollection();
            values.Add("Prediction", "False");
            values.Add("ShapeTypeName", "IdkYet32");
            var aElem = new AnimationSingleElement();
            aElem.UpdateManual(values);
            return aElem;
        }

        public static TimeLine GetTimeLine()
        {
            TimeLine timeLine = new TimeLine();
            timeLine.AnimationElements = new List<IAnimationElement>() { new AnimationGroupElement("TestGroup1") };
            timeLine.Name = "TEstLimeLint1";
            return timeLine;
        }

        public static TimeLine GetTimeLine2()
        {
            return new TimeLine()
            {
                AnimationElements = new List<DogeBeats.EngineSections.AnimationObjects.IAnimationElement>()
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
            }
            };
        }
    }
}
