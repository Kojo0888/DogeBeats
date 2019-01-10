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
        public DDictionary<string, TimeLine> TimeLines { get; set; }

        public void LoadAllTimeLines()
        {
            TimeLines = StaticHub.ResourceManager.GetAllOfSerializedObjects<TimeLine>("TimeLines");
        }

        public void SaveAllTimeLines()
        {
            StaticHub.ResourceManager.SetAllOfSerializedObjects<TimeLine>("TimeLines", TimeLines);
        }

        public void SaveTimeLine(TimeLine timeLine)
        {
            if (!TimeLines.ContainsKey(timeLine.TimeLineName))
                TimeLines.Add(timeLine.TimeLineName, timeLine);
            else if (!ReferenceEquals(timeLine, TimeLines[timeLine.TimeLineName]))
                TimeLines[timeLine.TimeLineName] = timeLine;

            StaticHub.ResourceManager.SetAllOfSerializedObjects("TimeLines", new Dictionary<string, TimeLine>() { { timeLine.TimeLineName, timeLine } });
        }

        public TimeLine GetTimeLine(string timeLineName)
        {
            if (TimeLines.ContainsKey(timeLineName))
                return TimeLines[timeLineName];
            else
                return null;
        }

        public DDictionary<string, AnimationGroupElement> GetAllAnimationGroupElements()
        {
            DDictionary<string, AnimationGroupElement> dic = new DDictionary<string, AnimationGroupElement>();
            foreach (var TimeLine in TimeLines)
            {
                var AnimationGroupElements = TimeLine.Value.AnimationGroupElementsAll;
                if (AnimationGroupElements == null)
                    continue;
                dic.AddRange(AnimationGroupElements.Where(w => !string.IsNullOrEmpty(w.GroupName)).ToDictionary(d => d.GroupName));
            }
            return dic;
        }

        public DDictionary<string, TimeLine> GetAllTimeLines()
        {
            return TimeLines;
        }
    }
}
