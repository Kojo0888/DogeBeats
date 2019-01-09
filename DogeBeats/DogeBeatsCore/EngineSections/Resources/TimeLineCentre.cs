using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.EngineSections.Resources
{
    public class TimeLineCentre
    {
        public Dictionary<string, TimeLine> TimeLines { get; set; }

        public void LoadAllTimeLines()
        {
            TimeLines = StaticHub.ResourceManager.GetAllOfSerializedObjects<TimeLine>("TimeLines");
        }

        public void SaveAllTimeLines()
        {
            StaticHub.ResourceManager.SetAllOfSerializedObjects<TimeLine>("TimeLines", TimeLines);
        }

        public DDictionary<string, AnimationGroupElement> GetAllAnimationGroupElements()
        {
            DDictionary<string, AnimationGroupElement> dic = new DDictionary<string, AnimationGroupElement>();
            foreach (var TimeLine in TimeLines)
            {
                var AnimationGroupElements = TimeLine.Value.GetAllAnimationGroupElements();
                dic.AddRange(AnimationGroupElements);
            }
            return dic;
        }
    }
}
