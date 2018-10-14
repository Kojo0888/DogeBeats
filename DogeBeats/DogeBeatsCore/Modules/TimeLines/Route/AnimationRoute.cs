using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Testowy.Model
{
    public  class AnimationRoute
    {
        public List<AnimationRouteFrame> Frames { get; set; }

        public string RouteName { get; set; }

        private TimeSpan _animationTime;
        public TimeSpan AnimationTime
        {
            get
            {
                if(_animationTime == null)
                    _animationTime = CalculateAnimationTime();
                return _animationTime;
            }
        }

        internal Placement CalculatePlacement(TimeSpan currentStopperTime, Placement shapePlacement)
        {
            var frameSlider = GetFrameSlider(currentStopperTime);
            Placement newPlacement = new Placement();

            float easeMultiplier = CalculateEaseMultiplier(frameSlider, currentStopperTime);

            newPlacement.X = frameSlider.PreviousFrame.Placement.X + (easeMultiplier * frameSlider.CurrentFrame.Placement.X);

            newPlacement.Y = frameSlider.PreviousFrame.Placement.Y + (easeMultiplier * frameSlider.CurrentFrame.Placement.Y);

            newPlacement.Width = frameSlider.PreviousFrame.Placement.Width + (easeMultiplier * frameSlider.CurrentFrame.Placement.Width);

            newPlacement.Height = frameSlider.PreviousFrame.Placement.Height + (easeMultiplier * frameSlider.CurrentFrame.Placement.Height);

            newPlacement.Rotation = frameSlider.PreviousFrame.Placement.Rotation + (easeMultiplier * frameSlider.CurrentFrame.Placement.Rotation);
            
            return newPlacement;
        }

        private float CalculateEaseMultiplier(AnimationRouteFrameSlider pair, TimeSpan currentStopperTime)
        {
            var diffTicks = pair.CurrentFrame.TimeLength.Ticks;
            var progressTime = diffTicks / currentStopperTime.Ticks;
            if (progressTime > 1)
                throw new Exception("Nesu: Ease value is greater than 1");

            QuadraticEase ease = new QuadraticEase();
            ease.EasingMode = pair.CurrentFrame.Ease;
            var multiplier = ease.Ease(progressTime);
            return (float)multiplier;
        }

        public TimeSpan CalculateAnimationTime()
        {
            TimeSpan animationTime = new TimeSpan();
            foreach (var route in Frames)
            {
                animationTime = animationTime.Add(route.TimeLength);
            }
            return animationTime;
        }

        public AnimationRouteFrameSlider GetFrameSlider(TimeSpan currentStopperTime)
        {
            AnimationRouteFrameSlider slider = new AnimationRouteFrameSlider();

            for (int i = 0; i < Frames.Count; i++)
            {
                var frame = Frames[i];
                if (currentStopperTime < frame.TimeLength)
                {
                    if (i - 1 > 0)
                        slider.PreviousFrame = Frames[i - 1];
                    slider.CurrentFrame = frame;
                    if (i + 1 < Frames.Count)
                        slider.NextFrame = Frames[i + 1];
                }
            }

            return slider;
        }
    }
}
