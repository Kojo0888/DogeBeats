using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogeBeats.Modules.TimeLines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.Modules.TimeLines.Tests
{
    [TestClass()]
    public class BeatGuideTests
    {
        private BeatGuide guide = new BeatGuide();

        [TestInitialize]
        public void Init()
        {
            guide = new BeatGuide();
        }


        [TestMethod()]
        public void RegisterBeatTest()
        {
            var ts = new TimeSpan(0, 0, 0, 1);
            guide.RegisterBeat(ts);
            if (guide.Beats.Count == 0 || guide.Beats[0].Timestamp != ts)
                Assert.Fail();
        }

        [TestMethod()]
        public void RemoveBeatTest()
        {
            var ts = new TimeSpan(0, 0, 0, 1);
            guide.RegisterBeat(ts);
            guide.RemoveBeat(ts);
            if (guide.Beats.Count > 0)
                Assert.Fail();
        }

        [TestMethod()]
        public void RemoveBeatTest2()
        {
            var ts = new TimeSpan(0, 0, 0, 1);
            guide.RegisterBeat(ts);
            guide.RemoveBeat(ts + Beat.DurationTime);
            if (guide.Beats.Count == 0)
                Assert.Fail();
        }

        [TestMethod()]
        public void RemoveBeatTest3()
        {
            var ts = new TimeSpan(0, 0, 0, 1);
            guide.RegisterBeat(ts);
            guide.RemoveBeat(ts + Beat.DurationTime - new TimeSpan(0,0,0,0,1));
            if (guide.Beats.Count > 0)
                Assert.Fail();
        }
    }
}