using DogeBeats.Modules.TimeLines;
using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Testowy.Model
{
    //struct candidate
    public class AnimationRouteFrame : ITLEPanelElement
    {
        public Placement Placement { get; set; }
        public EasingMode Ease { get; set; }
        public TimeSpan TimeLength { get; set; }

        internal static IEnumerable<string> GetKeysManualUpdate()
        {
            List<string> keys = new List<string>();
            keys.Add("Ease");
            keys.Add("RunningTime");
            return keys;
        }

        public static void ManualUpdate(AnimationRouteFrame frame, NameValueCollection values)
        {
            if (!string.IsNullOrEmpty(values["Ease"].ToString()))
                frame.Ease = ManualUpdateEase(values["Ease"]);

            if (!string.IsNullOrEmpty(values["RunningTime"].ToString()))
                frame.TimeLength = ManualUpdateTime(values["RunningTime"]);
        }

        private static TimeSpan ManualUpdateTime(string v)
        {
            TimeSpan temp;
            if(TimeSpan.TryParse(v, out temp))
            {
                return temp;
            }
            return new TimeSpan();
        }

        private static EasingMode ManualUpdateEase(string v)
        {
            switch (v)
            {
                case "In":
                    return EasingMode.EaseIn;
                case "Out":
                    return EasingMode.EaseOut;
                case "InOut":
                    return EasingMode.EaseInOut;
            }

            return EasingMode.EaseInOut;
        }
    }
}
