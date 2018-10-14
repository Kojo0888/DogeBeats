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

        public static void Update(Placement placement, NameValueCollection values)
        {
            float tempfloat;
            if (!string.IsNullOrEmpty(values["X"]) && float.TryParse(values["X"], out tempfloat))
                placement.X = tempfloat;
            if (!string.IsNullOrEmpty(values["Y"]) && float.TryParse(values["Y"], out tempfloat))
                placement.Y = tempfloat;
            if (!string.IsNullOrEmpty(values["Width"]) && float.TryParse(values["Width"], out tempfloat))
                placement.Width = tempfloat;
            if (!string.IsNullOrEmpty(values["Height"]) && float.TryParse(values["Height"], out tempfloat))
                placement.Height = tempfloat;
            if (!string.IsNullOrEmpty(values["Rotation"]) && float.TryParse(values["Rotation"], out tempfloat))
                placement.Rotation = tempfloat;
        }

        public static List<string> GetKeysForUpdate()
        {
            return new List<string>() { "X", "Y", "Width", "Height", "Rotation"};
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
