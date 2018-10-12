using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.Modules.TimeLines
{
    public class TimeLineEditorPanel
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan StopperTime { get; set; }
        public TimeSpan TimeCellWidth { get; set; } = new TimeSpan(0, 0, 0, 1, 0);
        public List<TimedGraphicElement> AllElements { get; set; } = new List<TimedGraphicElement>();
        public List<TimedGraphicElement> CurrentElements { get; set; } = new List<TimedGraphicElement>();
        public Dictionary<TimeSpan, List<TimedGraphicElement>> GrouppedElements { get; set; } = new Dictionary<TimeSpan, List<TimedGraphicElement>>();

        public float Height { get; set; }
        public float OffsetHeight { get; set; }
        public float Width { get; set; }
        public string PanelName { get; set; }

        public List<TLEPanelCell> PanelCells { get; set; }
        public static TLEPanelCell SelectedCell { get; set; }

        public void InitialineElements(List<TimedGraphicElement> elements)
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
            GrouppedElements = new Dictionary<TimeSpan, List<TimedGraphicElement>>();

            TimeSpan currentTimeSpan = new TimeSpan(StartTime.Ticks);
            currentTimeSpan = currentTimeSpan.Add(TimeCellWidth);

            List<TimedGraphicElement> group = new List<TimedGraphicElement>();

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

        internal void MoveElementTime(string elementName, TimeSpan destenationTime)
        {
            var elements = AllElements.Where(w => w.Object.GraphicName == elementName).ToList();

            foreach (var element in elements)
            {
                element.Timestamp = destenationTime;
            }

            IniitializeCurrentElements();
        }

        public void RemoveElement(string graphicName)
        {
            AllElements.RemoveAll(r => r.Object.GraphicName == graphicName);
            IniitializeCurrentElements();
            //InitializeGrouppedElements();
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
        }

        public List<TLEPanelCell> CreatePanelCells(List<TimedGraphicElement> elements)
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

        private Placement CalculatePlacementForPanelCell(TimedGraphicElement element)
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

        public KeyValuePair<TimeSpan, List<TimedGraphicElement>> GetElementGroupForTimeSpan(TimeSpan timespan)
        {
            var timespanKey = GrouppedElements.Where(w => w.Key < timespan).Max(m => m.Key);

            return new KeyValuePair<TimeSpan, List<TimedGraphicElement>(timespanKey, GrouppedElements[timespanKey]);
        }
    }
}
