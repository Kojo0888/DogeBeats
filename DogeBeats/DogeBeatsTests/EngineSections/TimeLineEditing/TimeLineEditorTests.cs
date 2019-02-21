using DogeBeats.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Testowy.Model;

namespace DogeBeatsTests.EngineSections.TimeLineEditing
{
    public class TimeLineEditorTests
    {
        TimeLineEditor TLE;// = new TimeLineEditor();

        public TimeLineEditorTests()
        {
            TLE = new TimeLineEditor();

            TimeLine tl = GetTimeLine();

            TLE.AttachTimeLineToEditor(tl);
        }

        private TimeLine GetTimeLine()
        {
            TimeLine tl = new TimeLine();

            tl.AnimationElements = new List<DogeBeats.EngineSections.AnimationObjects.IAnimationElement>();

            return tl;
        }

        [Fact]
        public void InitializePanelCellManagements()
        {
            TLE.InitializePanelCellManagements();
            if (TLE.PanelCellManagements == null)
                throw new Exception("PanelCellManagements is null");
            if (TLE.PanelCellManagements.Count != 3)
                throw new Exception("PanelCellManagements.Count is != 3");
        }

        [Fact]
        public void SetTimeCursorToPrecentage()
        {
            TLE.PanelHub.TimeIdentyficator.StartTime = new TimeSpan(0, 0, 10);
            TLE.PanelHub.TimeIdentyficator.EndTime = new TimeSpan(0, 0, 30);

            TLE.SetTimeCursorToPrecentage(0.5f);

            var time = TLE.TimeLine.Stopper.Elapsed;

            if (time.Seconds != 20)
                throw new Exception("Time has "+time.Seconds+" seconds");
        }

        [Fact]
        public void MoveForwardTimeScope()
        {
            TLE.PanelHub.PanelOffsetTime = new TimeSpan(0, 0, 10);
            TLE.PanelHub.PanelWidthTime = new TimeSpan(0, 0, 11);
            TLE.MoveForwardTimeScope();

            var startTime = TLE.PanelHub.TimeIdentyficator.StartTime;
            var endTime = TLE.PanelHub.TimeIdentyficator.EndTime;

            if (startTime.Seconds != 21)
                throw new Exception("Start Time has " + startTime.Seconds + " seconds");
            if (endTime.Seconds != 32)
                throw new Exception("End Time has " + startTime.Seconds + " seconds");
        }

        [Fact]
        public void MoveBackwardTimeScope()
        {
            TLE.PanelHub.PanelOffsetTime = new TimeSpan(0, 0, 10);
            TLE.PanelHub.PanelWidthTime = new TimeSpan(0, 0, 11);
            TLE.MoveBackwardTimeScope();

            var startTime = TLE.PanelHub.TimeIdentyficator.StartTime;
            var endTime = TLE.PanelHub.TimeIdentyficator.EndTime;

            if (startTime.Seconds != 0)
                throw new Exception("Start Time has " + startTime.Seconds + " seconds");
            if (endTime.Seconds != 11)
                throw new Exception("End Time has " + startTime.Seconds + " seconds");

        }

        [Fact]
        public void MoveBackwardTimeScope2()
        {
            TLE.PanelHub.PanelOffsetTime = new TimeSpan(0, 0, 20);
            TLE.PanelHub.PanelWidthTime = new TimeSpan(0, 0, 11);
            TLE.MoveBackwardTimeScope();

            var startTime = TLE.PanelHub.TimeIdentyficator.StartTime;
            var endTime = TLE.PanelHub.TimeIdentyficator.EndTime;

            if (startTime.Seconds != 9)
                throw new Exception("Start Time has " + startTime.Seconds + " seconds");
            if (endTime.Seconds != 20)
                throw new Exception("End Time has " + startTime.Seconds + " seconds");

        }

        [Fact]
        public void UpdateTimeScope()
        {
            TLE.UpdateTimeScope(new TimeSpan(0, 0, 9), new TimeSpan(0, 0, 20));

            var startTime = TLE.PanelHub.TimeIdentyficator.StartTime;
            var endTime = TLE.PanelHub.TimeIdentyficator.EndTime;

            if (startTime.Seconds != 9)
                throw new Exception("Start Time has " + startTime.Seconds + " seconds");
            if (endTime.Seconds != 20)
                throw new Exception("End Time has " + startTime.Seconds + " seconds");
        }
    }
}
