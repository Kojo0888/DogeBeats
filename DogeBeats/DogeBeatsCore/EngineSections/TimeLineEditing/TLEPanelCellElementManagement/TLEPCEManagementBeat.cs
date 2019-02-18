using DogeBeats.EngineSections.TimeLineEditing.TLEPanels;
using DogeBeats.Modules;
using DogeBeats.Modules.TimeLines;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.EngineSections.TimeLineEditing.TLEPanelCellElementManagement
{
    public class TLEPCEManagementBeat : ITLEPanelCellElementManagement
    {
        TimeLineEditor ParentTLE;

        public TLEPCEManagementBeat(TimeLineEditor parent)
        {
            ParentTLE = parent;
        }

        public void AddNewElement()
        {
            var timeSpan = ParentTLE.PanelHub.TimeIdentyficator.SelectedTime;

            if (timeSpan != ParentTLE.TimeLine.Stopper.Elapsed)
                throw new Exception("Nesu: Time Spans are not matched");

            ParentTLE.TimeLine.BeatGuider.RegisterBeat(timeSpan);

            ParentTLE.TimeLine.Refresh();

            ParentTLE.PanelHub.InitializePanel(TLEPanelNames.BEAT, ParentTLE.TimeLine.BeatGuider.GetTLECellElements());
        }

        public void MoveElement()
        {
            var timeSpan = ParentTLE.PanelHub.TimeIdentyficator.SelectedTime;

            if (timeSpan != ParentTLE.TimeLine.Stopper.Elapsed)
                throw new Exception("Nesu: Time Spans are not matched");

            var selectedBeat = ParentTLE.PanelHub.GetPanel(TLEPanelNames.BEAT)?.SelectedPanelCell.ReferenceElement;
            if (selectedBeat == null)
                return;

            var beat = selectedBeat as Beat;
            if (beat == null)
                return;

            ParentTLE.TimeLine.BeatGuider.RemoveBeat(beat);
            //ParentTLE.TimeLine.BeatGuider.RemoveBeat(beat.Timestamp);
            ParentTLE.TimeLine.BeatGuider.RegisterBeat(timeSpan);

            ParentTLE.TimeLine.Refresh();

            ParentTLE.PanelHub.InitializePanel(TLEPanelNames.BEAT, ParentTLE.TimeLine.BeatGuider.GetTLECellElements());
        }

        public void RemoveElement()
        {
            var timeSpan = ParentTLE.PanelHub.TimeIdentyficator.SelectedTime;

            if (timeSpan != ParentTLE.TimeLine.Stopper.Elapsed)
                throw new Exception("Nesu: Time Spans are not matched");

            ParentTLE.TimeLine.BeatGuider.RemoveBeat(timeSpan);

            ParentTLE.TimeLine.Refresh();

            ParentTLE.PanelHub.InitializePanel(TLEPanelNames.BEAT, ParentTLE.TimeLine.BeatGuider.GetTLECellElements());
        }

        public void UpdateElement(NameValueCollection values)
        {
            return;
        }
    }
}
