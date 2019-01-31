using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.Modules.TimeLines
{
    public class TLEPanelTimeGraphicIndicator : IGraphicElement
    {
        public Placement Placement { get; set; }
        public string GraphicName { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public float MaxWidth { get; set; }

        public TLEPanelTimeGraphicIndicator(float identificatorHeight, TimeSpan startTime, TimeSpan endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
            Placement = new Placement()
            {
                Height = identificatorHeight,
                Width = 3,
                Rotation = 0,
                X = 0,
                Y = 0
            };
        }

        public void Move(float x)
        {
            if (x > MaxWidth - Placement.Width)
                x = MaxWidth - Placement.Width;

            Placement.X = x;
        }

        public TimeSpan GetTime()
        {
            float precentage = Placement.X * MaxWidth;
            var diffTicks = EndTime.Ticks - StartTime.Ticks;
            var allTicks = StartTime.Ticks + (long)(diffTicks * precentage);
            return new TimeSpan(allTicks);
        }
    }
}
