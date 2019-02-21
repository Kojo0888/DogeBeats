using DogeBeats.EngineSections.Shared;
using DogeBeats.Modules.TimeLines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DogeBeatsTests.EngineSections.TimeLineEditing.TLEPanels
{
    public class TLEPanelTimeGraphicIndicatorTests
    {
        TLEPanelTimeGraphicIndicator TimeIndicator;

        public TLEPanelTimeGraphicIndicatorTests()
        {
            TimeIndicator = new TLEPanelTimeGraphicIndicator(100, 100, new TimeSpan(0,0,0,0), new TimeSpan(0,0,10,0));
        }

        [Fact]
        public void MovePrecentage()
        {
            TimeIndicator.StartTime = new TimeSpan(0, 0, 10);
            TimeIndicator.EndTime = new TimeSpan(0, 0, 20);
            TimeIndicator.MovePrecentage(0.5f);
            var time = TimeIndicator.GetTime();
            if (time == null)
                throw new NesuException("Time is null");
            if (time.Seconds != 15)
                throw new NesuException("Time has " + time.Seconds + " seconds");
        }

        [Fact]
        public void MovePosition()
        {
            TimeIndicator.StartTime = new TimeSpan(0, 0, 10);
            TimeIndicator.EndTime = new TimeSpan(0, 0, 20);

            TimeIndicator.MaxWidth = 1000;

            TimeIndicator.MovePosition(500);
            var time = TimeIndicator.GetTime();
            if (time == null)
                throw new NesuException("Time is null");
            if (time.Seconds != 15)
                throw new NesuException("Time has " + time.Seconds + " seconds");
        }
    }
}
