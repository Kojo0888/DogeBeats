using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.Modules.TimeLines
{
    public class Beat : ITLEPanelCellElement, IGraphicElement
    {
        public TimeSpan Timestamp { get; set; }
        public static TimeSpan DurationTime {get;set;}

        public string GraphicName { get; set; }
        public Placement Placement { get; set; }

        static Beat()
        {
            DurationTime = new TimeSpan(0, 0, 0, 0, 50);
        }

        public TimeSpan GetDurationTime()
        {
            return DurationTime;
        }

        public TimeSpan GetStartTime()
        {
            return Timestamp;
        }

        public void SetStartTime(TimeSpan timeSpan)
        {
            Timestamp = timeSpan;
        }
    }
}
