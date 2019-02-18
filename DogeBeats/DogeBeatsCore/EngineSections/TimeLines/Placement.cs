using DogeBeats.EngineSections.Shared;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testowy.Model
{
    public class Placement
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Rotation { get; set; }

        public void UpdateManual(NameValueCollection values)
        {
            X = ManualUpdaterParser.Parse(values["X"], X);
            Y = ManualUpdaterParser.Parse(values["Y"], Y);
            Width = ManualUpdaterParser.Parse(values["Width"], Width);
            Height = ManualUpdaterParser.Parse(values["Height"], Height);
            Rotation = ManualUpdaterParser.Parse(values["Rotation"], Rotation);
        }

        public static List<string> GetKeysManualUpdate()
        {
            return new List<string>() {
                "X",
                "Y",
                "Width",
                "Height",
                "Rotation"
            };
        }

        public static Placement operator+ (Placement p1, Placement p2)
        {
            throw new NotImplementedException();
#pragma warning disable CS0162 // Unreachable code detected
            Placement newPlacement = new Placement();
#pragma warning restore CS0162 // Unreachable code detected
            //newPlacement.X = frameSlider.PreviousFrame.Placement.X + (easeMultiplier * frameSlider.CurrentFrame.Placement.X);

            //newPlacement.Y = frameSlider.PreviousFrame.Placement.Y + (easeMultiplier * frameSlider.CurrentFrame.Placement.Y);

            //newPlacement.Width = frameSlider.PreviousFrame.Placement.Width + (easeMultiplier * frameSlider.CurrentFrame.Placement.Width);

            //newPlacement.Height = frameSlider.PreviousFrame.Placement.Height + (easeMultiplier * frameSlider.CurrentFrame.Placement.Height);

            //newPlacement.Rotation = frameSlider.PreviousFrame.Placement.Rotation + (easeMultiplier * frameSlider.CurrentFrame.Placement.Rotation);
            return newPlacement;
        }

        
        public static Placement operator* (Placement p1, float scalar)
        {
            throw new NotImplementedException();
#pragma warning disable CS0162 // Unreachable code detected
            Placement newPlacement = new Placement();
#pragma warning restore CS0162 // Unreachable code detected
            return newPlacement;
        }
    }
}
