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
    public class TLEPCEManagementAnimationElementTests
    {
        TimeLineEditor editor;
        TLEPCEManagementAnimationElement Management;

        public TLEPCEManagementAnimationElementTests()
        {
            editor = new TimeLineEditor();
            var timeLine = MockObjects.GetTimeLine2();
            editor.AttachTimeLineToEditor(timeLine);
            //editor.PanelHub.InitializeDefaultPanels(timeLine);
            Management = new TLEPCEManagementAnimationElement(editor);
        }

        //TODO
        //leftout: Why this fails. There is no Animation ROute Panel. Because Selected Cell is in TLEPanel scope, Instead of PanelHub scope
        [Fact]
        public void AddNewElement()
        {
            var animationElementPanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            var panelCell = animationElementPanel.PanelCells.FirstOrDefault();
            Management.ParentTLE.PanelHub.SelectPanelCell(panelCell);

            //editor.PanelHub.SelectPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");

            var animationRoutePanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            if (animationRoutePanel == null)
                throw new NesuException("PreCheck: Panel is null");
            var panelCells = animationRoutePanel.PanelCells;
            if (panelCells.Count != 3)
                throw new NesuException("PreCheck: Panel Cell is not 3. It is " + panelCells.Count);

            Management.AddNewElement();

            animationRoutePanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            if (animationRoutePanel == null)
                throw new NesuException("Panel is null");
            panelCells = animationRoutePanel.PanelCells;
            if (panelCells.Count != 4)
                throw new NesuException("Panel Cell is not 4");
        }

        [Fact]
        public void MoveElement()
        {
            var animationElementPanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            var panelCell = animationElementPanel.PanelCells.FirstOrDefault();
            Management.ParentTLE.PanelHub.SelectPanelCell(panelCell);

            //Management.ParentTLE.PanelHub.Time

            Management.ParentTLE.PanelHub.TimeIdentyficator.MaxWidth = 100;
            //editor.PanelHub.SelectPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");

            var animationRoutePanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            if (animationRoutePanel == null)
                throw new NesuException("PreCheck: Panel is null");

            Management.ParentTLE.SetTimeCursorToPrecentage(0.5f);

            Management.MoveElement();

            animationRoutePanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            if (animationRoutePanel == null)
                throw new NesuException("Panel is null");
            panelCell = animationElementPanel.PanelCells.FirstOrDefault();
            if (panelCell.ReferenceElement.GetStartTime() != new TimeSpan(0, 0, 15))
                throw new Exception("times not match");
        }

        [Fact]
        public void UpdateElement()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void RemoveElement()
        {
            throw new NotImplementedException();
        }
    }
}
