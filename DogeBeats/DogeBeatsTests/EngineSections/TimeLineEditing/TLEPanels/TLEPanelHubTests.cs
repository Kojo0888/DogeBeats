using DogeBeats.EngineSections.AnimationObjects;
using DogeBeats.EngineSections.Shared;
using DogeBeats.EngineSections.TimeLineEditing.TLEPanels;
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
            PanelHub = new TLEPanelHub();
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
            var elem  = cellAfter.ReferenceElement as AnimationSingleElement;
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
        public void SetTimeCursorToPrecentage()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void GetLastGroupPanel()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void MoveForwardTimeScope()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void MoveBackwardTimeScope()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void GetLastPanelAnimationElementIndex()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void UpdateAllPanelTimeScope()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void SelectPanel()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void SelectPanelAndPanelElement()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void SelectPanel_RemovePanels()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void GetPanelGroupIndexes()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void GetNewGroupPanelName()
        {
            throw new NotImplementedException();
        }
    }
}
