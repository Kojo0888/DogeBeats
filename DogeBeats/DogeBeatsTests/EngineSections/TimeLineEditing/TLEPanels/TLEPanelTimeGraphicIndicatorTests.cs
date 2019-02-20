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
            throw new NotImplementedException();
        }

        [Fact]
        public void MovePosition()
        {
            throw new NotImplementedException();
        }
    }
}
