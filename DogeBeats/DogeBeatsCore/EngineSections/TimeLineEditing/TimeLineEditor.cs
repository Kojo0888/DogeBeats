using DogeBeats.EngineSections.AnimationObjects;
using DogeBeats.EngineSections.Resources;
using DogeBeats.EngineSections.Shared;
using DogeBeats.EngineSections.TimeLineEditing;
using DogeBeats.EngineSections.TimeLineEditing.TLEPanelCellElementManagement;
using DogeBeats.EngineSections.TimeLineEditing.TLEPanels;
using DogeBeats.Model;
using DogeBeats.Modules.TimeLines;
using DogeBeats.Renderer;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.Modules
{
    public class TimeLineEditor
    {
        public TimeLine TimeLine { get; set; }

        public TLEPanelHub PanelHub { get; set; } = new TLEPanelHub();

        public Dictionary<string, ITLEPanelCellElementManagement> PanelCellManagements { get; set; } = new Dictionary<string, ITLEPanelCellElementManagement>();

        public TimeLineEditor()
        {
            InitializePanelCellManagements();
        }

        public void InitializePanelCellManagements()
        {
            PanelCellManagements.Clear();
            PanelCellManagements.Add(TLEPanelNames.BEAT, new TLEPCEManagementBeat(this));
            PanelCellManagements.Add(TLEPanelNames.ANIMATION_ROUTE, new TLEPCEManagementAnimationRoute(this));
            PanelCellManagements.Add(TLEPanelNames.ANIMATION_ELEMENT_PREFIX, new TLEPCEManagementAnimationElement(this));
        }

        public void AttachTimeLineToEditor(TimeLine timeline)
        {
            TimeLine = timeline;
        }

        public void Play()
        {
            TimeLine.ResumeStoryboard();
        }

        public void PlayFromTo(TimeSpan from, TimeSpan to)
        {
            TimeLine.Stopper.Stop();
            TimeLine.Stopper.Elapsed = from;
            TimeLine.RegisterPlayToTimeSpan(to);
            TimeLine.Stopper.Start();
        }

        public void Stop()
        {
            TimeLine.PauseStoryboard(false);
        }

        public void ChangeStopperTime(TimeSpan newTimespamp)
        {
            if (TimeLine.Stopper.IsRunning)
            {
                TimeLine.Stopper.Stop();
                TimeLine.Stopper.Elapsed = newTimespamp;
                TimeLine.Stopper.Start();
            }
            else
            {
                TimeLine.Stopper.Elapsed = newTimespamp;
            }
        }

        public void SetTimeCursorToPrecentage(float precentage)
        {
            var time = PanelHub.SetTimeCursorToPrecentage(precentage);
            bool wasRunning = TimeLine.Stopper.IsRunning;
            TimeLine.Stopper.Stop();
            TimeLine.Stopper.Elapsed = time;
            if (wasRunning)
                TimeLine.Stopper.Start();
            //PanelHub.SetTimeCursorToPrecentage(precentage);
        }

        public void SaveTimeLine()
        {
            StaticHub.TimeLineCentre.Save(TimeLine);
        }

        public void LoadTimeLine(string timelineName)
        {
            TimeLine timeline = StaticHub.TimeLineCentre.Get(timelineName);
            if (timeline != null)
                AttachTimeLineToEditor(timeline);
        }

        public void MoveForwardTimeScope()
        {
            PanelHub.MoveForwardTimeScope();
        }

        public void MoveBackwardTimeScope()
        {
            PanelHub.MoveBackwardTimeScope();
        }

        public void UpdateTimeScope(TimeSpan from, TimeSpan to)
        {
            PanelHub.PanelOffsetTime = from;
            PanelHub.PanelWidthTime = new TimeSpan(to.Ticks - from.Ticks);
            PanelHub.UpdateTimeScope();
        }
    }
}
