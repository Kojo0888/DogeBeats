using DogeBeats.EngineSections.Shared;
using DogeBeats.EngineSections.TimeLineEditing.TLEPanelCellElementManagement;
using DogeBeats.EngineSections.TimeLineEditing.TLEPanels;
using DogeBeats.Modules;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;
using Xunit;

namespace DogeBeatsTests.EngineSections.TimeLineEditing.TLEPanelCellElementManagement
{
    public class TLEPCEManagementAnimationRouteTests
    {
        TimeLineEditor editor;
        TLEPCEManagementAnimationRoute Management;

        public TLEPCEManagementAnimationRouteTests()
        {
            editor = new TimeLineEditor();
            var timeLine = MockObjects.GetTimeLine2();
            editor.AttachTimeLineToEditor(timeLine);
            Management = new TLEPCEManagementAnimationRoute(editor);

            var animationElementPanel = Management.ParentTLE.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            var animationElementCell = animationElementPanel.PanelCells.FirstOrDefault();
            Management.ParentTLE.PanelHub.SelectPanelCell(animationElementCell);
        }

        [Fact]
        public void AddNewElement()
        {
            var animationRoutePanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ROUTE);
            var panelCell = animationRoutePanel.PanelCells.FirstOrDefault();
            Management.ParentTLE.PanelHub.SelectPanelCell(panelCell);

            animationRoutePanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ROUTE);
            if (animationRoutePanel == null)
                throw new NesuException("PreCheck: Panel is null");
            var panelCells = animationRoutePanel.PanelCells;
            if (panelCells.Count != 3)
                throw new NesuException("PreCheck: Panel Cell is not 3. It is " + panelCells.Count);


            Management.AddNewElement();

            animationRoutePanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ROUTE);
            if (animationRoutePanel == null)
                throw new NesuException("Panel is null");
            panelCells = animationRoutePanel.PanelCells;
            if (panelCells.Count != 4)
                throw new NesuException("Panel Cell is not 4");
        }

        [Fact]
        public void MoveElement()
        {
            var animationRoutePanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ROUTE);
            var panelCell = animationRoutePanel.PanelCells.FirstOrDefault();
            Management.ParentTLE.PanelHub.SelectPanelCell(panelCell);
            Management.ParentTLE.PanelHub.TimeIdentyficator.MaxWidth = 100;

            animationRoutePanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ROUTE);
            if (animationRoutePanel == null)
                throw new NesuException("PreCheck: Panel is null");

            Management.ParentTLE.SetTimeCursorToPrecentage(0.5f);

            Management.MoveElement();

            animationRoutePanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ROUTE);
            if (animationRoutePanel == null)
                throw new NesuException("Panel is null");
            panelCell = animationRoutePanel.PanelCells.FirstOrDefault();
            if (panelCell.ReferenceElement.GetStartTime() != new TimeSpan(0, 0, 15))
                throw new Exception("times not match");
        }

        [Fact]
        public void UpdateElement()
        {
            var animationElementPanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ROUTE);
            if (animationElementPanel == null)
                throw new NesuException("PreCheck: Panel is null");

            var panelCell = animationElementPanel.PanelCells.FirstOrDefault();
            Management.ParentTLE.PanelHub.SelectPanelCell(panelCell);
            Management.ParentTLE.PanelHub.TimeIdentyficator.MaxWidth = 100;

            NameValueCollection values = new NameValueCollection();
            values.Add("Amplitude","1");
            values.Add("Cycles", "2");
            values.Add("SpeedAmplitude", "3");
            values.Add("SpeedPhase", "4");
            values.Add("SpeedCycles", "5");
            values.Add("FrameTime", "00:00:06");

            Management.UpdateElement(values);
            var anim = panelCell.ReferenceElement as AnimationRouteFrame;
            if (anim == null || anim.Amplitude != 1 || anim.Cycles != 2 || anim.SpeedAmplitude != 3 || anim.SpeedPhase != 4 || anim.SpeedCycles != 5 || anim.FrameTime != new TimeSpan(0,0,6))
                throw new Exception("Route frame data does not match");
        }

        [Fact]
        public void RemoveElement()
        {
            var animationRoutePanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ROUTE);
            if (animationRoutePanel == null)
                throw new NesuException("PreCheck: Panel is null");

            var panelCellCountBefore = animationRoutePanel.PanelCells.Count;
            var panelCell = animationRoutePanel.PanelCells.FirstOrDefault();
            Management.ParentTLE.PanelHub.SelectPanelCell(panelCell);
            Management.ParentTLE.PanelHub.TimeIdentyficator.MaxWidth = 100;

            Management.RemoveElement();

            animationRoutePanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ROUTE);
            var panelCellCountAfter = animationRoutePanel.PanelCells.Count;
            if (panelCellCountBefore == panelCellCountAfter)
                throw new Exception("Element was not removed");

            if (panelCellCountBefore != (panelCellCountAfter + 1))
                throw new Exception("2. Element was not removed");
        }
    }
}
