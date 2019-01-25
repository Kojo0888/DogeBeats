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

        internal Placement CalculatePlacement(TimeSpan currentStopperTime)
        {
            var frameSlider = GetFrameSlider(currentStopperTime);
            Placement newPlacement = new Placement();

            //TODO: Remake is needed
            //newPlacement.X += 
            
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

            for (int i = 0; i < Frames.Count; i++)
            {
                var frame = Frames[i];
                if (currentStopperTime < frame.FrameTime)
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


        public static IEnumerable<string> GetKeysManualUpdate()
        {
            List<string> keys = new List<string>();
            keys.Add("Name");
            keys.Add("AnimationStartTime");
            keys.Add("StartPlacement.X");
            keys.Add("StartPlacement.Y");
            keys.Add("StartPlacement.Width");
            keys.Add("StartPlacement.Height");
            keys.Add("StartPlacement.Rotation");
            return keys;
        }

        public void UpdateManual(NameValueCollection values)
        {
            if (!string.IsNullOrEmpty(values["CheckpointPosition.X"].ToString()))
                Name = ManualUpdaterParser.ParseString(values["Name"]);
            else if (!string.IsNullOrEmpty(values["CheckpointPosition.X"].ToString()))
                StartPlacement.X = ManualUpdaterParser.ParseFloat(values["CheckpointPosition.X"]);
            else if (!string.IsNullOrEmpty(values["CheckpointPosition.Y"].ToString()))
                StartPlacement.Y = ManualUpdaterParser.ParseFloat(values["CheckpointPosition.Y"]);
            else if (!string.IsNullOrEmpty(values["CheckpointPosition.Width"].ToString()))
                StartPlacement.Width = ManualUpdaterParser.ParseFloat(values["CheckpointPosition.Width"]);
            else if (!string.IsNullOrEmpty(values["CheckpointPosition.Height"].ToString()))
                StartPlacement.Height = ManualUpdaterParser.ParseFloat(values["CheckpointPosition.Height"]);
            else if (!string.IsNullOrEmpty(values["CheckpointPosition.Rotation"].ToString()))
                StartPlacement.Rotation = ManualUpdaterParser.ParseFloat(values["CheckpointPosition.Rotation"]);
            else if (!string.IsNullOrEmpty(values["AnimationStartTime"].ToString()))
                AnimationStartTime = ManualUpdaterParser.ParseTimeSpan(values["AnimationStartTime"]);
        }
    }
}
