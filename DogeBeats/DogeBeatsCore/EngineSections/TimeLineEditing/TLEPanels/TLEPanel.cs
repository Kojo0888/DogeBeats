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
        public List<TLEPanelCell> AllElements { get; set; } = new List<TLEPanelCell>();
        public List<TLEPanelCell> CurrentElements { get; set; } = new List<TLEPanelCell>();
        public Dictionary<TimeSpan, List<TLEPanelCell>> GrouppedElements { get; set; } = new Dictionary<TimeSpan, List<TLEPanelCell>>();

        public float Height { get; set; }
        public float OffsetHeight { get; set; }
        public float Width { get; set; }
        public string PanelName { get; set; }

        public List<TLEPanelCell> PanelCells { get; set; }

        public bool Selected { get; set; }

        public void InitialineElements(List<TLEPanelCell> elements)
        {
            AllElements = elements;
        }

        public void IniitializeCurrentElements()
        {
            CurrentElements = AllElements.Where(w => w.AnimationElement.GetStartTime() >= StartTime && w.AnimationElement.GetStartTime() < EndTime).OrderBy(o => o.AnimationElement.GetStartTime()).ToList();
            InitializeGrouppedElements();
        }

        public void InitializeGrouppedElements()
        {
            GrouppedElements = new Dictionary<TimeSpan, List<TLEPanelCell>>();

            TimeSpan currentTimeSpan = new TimeSpan(StartTime.Ticks);
            currentTimeSpan = currentTimeSpan.Add(TimeCellWidth);

            List<TLEPanelCell> group = new List<TLEPanelCell>();

            for (int i = 0; i < CurrentElements.Count; i++)
            {
                if (CurrentElements[i].AnimationElement.GetStartTime() < currentTimeSpan)
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
                cell.AnimationElement.SetStartTime(destenationTime);
            }

            IniitializeCurrentElements();
        }

        public void RemovePanelCell(string graphicName)
        {
            var cells = PanelCells.Where(w => w.GraphicName == graphicName);
            foreach (var cell in cells)
            {
                AllElements.Remove(cell);
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
        }

        private Placement CalculatePlacementForPanelCell(TLEPanelCell element)
        {
            float cellWidth = 0;
            if (TimeCellWidth == null)
                cellWidth = CalculateDynamicCellWidth(element);
            else
            {
                var diffticks = EndTime.Ticks - StartTime.Ticks;
                var numberOfPossibleCellsOnWidth = (((float)diffticks) / TimeCellWidth.Ticks);
                cellWidth = Width / numberOfPossibleCellsOnWidth;
            }

            var timestampWithGroup = GetElementGroupForTimeSpan(element.AnimationElement.GetStartTime());
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

        private float CalculateDynamicCellWidth(TLEPanelCell element)
        {
            var frame = element.AnimationElement as AnimationRouteFrame;
            if(frame != null)
            {
                var diffTime = EndTime - StartTime;
                var cellWidth = ((float)frame.FrameTime.Ticks / diffTime.Ticks) * Width;
                return cellWidth;
            }

            throw new Exception("Nesu: Unable to calculate dynamic cell width");
        }

        public KeyValuePair<TimeSpan, List<TLEPanelCell>> GetElementGroupForTimeSpan(TimeSpan timespan)
        {
            var timespanKey = GrouppedElements.Where(w => w.Key < timespan).Max(m => m.Key);

            return new KeyValuePair<TimeSpan, List<TLEPanelCell>>(timespanKey, GrouppedElements[timespanKey]);
        }

        public ITLEPanelCellElement GetCellElementBasedOnGraphicName(string graphicName)
        {
            if (!string.IsNullOrEmpty(graphicName))
                return null;

            TLEPanelCell cell = PanelCells.Where(w => w.GraphicName == graphicName).FirstOrDefault();
            if (cell != null)
            {
                return cell.AnimationElement;
            }
            return null;
        }

        public TLEPanelCell GetCell(string graphicName)
        {
            if (!string.IsNullOrEmpty(graphicName))
                return null;

            return PanelCells.FirstOrDefault(f => f.GraphicName == graphicName);
        }

        public void SelectPanelCell(string elementName)
        {
            ClearCellSelection();

            TLEPanelCell cell = GetCell(elementName);
            cell.Selected = true;
        }

        private void ClearCellSelection()
        {
            PanelCells.ForEach(fe => fe.Selected = false);
        }
    }
}
