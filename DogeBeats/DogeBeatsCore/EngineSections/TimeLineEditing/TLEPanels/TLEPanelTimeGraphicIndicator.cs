using DogeBeats.EngineSections.Resources;
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

        public float SelectedPrecentage { get; set; }
        public TimeSpan SelectedTime { get; private set; }

        public float MaxWidth { get; set; }

        public TLEPanelTimeGraphicIndicator(float identificatorHeight, int maxWidth, TimeSpan startTime, TimeSpan endTime)
        {
            MaxWidth = maxWidth;

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

        public void UpdateHeight(float height)
        {
            Placement.Height = height;
        }

        public void UpdateTimeScope(TimeSpan startTime, TimeSpan endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public void MoveTimeScope(TimeSpan timeDuration)
        {
            StartTime += timeDuration;
            EndTime += timeDuration;
        }

        public void MovePrecentage(float precentage)//0.12
        {
            SelectedPrecentage = precentage;
            var ticks = (EndTime.Ticks - StartTime.Ticks);
            SelectedTime = new TimeSpan((long)(ticks * (double)precentage));

            Placement.X = precentage * MaxWidth;
        }

        public void MovePosition(float x)
        {
            if (x < 0 || x > MaxWidth)
                return;

            var precentage = x / MaxWidth;

            MovePrecentage(precentage);
        }
    }
}
