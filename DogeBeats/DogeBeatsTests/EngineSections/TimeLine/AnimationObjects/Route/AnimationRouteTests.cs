using Testowy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using Xunit;

namespace Testowy.Model.Tests
{
    public class AnimationRouteTests
    {
        AnimationRoute route = new AnimationRoute();

        public AnimationRouteTests()
        {
            route.AnimationStartTime = new TimeSpan(0,0,20);
            route.Frames = new List<AnimationRouteFrame>() {
                new AnimationRouteFrame() { FrameTime = new TimeSpan(0,0,5)},
                new AnimationRouteFrame() { FrameTime = new TimeSpan(0,0,11)},
                new AnimationRouteFrame() { FrameTime = new TimeSpan(0,0,1)}
            };
        }

        [Fact]
        public void CalculateAnimationTimeTest()
        {
            var ts = route.CalculateAnimationTime();
            if(ts != new TimeSpan(0,0,17))
            throw new Exception("Assert Fails");
        }

        [Fact]
        public void AnimationEndTimeTest()
        {
            TimeSpan ts = route.CalculateAnimationTime();
            if (ts + route.AnimationStartTime != route.AnimationEndTime)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void GetFrameSliderTest()
        {
            var slider = route.GetFrameSlider(new TimeSpan(0,0,30));
            if (slider.PreviousFrame != route.Frames.ElementAt(0) ||
                slider.CurrentFrame != route.Frames.ElementAt(1) ||
                slider.NextFrame != route.Frames.ElementAt(2))
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void UpdateManualTest()
        {
            NameValueCollection values = new NameValueCollection();
            values.Add("Name", "tst");
            values.Add("AnimationStartTime", "00:00:40");
            route.UpdateManual(values);
            if (route.Name != values["Name"] ||
                route.AnimationStartTime.ToString() != values["AnimationStartTime"])
                throw new Exception("Assert Fails");
            //values.Add("");
        }
    }
}