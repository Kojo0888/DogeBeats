using DogeBeats.EngineSections.Shared;
using DogeBeats.EngineSections.TimeLineEditing.TLEPanels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            PanelHub.InitializePanels(new Testowy.Model.TimeLine());
            var panel = PanelHub.GetPanel(TLEPanelNames.BEAT);
            if (panel == null)
                throw new NesuException("panel is null");
            if (panel.PanelName != TLEPanelNames.BEAT)
                throw new NesuException("Panel name differs");
        }

        [Fact]
        public void InitializeGraphicIdentyficator()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void InitializePanels()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void InitializeNewAnimationPanel()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CreatePanelWithDefaultSettings()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void MoveTimeForPanelsElement()
        {
            throw new NotImplementedException();
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
