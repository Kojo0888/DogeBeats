using DogeBeats.EngineSections.Shared;
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
    public class TLEPanelTests
    {
        TLEPanel panel;

        public TLEPanelTests()
        {
            panel = new TLEPanel("Doesn't matter");
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
            var firstCell = panel.PanelCells.FirstOrDefault();
            var time = new TimeSpan(0, 0, 50);

            panel.MovePanelCellTime(firstCell, time);

            if (firstCell.ReferenceElement.GetStartTime() != time)
                throw new NesuException("Start time does not match");
        }

        [Fact]
        public void MovePanelCellTime_GraphicName()
        {
            string name = "123dasd";
            var firstCell = panel.PanelCells.FirstOrDefault();
            var time = new TimeSpan(0, 0, 50);
            firstCell.GraphicName = name;

            panel.MovePanelCellTime(name, time);

            if (firstCell.ReferenceElement.GetStartTime() != time)
                throw new NesuException("Start time does not match");
        }

        [Fact]
        public void RemovePanelCell()
        {
            var firstCell = panel.PanelCells.FirstOrDefault();
            var time = new TimeSpan(0, 0, 50);

            panel.RemovePanelCell(firstCell);

            if (panel.PanelCells.Contains(firstCell))
                throw new NesuException("Cell still exists in panel");
        }

        [Fact]
        public void RemovePanelCell_GraphicName()
        {
            string name = "123dasd";
            var firstCell = panel.PanelCells.FirstOrDefault();
            firstCell.GraphicName = name;
            var time = new TimeSpan(0, 0, 50);

            var result = panel.RemovePanelCell(name);

            if (panel.PanelCells.Contains(firstCell))
                throw new NesuException("Cell still exists in panel");
            if (result != 1)
                throw new NesuException("Result does not qual 1");
        }

        [Fact]
        public void CalculateMaxElementsAtColumn()
        {
            panel.InitializeStackedElements();
            var maxElementsInOneColumn = panel.CalculateMaxElementsAtColumn();
            if (maxElementsInOneColumn != 3)
                throw new NesuException("max Element at column is not 3... why?....");
        }

        [Fact]
        public void CalculatePlacementForPanelCell()
        {
            panel.InitializeStackedElements();
            var firstCell = panel.PanelCells.FirstOrDefault();
            panel.Placement.Width = 100;
            var placement = panel.CalculatePlacementForPanelCell(firstCell);
            if (placement == null)
                throw new NesuException("Placement is null");
            if (placement.Y != 0)
                throw new NesuException("Y");
            if (placement.X != (100.0f / 30) * 12)
                throw new NesuException("X");
            if (placement.Width != 100.0f / 30)
                throw new NesuException("Width");
            if (placement.Height != 0)
                throw new NesuException("Height");
            if (placement.Rotation != 0)
                throw new NesuException("Rotation");

        }

        [Fact]
        public void CalculatePlacementForPanelCell_FirstElement()
        {
            panel.PanelCells = new List<TLEPanelCell>() { new TLEPanelCell() { ReferenceElement = new AnimationSingleElement() { Route = new AnimationRoute() { Frames = new List<AnimationRouteFrame>() { new AnimationRouteFrame() { FrameTime = new TimeSpan(0, 0, 0) } } } } } };

            panel.InitializeStackedElements();

            var firstCell = panel.PanelCells.FirstOrDefault();
            panel.Placement.Width = 100;
            panel.Placement.Height = 100;
            var placement = panel.CalculatePlacementForPanelCell(firstCell);
            if (placement == null)
                throw new NesuException("Placement is null");
            if (placement.Y != 100)
                throw new NesuException("Y");
            if (placement.X != (100.0f / 30) * 0)
                throw new NesuException("X");
            if (placement.Width != 100.0f / 30)
                throw new NesuException("Width");
            if (placement.Height != 100)//will be wrong
                throw new NesuException("Height");
            if (placement.Rotation != 0)
                throw new NesuException("Rotation");

        }

        [Fact]
        public void CalculatePlacementForPanelCell_TwoFirstElements()
        {
            panel.PanelCells = new List<TLEPanelCell>();
            panel.PanelCells.Add(new TLEPanelCell() { ReferenceElement = new AnimationSingleElement() { Route = new AnimationRoute() { Frames = new List<AnimationRouteFrame>() { new AnimationRouteFrame() { FrameTime = new TimeSpan(0, 0, 0) } } } } } );
            panel.PanelCells.Add(new TLEPanelCell() { ReferenceElement = new AnimationSingleElement() { Route = new AnimationRoute() { Frames = new List<AnimationRouteFrame>() { new AnimationRouteFrame() { FrameTime = new TimeSpan(0, 0, 0, 0 ,1) } } } } });

            panel.InitializeStackedElements();

            var firstCell = panel.PanelCells.FirstOrDefault();
            panel.Placement.Width = 100;
            panel.Placement.Height = 100;
            var placement = panel.CalculatePlacementForPanelCell(firstCell);
            if (placement == null)
                throw new NesuException("Placement is null");
            if (placement.Y != 100)
                throw new NesuException("Y");
            if (placement.X != (100.0f / 30) * 0)
                throw new NesuException("X");
            if (placement.Width != 100.0f / 30)
                throw new NesuException("Width");
            if (placement.Height != 50)//will be wrong
                throw new NesuException("Height");
            if (placement.Rotation != 0)
                throw new NesuException("Rotation");

            firstCell = panel.PanelCells.LastOrDefault();
            panel.Placement.Width = 100;
            panel.Placement.Height = 100;
            placement = panel.CalculatePlacementForPanelCell(firstCell);
            if (placement == null)
                throw new NesuException("Placement is null");
            if (placement.Y != 50)
                throw new NesuException("Y");
            if (placement.X != (100.0f / 30) * 0)
                throw new NesuException("X");
            if (placement.Width != 100.0f / 30)
                throw new NesuException("Width");
            if (placement.Height != 50)
                throw new NesuException("Height");
            if (placement.Rotation != 0)
                throw new NesuException("Rotation");

        }

        [Fact]
        public void GetStackedElementsForTimeSpan()
        {
            panel.InitializeStackedElements();
            var result = panel.GetStackedElementsForTimeSpan(new TimeSpan(0,0,12));
            if (result.Value.Count != 3)
                throw new NesuException("Count is " + result.Value.Count);
        }

        [Fact]
        public void GetCellElementBasedOnGraphicName()
        {
            panel.PanelCells = new List<TLEPanelCell>();
            var firstCell = new AnimationSingleElement() { Route = new AnimationRoute() { Frames = new List<AnimationRouteFrame>() { new AnimationRouteFrame() { FrameTime = new TimeSpan(0, 0, 0) } } } };
            var secondCell = new AnimationSingleElement() { Route = new AnimationRoute() { Frames = new List<AnimationRouteFrame>() { new AnimationRouteFrame() { FrameTime = new TimeSpan(0, 0, 0) } } } };
            panel.PanelCells.Add(new TLEPanelCell() { ReferenceElement = firstCell, GraphicName = "Test1" });
            panel.PanelCells.Add(new TLEPanelCell() { ReferenceElement = secondCell, GraphicName = "Test2" });

            var receivedFirstCell = panel.GetCellElementBasedOnGraphicName("Test1");
            if (receivedFirstCell != firstCell)
                throw new NesuException("First cell does not match");
            var receivedSecondCell = panel.GetCellElementBasedOnGraphicName("Test2");
            if (receivedSecondCell != secondCell)
                throw new NesuException("Second cell does not match");
        }

        [Fact]
        public void GetCell()
        {
            var refElem = new AnimationSingleElement() { Route = new AnimationRoute() { Frames = new List<AnimationRouteFrame>() { new AnimationRouteFrame() { FrameTime = new TimeSpan(0, 0, 0) } } } };
            var cell = new TLEPanelCell() { ReferenceElement = refElem };
            panel.PanelCells = new List<TLEPanelCell>() { cell };
            var receivedCell = panel.GetCell(refElem);
            if (receivedCell != cell)
                throw new NesuException("Cells differs");
        }

        [Fact]
        public void GetCell_GraphicName()
        {
            string name = "TEST123321";
            var refElem = new AnimationSingleElement() { Route = new AnimationRoute() { Frames = new List<AnimationRouteFrame>() { new AnimationRouteFrame() { FrameTime = new TimeSpan(0, 0, 0) } } } };
            var cell = new TLEPanelCell() { ReferenceElement = refElem, GraphicName = name };
            panel.PanelCells = new List<TLEPanelCell>() { cell };
            var receivedCell = panel.GetCell(name);
            if (receivedCell != cell)
                throw new NesuException("Cells differs");
        }

        [Fact]
        public void SelectPanelCell()
        {
            var firstCell = panel.PanelCells.FirstOrDefault();
            panel.SelectPanelCell(firstCell);
            if (panel.SelectedPanelCell != firstCell)
                throw new NesuException("Invalid Panel Cell selected");
        }

        [Fact]
        public void SelectPanelCell_GraphicName()
        {
            string name = "asda";
            var firstCell = panel.PanelCells.FirstOrDefault();
            firstCell.GraphicName = name;
            panel.SelectPanelCell(name);
            if (panel.SelectedPanelCell != firstCell)
                throw new NesuException("Invalid Panel Cell selected");
        }
    }
}
