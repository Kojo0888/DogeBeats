using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.Modules.TimeLines
{
    public class TimedGraphicElement
    {
        public TimeSpan Timestamp { get; set; }
        public IGraphicElement Object { get; set; }

        public static List<TimedGraphicElement> Parse(List<AnimationGroupElement> elements)
        {
            List<TimedGraphicElement> timedElements = new List<TimedGraphicElement>();
            foreach (var element in elements)
            {
                TimedGraphicElement timedElem = new TimedGraphicElement();
                timedElem.Object = element;
                timedElem.Timestamp = element.GroupRoute.AnimationStartTime;
                timedElements.Add(timedElem);
            }
            return timedElements;
        }

        public static List<TimedGraphicElement> Parse(List<AnimationElement> elements)
        {
            List<TimedGraphicElement> timedElements = new List<TimedGraphicElement>();
            foreach (var element in elements)
            {
                TimedGraphicElement timedElem = new TimedGraphicElement();
                timedElem.Object = element;
                timedElem.Timestamp = element.Route.AnimationTime;
                timedElements.Add(timedElem);
            }
            return timedElements;
        }

        public static List<TimedGraphicElement> Parse(List<Beat> elements)
        {
            List<TimedGraphicElement> timedElements = new List<TimedGraphicElement>();
            foreach (var element in elements)
            {
                TimedGraphicElement timedElem = new TimedGraphicElement();
                timedElem.Object = element;
                timedElem.Timestamp = element.Timestamp;
                timedElements.Add(timedElem);
            }
            return timedElements;
        }

        public static TimedGraphicElement Parse(Beat element)
        {
            TimedGraphicElement timedElem = new TimedGraphicElement();
            timedElem.Object = element;
            timedElem.Timestamp = element.Timestamp;
            return timedElem;
        }

        public static TimedGraphicElement Parse(AnimationElement element)
        {
            TimedGraphicElement timedElem = new TimedGraphicElement();
            timedElem.Object = element;
            timedElem.Timestamp = element.Route.AnimationTime;
            return timedElem;
        }

        public static TimedGraphicElement Parse(AnimationGroupElement element)
        {
            TimedGraphicElement timedElem = new TimedGraphicElement();
            timedElem.Object = element;
            timedElem.Timestamp = element.GroupRoute.AnimationTime;
            return timedElem;
        }
    }
}
