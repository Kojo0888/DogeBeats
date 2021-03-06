﻿using DogeBeats.EngineSections.Resources;
using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.Modules.TimeLines
{
    public class TLEPanel : IGraphicElement
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan StopperTime { get; set; }
        public TimeSpan TimeCellWidth { get; set; } = new TimeSpan(0, 0, 0, 1, 0);
        //public List<TLEPanelCell> Elements { get; set; } = new List<TLEPanelCell>();
        public Dictionary<TimeSpan, List<TLEPanelCell>> StackedElements { get; set; } = new Dictionary<TimeSpan, List<TLEPanelCell>>();

        public static int PANEL_CELL_WIDTH = 10;

        public string PanelName { get; set; }

        public List<TLEPanelCell> PanelCells { get; set; }

        public TLEPanelCell SelectedPanelCell { get; set; }

        //public bool Selected { get; set; }
        public string GraphicName { get; set; }
        public Placement Placement { get; set; }

        public TLEPanel(string name)
        {
            Placement = new Placement();
            if (string.IsNullOrEmpty(name))
                throw new Exception("PANEL MUST HAVE A NAME!!!");
        }

        public void InitializeStackedElements()
        {
            StackedElements = new Dictionary<TimeSpan, List<TLEPanelCell>>();

            TimeSpan currentTimeSpan = new TimeSpan(StartTime.Ticks);
            currentTimeSpan = currentTimeSpan.Add(TimeCellWidth);

            List<TLEPanelCell> group = new List<TLEPanelCell>();
            var orderedPanelCells = PanelCells.OrderBy(o => o.ReferenceElement.GetStartTime()).ToList();

            //bool first = true;

            for (int i = 0; i < orderedPanelCells.Count; i++)
            {
                if (orderedPanelCells[i].ReferenceElement.GetStartTime() < currentTimeSpan)
                    group.Add(orderedPanelCells[i]);
                else
                {
                    StackedElements.Add(currentTimeSpan - TimeCellWidth, group);
                    group = new List<TLEPanelCell>();

                    if (currentTimeSpan > EndTime)
                        break;//fuse

                    currentTimeSpan = currentTimeSpan.Add(TimeCellWidth);
                    i--;
                    continue;
                }
            }

            if(group != null && group.Count > 0)
                StackedElements.Add(currentTimeSpan - TimeCellWidth, group);
        }

        public void MovePanelCellTime(string graphicName, TimeSpan destenationTime)
        {
            var cells = PanelCells.Where(w => w.GraphicName == graphicName);
            foreach (var cell in cells)
            {
                cell.ReferenceElement.SetStartTime(destenationTime);
            }
        }

        public void MovePanelCellTime(TLEPanelCell panelCell, TimeSpan destenationTime)
        {
            panelCell.ReferenceElement.SetStartTime(destenationTime);
        }

        //public void MovePanelCellTime(string graphicName, float precentage)
        //{
        //    var timespan = new TimeSpan((long)((EndTime.Ticks - StartTime.Ticks) * precentage));

        //    MovePanelCellTime(graphicName, StartTime + timespan);
        //}

        public void RemovePanelCell(TLEPanelCell panelCell)
        {
            PanelCells.Remove(panelCell);
        }

        public int RemovePanelCell(string graphicName)
        {
            return PanelCells.RemoveAll(w => w.GraphicName == graphicName);
        }

        public int CalculateMaxElementsAtColumn()
        {
            return StackedElements.Max(w => w.Value.ToList().Count);
        }

        public void UpdateTimeScope(TimeSpan startTime, TimeSpan endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
            //IniitializeCurrentElements();
        }

        public Placement CalculatePlacementForPanelCell(TLEPanelCell element)
        {
            float cellWidth = 0;
            if (TimeCellWidth == null)
                cellWidth = PANEL_CELL_WIDTH;//CalculateDynamicCellWidth(element);
            else
            {
                var diffticks = EndTime.Ticks - StartTime.Ticks;
                var numberOfPossibleCellsOnWidth = (((float)diffticks) / TimeCellWidth.Ticks);
                cellWidth = Placement.Width / numberOfPossibleCellsOnWidth;
            }

            var timestampWithGroup = GetStackedElementsForTimeSpan(element.ReferenceElement.GetStartTime());
            //TODO Leaving here this needs to change
            var orderInderForGroupKey = StackedElements.Keys.ToList().IndexOf(timestampWithGroup.Key);
            var indexInGroup = timestampWithGroup.Value.IndexOf(element);
            if (indexInGroup == -1)
                throw new Exception("Nesu: -1 for " + nameof(indexInGroup));

            Placement placement = new Placement();
            placement.X = orderInderForGroupKey * cellWidth;
            placement.Height = Placement.Height / timestampWithGroup.Value.Count;
            placement.Y = Placement.Y + Placement.Height - (placement.Height * indexInGroup);
            placement.Width = cellWidth;
            placement.Rotation = 0;

            return placement;
        }

        //public float CalculateDynamicCellWidth(TLEPanelCell element)
        //{
        //    var frame = element.AnimationElement as AnimationRouteFrame;
        //    if(frame != null)
        //    {
        //        var diffTime = EndTime - StartTime;
        //        var cellWidth = ((float)frame.FrameTime.Ticks / diffTime.Ticks) * Placement.Width;
        //        return cellWidth;
        //    }

        //    throw new Exception("Nesu: Unable to calculate dynamic cell width");
        //}

        public KeyValuePair<TimeSpan, List<TLEPanelCell>> GetStackedElementsForTimeSpan(TimeSpan timespan)
        {
            var timespanKey = StackedElements.Where(w => w.Key == timespan).Max(m => m.Key);

            return new KeyValuePair<TimeSpan, List<TLEPanelCell>>(timespanKey, StackedElements[timespanKey]);
        }

        public ITLEPanelCellElement GetCellElementBasedOnGraphicName(string graphicName)
        {
            if (string.IsNullOrEmpty(graphicName))
                return null;

            TLEPanelCell cell = PanelCells.Where(w => w.GraphicName == graphicName).FirstOrDefault();
            if (cell != null)
            {
                return cell.ReferenceElement;
            }
            return null;
        }

        public TLEPanelCell GetCell(string graphicName)
        {
            if (string.IsNullOrEmpty(graphicName))
                return null;

            return PanelCells.FirstOrDefault(f => f.GraphicName == graphicName);
        }

        public TLEPanelCell GetCell(ITLEPanelCellElement referenceElement)
        {
            if (referenceElement == null)
                return null;

            return PanelCells.FirstOrDefault(f => f.ReferenceElement == referenceElement);
        }

        public void SelectPanelCell(string elementName)
        {
            TLEPanelCell cell = GetCell(elementName);
            SelectedPanelCell = cell;
        }

        public void SelectPanelCell(TLEPanelCell panelCell)
        {
            SelectedPanelCell = panelCell;
        }

        private void ClearCellSelection()
        {
            SelectedPanelCell = null;
            //PanelCells.ForEach(fe => fe.Selected = false);
        }
    }
}
