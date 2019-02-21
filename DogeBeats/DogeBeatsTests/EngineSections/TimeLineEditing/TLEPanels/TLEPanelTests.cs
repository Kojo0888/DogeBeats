using DogeBeats.EngineSections.Shared;
using DogeBeats.Modules.TimeLines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DogeBeatsTests.EngineSections.TimeLineEditing.TLEPanels
{
    public class TLEPanelTests
    {
        TLEPanel panel = new TLEPanel();

        public TLEPanelTests()
        {
            panel = new TLEPanel();
            //panel.TimeCellWidth

            List<ITLEPanelCellElement> elements = new List<ITLEPanelCellElement>();
            elements.Add(new Beat() { Timestamp = new TimeSpan(0, 0, 12) });
            elements.Add(new Beat() { Timestamp = new TimeSpan(0, 0, 0, 12, 1) });
            elements.Add(new Beat() { Timestamp = new TimeSpan(0, 0, 0, 12, 2) });
            elements.Add(new Beat() { Timestamp = new TimeSpan(0, 0, 14) });
            elements.Add(new Beat() { Timestamp = new TimeSpan(0, 0, 15) });

            var parsedCells = TLEPanelCell.Parse(elements);
            panel.PanelCells = new List<TLEPanelCell>();
            panel.PanelCells.AddRange(parsedCells);

            panel.StartTime = new TimeSpan();
            panel.EndTime = new TimeSpan(0, 0, 0, 30, 0);
        }


        [Fact]
        public void InitializeGrouppedElements()
        {
            panel.InitializeStackedElements();

            if (!panel.StackedElements.ContainsKey(new TimeSpan(0, 0, 12)))
                throw new NesuException("Does not contain the key");
            if (panel.StackedElements[new TimeSpan(0, 0, 12)].Count != 3)
                throw new NesuException("Element count is " + panel.StackedElements[new TimeSpan(0, 0, 12)].Count);
        }

        [Fact]
        public void MovePanelCellTime()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void RemovePanelCell()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CalculateMaxElementsAtColumn()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CalculatePlacementForPanelCell()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void GetStackedElementsForTimeSpan()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void GetCellElementBasedOnGraphicName()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void GetCell()
        {
            throw new NotImplementedException();
        }


        [Fact]
        public void SelectPanelCell()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void ClearCellSelection()
        {
            throw new NotImplementedException();
        }
    }
}
