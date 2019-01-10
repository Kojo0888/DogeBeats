using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogeBeats.EngineSections.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;
using DogeBeatsTests;

namespace DogeBeats.EngineSections.Resources.Tests
{
    [TestClass()]
    public class TimeLineCentreTests
    {
        private TimeLineCentre TimeLineCentre { get; set; }

        [TestInitialize]
        public void Init()
        {
            TimeLineCentre = new TimeLineCentre();
            TimeLineCentre.LoadAllTimeLines();

            StaticHub.ResourceManager.LoadAllResources();
        }

        [TestMethod()]
        public void LoadAllTimeLinesTest()
        {
            TimeLineCentre = new TimeLineCentre();
            TimeLineCentre.LoadAllTimeLines();
        }

        [TestMethod()]
        public void SaveAllTimeLinesTest()
        {
            TimeLineCentre.SaveAllTimeLines();
        }

        [TestMethod()]
        public void GetAllAnimationGroupElementsTest()
        {
            TimeLine timeLine = MockObjects.GetTimeLine();
            TimeLineCentre.SaveTimeLine(timeLine);

            var groups = TimeLineCentre.GetAllAnimationGroupElements();
            if (groups == null || groups.Count == 0)
                Assert.Fail();
        }

        [TestMethod()]
        public void SaveTimeLineTest()
        {
            TimeLine timeLine = MockObjects.GetTimeLine();
            TimeLineCentre.SaveTimeLine(timeLine);

            if (!StaticHub.ResourceManager.Resources.ContainsKey("timelines") || !StaticHub.ResourceManager.Resources["timelines"].ContainsKey(timeLine.TimeLineName))
                Assert.Fail();
        }

        [TestMethod()]
        public void GetTimeLineTest()
        {
            TimeLine timeLine = MockObjects.GetTimeLine();
            TimeLineCentre.SaveTimeLine(timeLine);

            TimeLine timeLine2 = TimeLineCentre.GetTimeLine(timeLine.TimeLineName);
            if (timeLine2 == null)
                Assert.Fail();
        }

        [TestMethod()]
        public void GetAllTimeLinesTest()
        {
            TimeLineCentre.LoadAllTimeLines();
            var timeLine = TimeLineCentre.GetAllTimeLines();
            if (timeLine == null || timeLine.Count == 0)
                Assert.Fail();
        }
    }
}