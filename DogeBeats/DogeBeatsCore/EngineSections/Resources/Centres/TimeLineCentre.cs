﻿using DogeBeats.Other;
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
        public TimeLineCentre()
        {
            ResourceType = "TimeLines";
        }

        public DDictionary<string, AnimationGroupElement> GetAllAnimationGroupElements()
        {
            DDictionary<string, AnimationGroupElement> dic = new DDictionary<string, AnimationGroupElement>();
            foreach (var TimeLine in base.CentreElements)
            {
                var AnimationGroupElements = TimeLine.Value.AnimationGroupElementsAll;
                if (AnimationGroupElements == null)
                    continue;
                dic.AddRange(AnimationGroupElements.Where(w => !string.IsNullOrEmpty(w.Name)).ToDictionary(d => d.Name));
            }
            return dic;
        }
    }
}