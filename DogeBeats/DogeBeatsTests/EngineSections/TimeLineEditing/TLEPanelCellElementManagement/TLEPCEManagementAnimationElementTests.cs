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
        TLEPCEManagementAnimationRoute Management;

        public TLEPCEManagementAnimationElementTests()
        {
            editor = new TimeLineEditor();
            var timeLine = MockObjects.GetTimeLine2();
            editor.AttachTimeLineToEditor(timeLine);
            //editor.PanelHub.InitializeDefaultPanels(timeLine);
            Management = new TLEPCEManagementAnimationRoute(editor);
        }

        //TODO
        //leftout: Why this fails. There is no Animation ROute Panel. Because Selected Cell is in TLEPanel scope, Instead of PanelHub scope
        [Fact]
        public void AddNewElement()
        {
            var animationElementPanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            var panelCell = animationElementPanel.PanelCells.FirstOrDefault();
            animationElementPanel.SelectPanelCell(panelCell);

            //editor.PanelHub.SelectPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");

            var animationRoutePanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ROUTE);
            if (animationRoutePanel == null)
                throw new NesuException("PreCheck: Panel is null");
            var panelCells = animationRoutePanel.PanelCells;
            if (panelCells.Count != 0)
                throw new NesuException("PreCheck: Panel Cell is not 0");

            Management.AddNewElement();

            animationRoutePanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ROUTE);
            if (animationRoutePanel == null)
                throw new NesuException("Panel is null");
            panelCells = animationRoutePanel.PanelCells;
            if (panelCells.Count != 1)
                throw new NesuException("Panel Cell is not 1");
        }

        [Fact]
        public void MoveElement()
        {
            throw new NotImplementedException();
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
