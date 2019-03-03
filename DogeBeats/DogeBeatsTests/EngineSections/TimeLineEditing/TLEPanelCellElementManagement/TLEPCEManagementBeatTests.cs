using DogeBeats.EngineSections.Shared;
using DogeBeats.EngineSections.TimeLineEditing.TLEPanelCellElementManagement;
using DogeBeats.EngineSections.TimeLineEditing.TLEPanels;
using DogeBeats.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DogeBeatsTests.EngineSections.TimeLineEditing.TLEPanelCellElementManagement
{
    public class TLEPCEManagementBeatTests
    {
        TimeLineEditor editor;
        TLEPCEManagementBeat Management;

        public TLEPCEManagementBeatTests()
        {
            editor = new TimeLineEditor();
            var timeLine = MockObjects.GetTimeLine2();
            editor.AttachTimeLineToEditor(timeLine);
            Management = new TLEPCEManagementBeat(editor);
        }

        [Fact]
        public void AddNewElement()
        {
            var beatPanel = editor.PanelHub.GetPanel(TLEPanelNames.BEAT);
            var panelCell = beatPanel.PanelCells.FirstOrDefault();
            Management.ParentTLE.PanelHub.SelectPanelCell(panelCell);

            beatPanel = editor.PanelHub.GetPanel(TLEPanelNames.BEAT);
            if (beatPanel == null)
                throw new NesuException("PreCheck: Panel is null");
            var panelCells = beatPanel.PanelCells;
            if (panelCells.Count != 3)
                throw new NesuException("PreCheck: Panel Cell is not 3. It is " + panelCells.Count);

            Management.AddNewElement();

            beatPanel = editor.PanelHub.GetPanel(TLEPanelNames.BEAT);
            if (beatPanel == null)
                throw new NesuException("Panel is null");
            panelCells = beatPanel.PanelCells;
            if (panelCells.Count != 4)
                throw new NesuException("Panel Cell is not 4");
        }

        [Fact]
        public void MoveElement()
        {
            var beatPanel = editor.PanelHub.GetPanel(TLEPanelNames.BEAT);
            var panelCell = beatPanel.PanelCells.FirstOrDefault();
            Management.ParentTLE.PanelHub.SelectPanelCell(panelCell);
            Management.ParentTLE.PanelHub.TimeIdentyficator.MaxWidth = 100;

            beatPanel = editor.PanelHub.GetPanel(TLEPanelNames.BEAT);
            if (beatPanel == null)
                throw new NesuException("PreCheck: Panel is null");

            Management.ParentTLE.SetTimeCursorToPrecentage(0.5f);

            Management.MoveElement();

            beatPanel = editor.PanelHub.GetPanel(TLEPanelNames.BEAT);
            if (beatPanel == null)
                throw new NesuException("Panel is null");
            panelCell = beatPanel.PanelCells.FirstOrDefault();
            if (panelCell.ReferenceElement.GetStartTime() != new TimeSpan(0, 0, 15))
                throw new Exception("times not match");
        }

        [Fact]
        public void UpdateElement()
        {
            Management.UpdateElement(new System.Collections.Specialized.NameValueCollection());
        }

        [Fact]
        public void RemoveElement()
        {
            var animationRoutePanel = editor.PanelHub.GetPanel(TLEPanelNames.BEAT);
            if (animationRoutePanel == null)
                throw new NesuException("PreCheck: Panel is null");

            var panelCellCountBefore = animationRoutePanel.PanelCells.Count;
            var panelCell = animationRoutePanel.PanelCells.FirstOrDefault();
            Management.ParentTLE.PanelHub.SelectPanelCell(panelCell);
            Management.ParentTLE.PanelHub.TimeIdentyficator.MaxWidth = 100;

            Management.RemoveElement();

            animationRoutePanel = editor.PanelHub.GetPanel(TLEPanelNames.BEAT);
            var panelCellCountAfter = animationRoutePanel.PanelCells.Count;
            if (panelCellCountBefore == panelCellCountAfter)
                throw new Exception("Element was not removed");

            if (panelCellCountBefore != (panelCellCountAfter + 1))
                throw new Exception("2. Element was not removed");
        }
    }
}
