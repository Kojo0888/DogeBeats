using DogeBeats.EngineSections.AnimationObjects;
using DogeBeats.EngineSections.Resources;
using DogeBeats.EngineSections.Shared;
using DogeBeats.EngineSections.TimeLineEditing.TLEPanels;
using DogeBeats.Model;
using DogeBeats.Modules.TimeLines;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.EngineSections.TimeLineEditing.TLEPanels
{
    public class TLEPanelHub
    {
        //public TLEPanel Panels["Group"] { get; set; }
        //public TLEPanel Panels["Element"] { get; set; }
        //public TLEPanel Panels["Route"] { get; set; }
        //public TLEPanel Panels["Beat"] { get; set; }

        public Dictionary<string, TLEPanel> Panels = new Dictionary<string, TLEPanel>();
        //public Dictionary<string, TLEPanel> GroupPanels = new Dictionary<string, TLEPanel>();
        //public Dictionary<string, TLEPanel> AllPanels = new Dictionary<string, TLEPanel>();

        private static Dictionary<string, int> PANEL_DEFAULT_HEIGHTS = new Dictionary<string, int>()
        {
            { TLEPanelNames.BEAT, 20},
            { TLEPanelNames.ANIMATION_ROUTE, 100},
        };

        public TimeSpan PanelOffsetTime { get; set; } = new TimeSpan();
        public TimeSpan PanelWidthTime { get; set; } = new TimeSpan(0, 0, 1, 0, 0);

        public TLEPanelTimeGraphicIndicator TimeIdentyficator { get; set; }

        //public AnimationGroupElement SelectedGroup { get; set; }
        //public AnimationSingleElement SelectedElement { get; set; }
        //public string SelectedPanelName { get; set; }

        public TLEPanel SelectedPanel { get; set; }

        public float Width = 600;

        public TLEPanelHub()
        {
            TimeIdentyficator = new TLEPanelTimeGraphicIndicator(GetAllPanelsHeight(), int.Parse(Width.ToString()), PanelOffsetTime, PanelOffsetTime + PanelWidthTime);
        }

        public TLEPanel GetPanel(string panelName)
        {
            if (Panels.ContainsKey(panelName))
            {
                return Panels[panelName];
            }
            else
                return null;
        }

        #region Initialization

        public void InitializeGraphicIdentyficator()
        {
            float panelHeight = GetAllPanelsHeight();
            TimeIdentyficator = new TLEPanelTimeGraphicIndicator(panelHeight, StaticHub.EnvironmentVariables.MainWindowWidth, PanelOffsetTime, PanelOffsetTime + PanelWidthTime);
        }

        public void InitializeDefaultPanels(TimeLine timeLine)
        {
            Panels.Clear();

            InitializePanel(TLEPanelNames.BEAT, timeLine.BeatGuider.Beats);
            InitializePanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "0", timeLine.AnimationElements);

            InitializeGraphicIdentyficator();
        }

        public void InitializePanel<T>(string panelName, List<T> elements)
        {
            Panels[panelName] = CreatePanelWithDefaultSettings();
            Panels[panelName].PanelCells = TLEPanelCell.Parse(elements);

            ReCalculatePlacementYForPanels();
        }

        public void InitializeNewAnimationPanel<T>(List<T> animationElements)
        {
            var indexes = GetPanelGroupIndexes();
            int lastIndex = -1;

            if(indexes.Count > 0)
            {
                lastIndex = indexes.LastOrDefault();
                //if (lastIndex == default(int))
                //    throw new NesuException("PanelHub: Index is default... DO SOMETHING ABOUT IT!!!");
            }

            lastIndex++;
            InitializePanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + lastIndex, animationElements);
        }

        private TLEPanel CreatePanelWithDefaultSettings(string panelName = "")
        {
            TLEPanel panel = new TLEPanel();
            panel.StartTime = PanelOffsetTime;
            panel.EndTime = PanelOffsetTime + PanelWidthTime;
            panel.PanelName = panelName;
            panel.Placement = new Placement();

            if (!string.IsNullOrEmpty(panelName) && PANEL_DEFAULT_HEIGHTS.ContainsKey(panelName))
                panel.Placement.Height = PANEL_DEFAULT_HEIGHTS[panelName];
            else //Groups
                panel.Placement.Height = 100;

            panel.Placement.Width = Width;
            return panel;
        }

        #endregion

        public void MoveTimeForPanelsElement(string elementName, float wayPrecentage)
        {
            TimeIdentyficator.MovePrecentage(wayPrecentage);

            TimeSpan destenationTime = TimeIdentyficator.GetTime();

            foreach (var panel in Panels.Values)
            {
                panel.MovePanelCellTime(elementName, destenationTime);
            }
        }


        public void MoveTimeForPanelsElement(ITLEPanelCellElement element, float wayPrecentage)
        {
            TimeIdentyficator.MovePrecentage(wayPrecentage);

            TimeSpan destenationTime = TimeIdentyficator.GetTime();

            element.SetStartTime(destenationTime);
        }

        public TLEPanelCell GetPanelCell(string grahicName)
        {
            foreach (var panel in Panels.Values)
            {
                var cell = panel.PanelCells.FirstOrDefault(f => f.GraphicName == grahicName);
                if (cell != null)
                    return cell;
            }
            return null;
        }

        public TLEPanelCell GetPanelCellBasedOnReferenceElement(ITLEPanelCellElement element)
        {
            foreach (var panel in Panels.Values)
            {
                var cell = panel.PanelCells.FirstOrDefault(f => f.ReferenceElement == element);
                if (cell != null)
                    return cell;
            }
            return null;
        }

        public TimeSpan SetTimeCursorToPrecentage(float precentage)
        {
            TimeIdentyficator.MovePrecentage(precentage);
            //TimeSpan time = new TimeSpan(PanelOffsetTime.Ticks + (long)(PanelWidthTime.Ticks * precentage));
            TimeSpan time = TimeIdentyficator.GetTime();
            return time;
        }

        private void RefreshPanels()
        {
            ReCalculatePlacementYForPanels();
        }

        public TLEPanel GetLastGroupPanel()
        {
            var lastGroupPanelKey = Panels.Keys.Where(s => s.StartsWith(TLEPanelNames.ANIMATION_ELEMENT_PREFIX)).OrderBy(o => o).LastOrDefault();
            if (lastGroupPanelKey != null && Panels.ContainsKey(lastGroupPanelKey))
            {
                return Panels[lastGroupPanelKey];
            }
            else
                return null;
        }

        public void MoveForwardTimeScope()
        {
            PanelOffsetTime = new TimeSpan(PanelOffsetTime.Ticks + PanelWidthTime.Ticks);
            UpdateTimeScope();
            TimeIdentyficator.UpdateTimeScope(PanelOffsetTime, PanelOffsetTime + PanelWidthTime);
        }

        public void MoveBackwardTimeScope()
        {
            if (PanelOffsetTime.Ticks - PanelWidthTime.Ticks < 0)
            {
                PanelOffsetTime = new TimeSpan();
            }
            else
            {
                PanelOffsetTime = new TimeSpan(PanelOffsetTime.Ticks - PanelWidthTime.Ticks);
            }

            UpdateTimeScope();
            TimeIdentyficator.UpdateTimeScope(PanelOffsetTime, PanelOffsetTime + PanelWidthTime);
        }

        public object GetLastPanelAnimationElementIndex()
        {
            var panelIndexe = GetPanelGroupIndexes().LastOrDefault();
            return panelIndexe;
        }

        public void UpdateTimeScope(TimeSpan from, TimeSpan to)
        {
            PanelWidthTime = new TimeSpan(to.Ticks - from.Ticks);
            PanelOffsetTime = from;

            foreach (var panel in Panels.Values)
            {
                panel.UpdateTimeScope(from, to);
            }

            TimeIdentyficator.StartTime = from;
            TimeIdentyficator.EndTime = to;
        }

        public void UpdateTimeScope()
        {
            UpdateTimeScope(PanelOffsetTime, PanelOffsetTime + PanelWidthTime);
        }

        public void SelectPanel(string panelGraphicalName)
        {
            foreach (var panel in Panels.Values)
            {
                if(panel.GraphicName == panelGraphicalName)
                {
                    SelectedPanel = panel;
                    SelectPanel_RemovePanels(SelectedPanel.PanelName);
                }
            }
        }

        public void SelectPanelAndPanelElement(string elementName)
        {
            foreach (var Panel in Panels.Values)
            {
                var searchedElement = Panel.PanelCells.FirstOrDefault(f => f.GraphicName == elementName);
                if (searchedElement != null)
                {
                    SelectedPanel = Panel;
                    Panel.SelectedPanelCell = searchedElement;
                }
            }
        }

        public void SelectPanel_RemovePanels(string panelName)
        {
            if (panelName.Contains(TLEPanelNames.ANIMATION_ELEMENT_PREFIX))
            {
                var groupPanelIndexes = GetPanelGroupIndexes();
                var panelSelectionIndex = int.Parse(panelName.Replace(TLEPanelNames.ANIMATION_ELEMENT_PREFIX, ""));

                foreach (var groupPanelIndex in groupPanelIndexes)
                {
                    if (groupPanelIndex < panelSelectionIndex)
                        Panels.Remove(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + groupPanelIndex);
                }

                Panels.Remove(TLEPanelNames.ANIMATION_ROUTE);
            }
        }

        public List<int> GetPanelGroupIndexes()
        {
            return Panels.Keys
                .Where(s => s.StartsWith(TLEPanelNames.ANIMATION_ELEMENT_PREFIX))
                .Select(s => int.Parse(s.Replace(TLEPanelNames.ANIMATION_ELEMENT_PREFIX, "")))
                .OrderBy(o => o)
                .ToList();
        }

        private string GetNewGroupPanelName()
        {
            string lastPanelName = GetLastGroupPanel().PanelName;
            var splitedName = lastPanelName.Split('_');

            int lastPanelNameIndex = -1;
            if (!int.TryParse(splitedName[1], out lastPanelNameIndex))
                throw new NesuException("PanelHub: InitializeNewElementGroupPanel index is not int " + splitedName[1]);

            string newPanelName = TLEPanelNames.ANIMATION_ELEMENT_PREFIX + "_" + (lastPanelNameIndex + 1);

            return newPanelName;
        }

        private void ReCalculatePlacementYForPanels()
        {
            float currentHeight = 0;

            if (Panels.ContainsKey(TLEPanelNames.BEAT))
            {
                Panels[TLEPanelNames.BEAT].Placement.Y = 0;
                currentHeight += Panels[TLEPanelNames.BEAT].Placement.Height;
            }

            var groupPanelIndexes = GetPanelGroupIndexes();
            foreach (var groupPanelIndex in groupPanelIndexes)
            {
                string groupKey = TLEPanelNames.ANIMATION_ELEMENT_PREFIX + groupPanelIndex;
                if (Panels.ContainsKey(groupKey))
                {
                    Panels[groupKey].Placement.Y = currentHeight;
                    currentHeight += Panels[groupKey].Placement.Height;
                }
            }

            if (Panels.ContainsKey(TLEPanelNames.ANIMATION_ROUTE))
            {
                Panels[TLEPanelNames.ANIMATION_ROUTE].Placement.Y = currentHeight;
                currentHeight += Panels[TLEPanelNames.ANIMATION_ROUTE].Placement.Height;
            }
        }

        private float GetAllPanelsHeight()
        {
            float currentHeight = 0;

            foreach (var panel in Panels)
            {
                currentHeight += panel.Value.Placement.Height;
            }
            return currentHeight;
        }
    }
}
