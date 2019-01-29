using DogeBeats.Modules.TimeLines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DogeBeats.Modules.TimeLines.Tests
{
    public class BeatGuideTests
    {
        private BeatGuide guide = new BeatGuide();

        public BeatGuideTests()
        {
            guide = new BeatGuide();
        }


        [Fact]
        public void RegisterBeatTest()
        {
            var ts = new TimeSpan(0, 0, 0, 1);
            guide.RegisterBeat(ts);
            if (guide.Beats.Count == 0 || guide.Beats[0].Timestamp != ts)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void RemoveBeatTest()
        {
            var ts = new TimeSpan(0, 0, 0, 1);
            guide.RegisterBeat(ts);
            guide.RemoveBeat(ts);
            if (guide.Beats.Count > 0)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void RemoveBeatTest2()
        {
            var ts = new TimeSpan(0, 0, 0, 1);
            guide.RegisterBeat(ts);
            guide.RemoveBeat(ts + Beat.DurationTime);
            if (guide.Beats.Count == 0)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void RemoveBeatTest3()
        {
            var ts = new TimeSpan(0, 0, 0, 1);
            guide.RegisterBeat(ts);
            guide.RemoveBeat(ts + Beat.DurationTime - new TimeSpan(0,0,0,0,1));
            if (guide.Beats.Count > 0)
                throw new Exception("Assert Fails");
        }
    }
}