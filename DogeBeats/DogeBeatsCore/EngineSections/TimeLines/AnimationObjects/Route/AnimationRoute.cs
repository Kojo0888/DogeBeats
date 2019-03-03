using DogeBeats.EngineSections.Shared;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Testowy.Model
{
    public class AnimationRoute : INamedElement
    {
        public Placement StartPlacement { get; set; }

        public List<AnimationRouteFrame> Frames { get; set; }

        public string Name { get; set; }

        public TimeSpan AnimationStartTime { get; set; }//relative

        public TimeSpan AnimationEndTime
        {
            get
            {
                return AnimationStartTime.Add(CalculateAnimationTime());
            }
        }

        public AnimationRoute()
        {
            Frames = new List<AnimationRouteFrame>();
            StartPlacement = new Placement();
        }

        public static IEnumerable<string> GetKeysManualUpdate()
        {
            List<string> keys = new List<string>();
            keys.Add("Name");
            keys.Add("AnimationStartTime");
            return keys;
        }

        public void UpdateManual(NameValueCollection values)
        {
            Name = ManualUpdaterParser.Parse(values["Name"], Name);
            AnimationStartTime = ManualUpdaterParser.Parse(values["AnimationStartTime"], AnimationStartTime);
            StartPlacement.UpdateManual(values);
        }

        internal Placement CalculatePlacement(TimeSpan currentStopperTime)
        {
            var frameSlider = GetFrameSlider(currentStopperTime);
            Placement newPlacement = new Placement();

            if (frameSlider.NextFrame == null && currentStopperTime > CalculateAnimationTime())
                throw new Exception("Nesu Out of Animation Duration Time (Parent probably)");
            //TODO: Remake is needed
            
            return frameSlider.CurrentFrame.CheckpointPosition;
        }

        public TimeSpan CalculateAnimationTime()
        {
            TimeSpan animationTime = new TimeSpan();
            foreach (var frame in Frames)
            {
                animationTime = animationTime.Add(frame.FrameTime);
            }
            return animationTime;
        }

        public AnimationRouteFrameSlider GetFrameSlider(TimeSpan currentStopperTime)
        {
            AnimationRouteFrameSlider slider = new AnimationRouteFrameSlider();
            //if(currentStopperTime < AnimationStartTime)
            //{
            //    if(Frames != null && Frames.Count > 0)
            //        slider.NextFrame = Frames[0];
            //    return slider;
            //}

            TimeSpan time = currentStopperTime;// - AnimationStartTime;//lub odwrotnie
            bool breaked = false;
            for (int i = 0; i < Frames.Count; i++)
            {
                var frame = Frames[i];
                if (time < frame.FrameTime)
                {
                    if (i - 1 >= 0)
                        slider.PreviousFrame = Frames[i - 1];
                    slider.CurrentFrame = frame;
                    if (i + 1 < Frames.Count)
                        slider.NextFrame = Frames[i + 1];

                    breaked = true;
                    break;
                }
                else
                {
                    time -= frame.FrameTime;
                }
            }

            if (!breaked)
                slider.PreviousFrame = Frames.LastOrDefault();

            return slider;
        }

        public void DuplicateLastFrame(TimeSpan ts)
        {
            var slider = GetFrameSlider(ts);
            var newLastFrame = new AnimationRouteFrame();
            if(slider.PreviousFrame != null)
            {
                newLastFrame.Amplitude = slider.PreviousFrame.Amplitude;
                newLastFrame.CheckpointPosition = slider.PreviousFrame.CheckpointPosition;
                newLastFrame.Cycles = slider.PreviousFrame.Cycles;
                newLastFrame.SpeedAmplitude = slider.PreviousFrame.SpeedAmplitude;
                newLastFrame.SpeedCycles = slider.PreviousFrame.SpeedCycles;
                newLastFrame.SpeedPhase = slider.PreviousFrame.SpeedPhase;
            }
            newLastFrame.FrameTime = ts;
            Frames.Add(newLastFrame);
        }
    }
}
