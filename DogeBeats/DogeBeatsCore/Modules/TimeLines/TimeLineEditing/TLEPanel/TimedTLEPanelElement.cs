using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.Modules.TimeLines
{
    public class TimedTLEPanelElement
    {
        public TimeSpan Timestamp { get; set; }
        public ITLEPanelElement Object { get; set; }

        public static List<TimedTLEPanelElement> Parse(List<AnimationGroupElement> elements)
        {
            List<TimedTLEPanelElement> timedElements = new List<TimedTLEPanelElement>();
            foreach (var element in elements)
            {
                TimedTLEPanelElement timedElem = new TimedTLEPanelElement();
                timedElem.Object = element;
                timedElem.Timestamp = element.GroupRoute.AnimationStartTime;
                timedElements.Add(timedElem);
            }
            return timedElements;
        }

        public static List<TimedTLEPanelElement> Parse(List<AnimationElement> elements)
        {
            List<TimedTLEPanelElement> timedElements = new List<TimedTLEPanelElement>();
            foreach (var element in elements)
            {
                TimedTLEPanelElement timedElem = new TimedTLEPanelElement();
                timedElem.Object = element;
                timedElem.Timestamp = element.Route.AnimationTime;
                timedElements.Add(timedElem);
            }
            return timedElements;
        }

        public static List<TimedTLEPanelElement> Parse(List<Beat> elements)
        {
            List<TimedTLEPanelElement> timedElements = new List<TimedTLEPanelElement>();
            foreach (var element in elements)
            {
                TimedTLEPanelElement timedElem = new TimedTLEPanelElement();
                timedElem.Object = element;
                timedElem.Timestamp = element.Timestamp;
                timedElements.Add(timedElem);
            }
            return timedElements;
        }

        public static TimedTLEPanelElement Parse(Beat element)
        {
            TimedTLEPanelElement timedElem = new TimedTLEPanelElement();
            timedElem.Object = element;
            timedElem.Timestamp = element.Timestamp;
            return timedElem;
        }

        public static TimedTLEPanelElement Parse(AnimationElement element)
        {
            TimedTLEPanelElement timedElem = new TimedTLEPanelElement();
            timedElem.Object = element;
            timedElem.Timestamp = element.Route.AnimationTime;
            return timedElem;
        }

        public static TimedTLEPanelElement Parse(AnimationGroupElement element)
        {
            TimedTLEPanelElement timedElem = new TimedTLEPanelElement();
            timedElem.Object = element;
            timedElem.Timestamp = element.GroupRoute.AnimationTime;
            return timedElem;
        }
    }
}
