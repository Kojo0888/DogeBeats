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

        [Fact]
        public void AddNewElement()
        {
            var animationElementPanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            var panelCell = animationElementPanel.PanelCells.FirstOrDefault();
            Management.ParentTLE.PanelHub.SelectPanelCell(panelCell);

            //editor.PanelHub.SelectPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");

            animationElementPanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            if (animationElementPanel == null)
                throw new NesuException("PreCheck: Panel is null");
            var panelCells = animationElementPanel.PanelCells;
            if (panelCells.Count != 3)
                throw new NesuException("PreCheck: Panel Cell is not 3. It is " + panelCells.Count);


            Management.AddNewElement();

            animationElementPanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            if (animationElementPanel == null)
                throw new NesuException("Panel is null");
            panelCells = animationElementPanel.PanelCells;
            if (panelCells.Count != 4)
                throw new NesuException("Panel Cell is not 4");
        }

        [Fact]
        public void MoveElement()
        {
            var animationElementPanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            var panelCell = animationElementPanel.PanelCells.FirstOrDefault();
            Management.ParentTLE.PanelHub.SelectPanelCell(panelCell);
            Management.ParentTLE.PanelHub.TimeIdentyficator.MaxWidth = 100;

            animationElementPanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            if (animationElementPanel == null)
                throw new NesuException("PreCheck: Panel is null");

            Management.ParentTLE.SetTimeCursorToPrecentage(0.5f);

            Management.MoveElement();

            animationElementPanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            if (animationElementPanel == null)
                throw new NesuException("Panel is null");
            panelCell = animationElementPanel.PanelCells.FirstOrDefault();
            if (panelCell.ReferenceElement.GetStartTime() != new TimeSpan(0, 0, 15))
                throw new Exception("times not match");
        }

        [Fact]
        public void UpdateElement()
        {
            var animationElementPanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            if (animationElementPanel == null)
                throw new NesuException("PreCheck: Panel is null");

            var panelCell = animationElementPanel.PanelCells.FirstOrDefault();
            Management.ParentTLE.PanelHub.SelectPanelCell(panelCell);
            Management.ParentTLE.PanelHub.TimeIdentyficator.MaxWidth = 100;

            NameValueCollection values = new NameValueCollection();
            values.Add("X", "100");
            values.Add("Y", "100");
            values.Add("Width", "100");
            values.Add("Height", "100");
            values.Add("Rotation", "100");
            values.Add("Prediction", "false");
            values.Add("ShapeTypeName", "Square");
            values.Add("Name", "testName");

            Management.UpdateElement(values);
            var anim = panelCell.ReferenceElement as AnimationSingleElement;
            if (anim == null || anim.Placement.X != 100 || anim.Placement.Y != 100 || anim.Placement.Width != 100 || anim.Placement.Height != 100 || anim.Placement.Rotation != 100)
                throw new Exception("Placement data does not match");

            if (anim == null || anim.Name != "testName" || anim.Shape.TypeName != "Square" || anim.Prediction)
                throw new Exception("AnimationElement data differs");

        }

        [Fact]
        public void RemoveElement()
        {
            var animationElementPanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            if (animationElementPanel == null)
                throw new NesuException("PreCheck: Panel is null");

            var panelCellCountBefore = animationElementPanel.PanelCells.Count;
            var panelCell = animationElementPanel.PanelCells.FirstOrDefault();
            Management.ParentTLE.PanelHub.SelectPanelCell(panelCell);
            Management.ParentTLE.PanelHub.TimeIdentyficator.MaxWidth = 100;

            Management.RemoveElement();

            animationElementPanel = editor.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            var panelCellCountAfter = animationElementPanel.PanelCells.Count;
            if (panelCellCountBefore == panelCellCountAfter)
                throw new Exception("Element was not removed");

            if (panelCellCountBefore != (panelCellCountAfter + 1))
                throw new Exception("2. Element was not removed");
        }
    }
}
