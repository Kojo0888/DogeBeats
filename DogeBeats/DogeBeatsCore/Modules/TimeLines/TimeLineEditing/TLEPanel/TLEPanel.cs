using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.Modules.TimeLines
{
    public class TLEPanel
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan StopperTime { get; set; }
        public TimeSpan TimeCellWidth { get; set; } = new TimeSpan(0, 0, 0, 1, 0);
        public List<TimedTLEPanelElement> AllElements { get; set; } = new List<TimedTLEPanelElement>();
        public List<TimedTLEPanelElement> CurrentElements { get; set; } = new List<TimedTLEPanelElement>();
        public Dictionary<TimeSpan, List<TimedTLEPanelElement>> GrouppedElements { get; set; } = new Dictionary<TimeSpan, List<TimedTLEPanelElement>>();

        public float Height { get; set; }
        public float OffsetHeight { get; set; }
        public float Width { get; set; }
        public string PanelName { get; set; }

        public List<TLEPanelCell> PanelCells { get; set; }
        public static TLEPanelCell SelectedCell { get; set; }

        public void InitialineElements(List<TimedTLEPanelElement> elements)
        {
            AllElements = elements;
        }

        public void IniitializeCurrentElements()
        {
            CurrentElements = AllElements.Where(w => w.Timestamp >= StartTime && w.Timestamp < EndTime).OrderBy(o => o.Timestamp).ToList();
            InitializeGrouppedElements();
        }

        public void InitializeGrouppedElements()
        {
            GrouppedElements = new Dictionary<TimeSpan, List<TimedTLEPanelElement>>();

            TimeSpan currentTimeSpan = new TimeSpan(StartTime.Ticks);
            currentTimeSpan = currentTimeSpan.Add(TimeCellWidth);

            List<TimedTLEPanelElement> group = new List<TimedTLEPanelElement>();

            for (int i = 0; i < CurrentElements.Count; i++)
            {
                if (CurrentElements[i].Timestamp < currentTimeSpan)
                    group.Add(CurrentElements[i]);
                else
                {
                    if (currentTimeSpan > EndTime)
                        break;//fuse

                    currentTimeSpan = currentTimeSpan.Add(TimeCellWidth);
                    i--;
                    continue;
                }
            }
        }

        internal void MovePanelCellTime(string graphicName, TimeSpan destenationTime)
        {
            var cells = PanelCells.Where(w => w.GraphicName == graphicName);
            foreach (var cell in cells)
            {
                cell.ReferencingTimedElement.Timestamp = destenationTime;
                RefreshPanelCells(new List<TimedTLEPanelElement>() { cell.ReferencingTimedElement });
            }

            IniitializeCurrentElements();
        }

        public void RemovePanelCell(string graphicName)
        {
            var cells = PanelCells.Where(w => w.GraphicName == graphicName);
            foreach (var cell in cells)
            {
                AllElements.Remove(cell.ReferencingTimedElement);
                PanelCells.Remove(cell);
            }

            IniitializeCurrentElements();
            //InitializeGrouppedElements();
            //return cells.FirstOrDefault()?.ReferencingTimedElement.Object;
        }

        public int CalculateMaxElementsAtColumn()
        {
            return GrouppedElements.Max(w => w.Value.ToList().Count);
        }

        internal void UpdateSectionTime(TimeSpan startTime, TimeSpan endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
            IniitializeCurrentElements();
            RefreshPanelCells();
        }

        public void RefreshPanelCells(List<TimedTLEPanelElement> element = null)
        {
            if(CurrentElements != null)
            {
                PanelCells.RemoveAll(r => element.Contains(r.ReferencingTimedElement));
                PanelCells.AddRange(CreatePanelCells(element));
            }
            else
            {
                PanelCells = CreatePanelCells(CurrentElements);
            }
        }

        public List<TLEPanelCell> CreatePanelCells(List<TimedTLEPanelElement> elements)
        {
            List<TLEPanelCell> panelCells = new List<TLEPanelCell>();
            foreach (var element in elements)
            {
                TLEPanelCell cell = TLEPanelCell.Parse(element);//no Placement set
                cell.Placement = CalculatePlacementForPanelCell(element);
                panelCells.Add(cell);
            }
            return panelCells;
        }

        private Placement CalculatePlacementForPanelCell(TimedTLEPanelElement element)
        {
            float cellWidth = 0;
            if (TimeCellWidth == null)
            {
                //Todo automatic calculation of cell width
                cellWidth = 0;
            }
            else
            {
                var diffticks = EndTime.Ticks - StartTime.Ticks;
                var numberOfPossibleCellsOnWidth = (((float)diffticks) / TimeCellWidth.Ticks);
                cellWidth = Width / numberOfPossibleCellsOnWidth;
            }

            var timestampWithGroup = GetElementGroupForTimeSpan(element.Timestamp);
            var orderInderForGroupKey = GrouppedElements.Keys.ToList().IndexOf(timestampWithGroup.Key);
            var indexInGroup = timestampWithGroup.Value.IndexOf(element);
            if (indexInGroup == -1)
                throw new Exception("NEsu: -1 for " + nameof(indexInGroup));

            Placement placement = new Placement();
            placement.X = orderInderForGroupKey * cellWidth;
            placement.Height = Height / timestampWithGroup.Value.Count;
            placement.Y = OffsetHeight + Height - (Height * indexInGroup);
            placement.Width = cellWidth;
            placement.Rotation = 0;

            return placement;
        }

        public KeyValuePair<TimeSpan, List<TimedTLEPanelElement>> GetElementGroupForTimeSpan(TimeSpan timespan)
        {
            var timespanKey = GrouppedElements.Where(w => w.Key < timespan).Max(m => m.Key);

            return new KeyValuePair<TimeSpan, List<TimedTLEPanelElement>>(timespanKey, GrouppedElements[timespanKey]);
        }

        public ITLEPanelElement GetObjectFromCellElementName(string elementName)
        {
            TLEPanelCell cell = PanelCells.Where(w => w.GraphicName == elementName).FirstOrDefault();
            if (cell != null)
            {
                return cell.ReferencingTimedElement.Object;
            }
            return null;
        }
    }
}
