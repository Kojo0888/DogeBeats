using DogeBeats.EngineSections.AnimationObjects;
using DogeBeats.EngineSections.Resources;
using DogeBeats.EngineSections.Shared;
using DogeBeats.EngineSections.TimeLineEditing.TLEPanels;
using DogeBeats.Modules;
using DogeBeats.Modules.TimeLines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;
using Xunit;

namespace DogeBeatsTests.EngineSections.TimeLineEditing.TLEPanels
{
    public class TLEPanelHubTests
    {
        TLEPanelHub PanelHub;

        public TLEPanelHubTests()
        {
            PanelHub = new TLEPanelHub(TimeLineEditor.DEFAULT_PANEL_START_TIME, TimeLineEditor.DEFAULT_PANEL_WIDTH_TIME, EnvironmentVariables.MainWindowWidth);
        }


        [Fact]
        public void GetPanel()
        {
            PanelHub.InitializeDefaultPanels(new Testowy.Model.TimeLine());
            var panel = PanelHub.GetPanel(TLEPanelNames.BEAT);
            if (panel == null)
                throw new NesuException("panel is null");
            if (panel.PanelName != TLEPanelNames.BEAT)
                throw new NesuException("Panel name differs");
        }

        [Fact]
        public void InitializeGraphicIdentyficator()
        {
            PanelHub.InitializeGraphicIdentyficator();
            if (PanelHub.TimeIdentyficator == null)
                throw new NesuException("TimeIdentificator is null");
        }

        [Fact]
        public void InitializeDefaultPanels()
        {
            PanelHub.InitializeDefaultPanels(new Testowy.Model.TimeLine());

            if (!PanelHub.Panels.ContainsKey(TLEPanelNames.BEAT))
                throw new NesuException("Beat panel is null");
            if (!PanelHub.Panels.ContainsKey(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0"))
                throw new NesuException("Beat panel is null");

            if (PanelHub.TimeIdentyficator == null)
                throw new NesuException("TimeIdentificator is null");
        }

        [Fact]
        public void InitializeNewAnimationPanel()
        {
            PanelHub.InitializeNewAnimationPanel(new List<IAnimationElement>());

            if (!PanelHub.Panels.ContainsKey(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0"))
                throw new NesuException("Panel animationElemnt with index 0 does not exist");

            PanelHub.InitializeNewAnimationPanel(new List<IAnimationElement>());

            if (!PanelHub.Panels.ContainsKey(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "1"))
                throw new NesuException("Panel animationElemnt with index 1 does not exist");

        }

        [Fact]
        public void MoveTimeForPanelsElement()
        {
            string graphicElement = "asddsa123";

            TimeLine tl = MockObjects.GetTimeLine2();
            var element = tl.AnimationElements.FirstOrDefault();
            element.GraphicName = graphicElement;
            //tl.AnimationElements
            PanelHub.InitializeDefaultPanels(tl);

            var cellBefore = PanelHub.GetPanelCellBasedOnReferenceElement(element);
            var panelCellGraphicName = cellBefore.GraphicName;

            PanelHub.TimeIdentyficator.MovePrecentage(.8f);
            var time = PanelHub.TimeIdentyficator.GetTime();

            PanelHub.MoveTimeForPanelsElement(panelCellGraphicName, .8f);

            var cellAfter = PanelHub.GetPanelCellBasedOnReferenceElement(element);
            var elem = cellAfter.ReferenceElement as AnimationSingleElement;
            if (elem.Route.AnimationStartTime != time)
                throw new NesuException("AnimationTime does not equal to time");
        }

        [Fact]
        public void MoveTimeForPanelsElement2()
        {
            string graphicElement = "asddsa123";

            TimeLine tl = MockObjects.GetTimeLine2();
            var element = tl.AnimationElements.FirstOrDefault();
            element.GraphicName = graphicElement;
            //tl.AnimationElements
            PanelHub.InitializeDefaultPanels(tl);

            var cellBefore = PanelHub.GetPanelCellBasedOnReferenceElement(element);
            var panelCellGraphicName = cellBefore.GraphicName;

            PanelHub.TimeIdentyficator.MovePrecentage(.8f);
            var time = PanelHub.TimeIdentyficator.GetTime();

            PanelHub.MoveTimeForPanelsElement(element, .8f);

            var cellAfter = PanelHub.GetPanelCellBasedOnReferenceElement(element);
            var elem = cellAfter.ReferenceElement as AnimationSingleElement;
            if (elem.Route.AnimationStartTime != time)
                throw new NesuException("AnimationTime does not equal to time");
        }

        [Fact]
        public void SelectPanelCell()
        {
            TimeLine tl = MockObjects.GetTimeLine2();
            //var element = tl.AnimationElements.FirstOrDefault();
            PanelHub.InitializeDefaultPanels(tl);
            var panel = PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            var cell = panel.PanelCells.FirstOrDefault();

            if (PanelHub.Panels.Count != 2)
                throw new NesuException("1. There supposed to be only 2 panels. There is " + PanelHub.Panels.Count);

            PanelHub.SelectPanelCell(cell);

            if (PanelHub.Panels.Count != 3)
                throw new NesuException("2. There supposed to be only 3 panels. There is " + PanelHub.Panels.Count);
        }

        [Fact]
        public void GetPanelForPanelCell_GraphicalName()
        {
            string name = "ttrt";

            TimeLine tl = MockObjects.GetTimeLine2();
            //var element = tl.AnimationElements.FirstOrDefault();
            PanelHub.InitializeDefaultPanels(tl);
            var panel = PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            var cell = panel.PanelCells.FirstOrDefault();
            cell.GraphicName = name;

            if (PanelHub.Panels.Count != 2)
                throw new NesuException("1. There supposed to be only 2 panels. There is " + PanelHub.Panels.Count);

            PanelHub.SelectPanelCell(name);

            if (PanelHub.Panels.Count != 3)
                throw new NesuException("2. There supposed to be only 3 panels. There is " + PanelHub.Panels.Count);
        }

        [Fact]
        public void GetPanelForPanelCell()
        {
            TimeLine tl = MockObjects.GetTimeLine2();
            PanelHub.InitializeDefaultPanels(tl);
            var panel = PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            var cell = panel.PanelCells.FirstOrDefault();

            var receivedPanel = PanelHub.GetPanelForPanelCell(cell);
            if (receivedPanel != panel)
                throw new NesuException("Panels does not match");
        }

        [Fact]
        public void CreatePanels()
        {
            TimeLine tl = MockObjects.GetTimeLine2();
            PanelHub.InitializeDefaultPanels(tl);
            var panel = PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            var cell = panel.PanelCells.FirstOrDefault();

            PanelHub.CreatePanels(panel.PanelName, cell);

            if (PanelHub.Panels.Count != 3)
                throw new NesuException("2. There supposed to be only 3 panels. There is " + PanelHub.Panels.Count);

            if (PanelHub.Panels.LastOrDefault().Key != TLEPanelNames.ANIMATION_ROUTE)
                throw new NesuException("New Panel is not an AniamtionElement panel. It is: " + PanelHub.Panels.LastOrDefault().Key);
        }

        [Fact]
        public void CreatePanels2()
        {
            TimeLine tl = MockObjects.GetTimeLine2();
            PanelHub.InitializeDefaultPanels(tl);
            var panel = PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            var cell = panel.PanelCells.LastOrDefault();

            PanelHub.CreatePanels(panel.PanelName, cell);

            if (PanelHub.Panels.Count != 3)
                throw new NesuException("2. There supposed to be only 3 panels. There is " + PanelHub.Panels.Count);

            if (PanelHub.Panels.LastOrDefault().Key != TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "1")
                throw new NesuException("New Panel is not an AniamtionElement panel. It is: " + PanelHub.Panels.LastOrDefault().Key);
        }

        [Fact]
        public void RemovePanels()
        {
            TimeLine tl = MockObjects.GetTimeLine2();
            PanelHub.InitializeDefaultPanels(tl);
            var panel = PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            var cell = panel.PanelCells.LastOrDefault();

            PanelHub.CreatePanels(panel.PanelName, cell);

            if (PanelHub.Panels.Count != 3)
                throw new NesuException("1. There supposed to be only 3 panels. There is " + PanelHub.Panels.Count);

            PanelHub.RemovePanels(panel.PanelName);

            if (PanelHub.Panels.Count != 2)
                throw new NesuException("2. There supposed to be only 2 panels. There is " + PanelHub.Panels.Count);
        }

        [Fact]
        public void SetTimeCursorToPrecentage()
        {
            var time = PanelHub.SetTimeCursorToPrecentage(.5f);
            if (time != new TimeSpan(0, 0, 15))
                throw new NesuException("time is " + time.ToString());
        }

        [Fact]
        public void GetLastGroupPanel_Single()
        {
            TimeLine tl = MockObjects.GetTimeLine2();
            PanelHub.InitializeDefaultPanels(tl);
            var panel = PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            var cell = panel.PanelCells.FirstOrDefault();

            PanelHub.CreatePanels(panel.PanelName, cell);

            var lastGroupPanel = PanelHub.GetLastAnimationElementPanel();
            if (lastGroupPanel.PanelName != TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0")
                throw new NesuException("LAst panel is not new animation element panel. It is: " + lastGroupPanel.PanelName);
        }

        [Fact]
        public void GetLastGroupPanel_Group()
        {
            TimeLine tl = MockObjects.GetTimeLine2();
            PanelHub.InitializeDefaultPanels(tl);
            var panel = PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            var cell = panel.PanelCells.LastOrDefault();

            PanelHub.CreatePanels(panel.PanelName, cell);

            var lastGroupPanel = PanelHub.GetLastAnimationElementPanel();
            if (lastGroupPanel.PanelName != TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "1")
                throw new NesuException("LAst panel is not new animation element panel with 0 index. It is: " + lastGroupPanel.PanelName);
        }

        [Fact]
        public void MoveForwardTimeScope()
        {
            var timeLine = MockObjects.GetTimeLine2();
            PanelHub.InitializeDefaultPanels(timeLine);

            PanelHub.MoveForwardTimeScope();

            var time = PanelHub.TimeIdentyficator.StartTime;
            if (time != TimeLineEditor.DEFAULT_PANEL_START_TIME + TimeLineEditor.DEFAULT_PANEL_WIDTH_TIME)
                throw new NesuException("Start time does not match. It Is " + time.ToString());

            var panel = PanelHub.Panels.Values.FirstOrDefault();
            if (panel.StartTime != TimeLineEditor.DEFAULT_PANEL_START_TIME + TimeLineEditor.DEFAULT_PANEL_WIDTH_TIME)
                throw new NesuException("Panel start time does not match. It Is " + panel.StartTime.ToString());
        }

        [Fact]
        public void MoveBackwardTimeScope_Zero()
        {
            var timeLine = MockObjects.GetTimeLine2();
            PanelHub.InitializeDefaultPanels(timeLine);

            PanelHub.MoveBackwardTimeScope();

            var time = PanelHub.TimeIdentyficator.StartTime;
            if (time != new TimeSpan(0, 0, 0))
                throw new NesuException("Start time does not match. It Is " + time.ToString());

            var panel = PanelHub.Panels.Values.FirstOrDefault();
            if (panel.StartTime != new TimeSpan(0, 0, 0))
                throw new NesuException("Panel start time does not match. It Is " + panel.StartTime.ToString());
        }

        [Fact]
        public void MoveBackwardTimeScope_Bacward()
        {
            var timeLine = MockObjects.GetTimeLine2();
            PanelHub.InitializeDefaultPanels(timeLine);

            PanelHub.MoveForwardTimeScope();
            PanelHub.MoveForwardTimeScope();
            PanelHub.MoveBackwardTimeScope();

            var time = PanelHub.TimeIdentyficator.StartTime;
            if (time != new TimeSpan(0, 0, 30))
                throw new NesuException("Start time does not match. It Is " + time.ToString());

            var panel = PanelHub.Panels.Values.FirstOrDefault();
            if (panel.StartTime != new TimeSpan(0, 0, 30))
                throw new NesuException("Panel start time does not match. It Is " + panel.StartTime.ToString());
        }

        [Fact]
        public void GetLastPanelAnimationElementIndex_Group()
        {
            TimeLine tl = MockObjects.GetTimeLine2();
            PanelHub.InitializeDefaultPanels(tl);
            var panel = PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            var cell = panel.PanelCells.LastOrDefault();

            PanelHub.CreatePanels(panel.PanelName, cell);

            var lastGroupPanelIndex = PanelHub.GetLastPanelAnimationElementIndex();
            if (lastGroupPanelIndex != 1)
                throw new NesuException("LAst panel index is not 1 index. It is: " + lastGroupPanelIndex);
        }

        [Fact]
        public void GetLastPanelAnimationElementIndex_Single()
        {
            TimeLine tl = MockObjects.GetTimeLine2();
            PanelHub.InitializeDefaultPanels(tl);
            var panel = PanelHub.GetPanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");
            var cell = panel.PanelCells.FirstOrDefault();

            PanelHub.CreatePanels(panel.PanelName, cell);

            var lastGroupPanelIndex = PanelHub.GetLastPanelAnimationElementIndex();
            if (lastGroupPanelIndex != 0)
                throw new NesuException("LAst panel index is not 0 index. It is: " + lastGroupPanelIndex);
        }

        [Fact]
        public void UpdateAllPanelTimeScope_1()
        {
            var timeLine = MockObjects.GetTimeLine2();
            PanelHub.InitializeDefaultPanels(timeLine);
            var starttime = new TimeSpan(0, 2, 0);
            PanelHub.PanelOffsetTime = starttime;
            PanelHub.PanelWidthTime = new TimeSpan(0, 0, 15);

            PanelHub.UpdateTimeScope();

            var time = PanelHub.TimeIdentyficator.StartTime;
            if (time != starttime)
                throw new NesuException("Start time does not match. It Is " + time.ToString());

            var panel = PanelHub.Panels.Values.FirstOrDefault();
            if (panel.StartTime != starttime)
                throw new NesuException("Panel start time does not match. It Is " + panel.StartTime.ToString());

        }

        [Fact]
        public void UpdateAllPanelTimeScope_2()
        {
            var timeLine = MockObjects.GetTimeLine2();
            PanelHub.InitializeDefaultPanels(timeLine);
            var starttime = new TimeSpan(0, 2, 0);

            PanelHub.UpdateTimeScope(starttime, new TimeSpan(0, 0, 15));

            var time = PanelHub.TimeIdentyficator.StartTime;
            if (time != starttime)
                throw new NesuException("Start time does not match. It Is " + time.ToString());

            var panel = PanelHub.Panels.Values.FirstOrDefault();
            if (panel.StartTime != starttime)
                throw new NesuException("Panel start time does not match. It Is " + panel.StartTime.ToString());
        }

        [Fact]
        public void SelectPanelAndPanelElement_AnimaitonElemnt0()
        {
            var timeLine = MockObjects.GetTimeLine2();
            PanelHub.InitializeDefaultPanels(timeLine);

            var panelCountBefore = PanelHub.Panels.Values.Count();

            var panel = PanelHub.Panels[TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0"];
            var cell = panel.PanelCells.FirstOrDefault();
            PanelHub.SelectPanelCell(cell);

            var panelCountAfter = PanelHub.Panels.Values.Count();
            if (panelCountBefore == panelCountAfter)
                throw new NesuException("Panel count didn't change");
        }

        [Fact]
        public void SelectPanelAndPanelElement_Beat()
        {
            var timeLine = MockObjects.GetTimeLine2();
            PanelHub.InitializeDefaultPanels(timeLine);

            var panelCountBefore = PanelHub.Panels.Values.Count();

            var panel = PanelHub.Panels[TLEPanelNames.BEAT];
            panel.PanelCells.Add(new DogeBeats.Modules.TimeLines.TLEPanelCell() { ReferenceElement = new Beat() { Timestamp = new TimeSpan(0, 0, 20) } });
            var cell = panel.PanelCells.FirstOrDefault();
            PanelHub.SelectPanelCell(cell);

            var panelCountAfter = PanelHub.Panels.Values.Count();
            if (panelCountBefore != panelCountAfter)
                throw new NesuException("Panel count did change");
        }

        [Fact]
        public void SelectPanel_RemovePanels()
        {
            var timeLine = MockObjects.GetTimeLine2();
            PanelHub.InitializeDefaultPanels(timeLine);

            var panelCountBefore = PanelHub.Panels.Values.Count();

            var panel = PanelHub.Panels[TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0"];
            var cell = panel.PanelCells.FirstOrDefault();
            PanelHub.SelectPanelCell(cell);

            var panelCountAfter = PanelHub.Panels.Values.Count();
            if (panelCountBefore == panelCountAfter)
                throw new NesuException("Panel count didn't change");

            PanelHub.RemovePanels(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0");

            var panelCountAfter2 = PanelHub.Panels.Values.Count();
            if (panelCountBefore != panelCountAfter2)
                throw new NesuException("Removal unsuccessful");
        }

        [Fact]
        public void GetPanelGroupIndexes()
        {
            var timeLine = MockObjects.GetTimeLine2();
            PanelHub.InitializeDefaultPanels(timeLine);

            var panel = PanelHub.Panels[TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0"];
            var cell = panel.PanelCells.LastOrDefault();
            PanelHub.SelectPanelCell(cell);

            var listOfIndexes = PanelHub.GetPanelGroupIndexes();
            if (listOfIndexes == null || !listOfIndexes.Contains(0) || !listOfIndexes.Contains(1))
                throw new NesuException("0 or 1 are not on the list");
        }
    }
}
