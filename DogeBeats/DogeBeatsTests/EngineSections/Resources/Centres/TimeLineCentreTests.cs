using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogeBeats.EngineSections.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;
using DogeBeatsTests;
using DogeBeatsTests.Data;

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
            TimeLineCentre.LoadAll();

            StaticHub.ResourceManager.LoadAllResources();
        }

        [TestMethod()]
        public void LoadAllTimeLinesTest()
        {
            TimeLineCentre = new TimeLineCentre();
            TimeLineCentre.LoadAll();
        }

        [TestMethod()]
        public void SaveAllTimeLinesTest()
        {
            TimeLineCentre.SaveAll();
        }

        [TestMethod()]
        public void GetAllAnimationGroupElementsTest()
        {
            TimeLine timeLine = MockObjects.GetTimeLine();
            TimeLineCentre.Save(timeLine);

            var groups = TimeLineCentre.GetAllAnimationGroupElements();
            if (groups == null || groups.Count == 0)
                Assert.Fail();
        }

        [TestMethod()]
        public void SaveTimeLineTest()
        {
            TimeLine timeLine = MockObjects.GetTimeLine();
            TimeLineCentre.Save(timeLine);

            if (StaticHub.ResourceManager.GetResource("TimeLines", timeLine.Name) == null)
                Assert.Fail();
        }

        [TestMethod()]
        public void GetTimeLineTest()
        {
            TimeLine timeLine = MockObjects.GetTimeLine();
            TimeLineCentre.Save(timeLine);

            TimeLine timeLine2 = TimeLineCentre.Get(timeLine.Name);
            if (timeLine2 == null)
                Assert.Fail();
        }

        [TestMethod()]
        public void GetAllTimeLinesTest()
        {
            TimeLineCentre.LoadAll();
            var timeLine = TimeLineCentre.GetAll();
            if (timeLine == null || timeLine.Count == 0)
                Assert.Fail();
        }
    }
}