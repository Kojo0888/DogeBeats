using DogeBeats.EngineSections.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;
using DogeBeatsTests;
using DogeBeatsTests.Data;
using Xunit;

namespace DogeBeats.EngineSections.Resources.Tests
{
    [Collection("Synchronical")]
    public class TimeLineCentreTests
    {
        private TimeLineCentre TimeLineCentre { get; set; }

        public TimeLineCentreTests()
        {
            TimeLineCentre = new TimeLineCentre();
            TimeLineCentre.LoadAll();

            StaticHub.ResourceManager.LoadAllResources();
        }

        [Fact]
        public void LoadAllTimeLinesTest()
        {
            TimeLineCentre = new TimeLineCentre();
            TimeLineCentre.LoadAll();
        }

        [Fact]
        public void SaveAllTimeLinesTest()
        {
            TimeLineCentre.SaveAll();
        }

        [Fact]
        public void GetAllAnimationGroupElementsTest()//Fix this test
        {
            TimeLine timeLine = MockObjects.GetTimeLine();
            TimeLineCentre.Save(timeLine);

            var groups = TimeLineCentre.GetAllAnimationGroupElements();
            if (groups == null || groups.Count == 0)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void SaveTimeLineTest()
        {
            TimeLine timeLine = MockObjects.GetTimeLine();
            TimeLineCentre.Save(timeLine);

            if (StaticHub.ResourceManager.GetResource("TimeLines", timeLine.Name) == null)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void GetTimeLineTest()
        {
            TimeLine timeLine = MockObjects.GetTimeLine();
            TimeLineCentre.Save(timeLine);

            TimeLine timeLine2 = TimeLineCentre.Get(timeLine.Name);
            if (timeLine2 == null)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void GetAllTimeLinesTest()
        {
            TimeLineCentre.LoadAll();
            var timeLine = TimeLineCentre.GetAll();
            if (timeLine == null || timeLine.Count == 0)
                throw new Exception("Assert Fails");
        }
    }
}