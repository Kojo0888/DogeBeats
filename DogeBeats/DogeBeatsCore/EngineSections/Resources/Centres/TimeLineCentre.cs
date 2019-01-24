using DogeBeats.EngineSections.AnimationObjects;
using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.EngineSections.Resources
{
    public class TimeLineCentre : CentreSerializationBase<TimeLine>
    {
        public TimeLineCentre() : base ("TimeLines")
        {
        }

        public DDictionary<string, AnimationGroupElement> GetAllAnimationGroupElements()
        {
            DDictionary<string, AnimationGroupElement> dic = new DDictionary<string, AnimationGroupElement>();
            foreach (var TimeLine in base.CentreElements)
            {

                var AnimationGroupElements = TimeLine.Value.GetAllAnimationGroupElements();
                if (AnimationGroupElements == null)
                    continue;

                dic.AddRange(AnimationGroupElements.Where(w => !string.IsNullOrEmpty(w.Name)).ToDictionary(d => d.Name));
            }
            return dic;
        }
    }
}
