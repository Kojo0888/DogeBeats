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

        public static string ANIMATIONELEMENT_PANEL_PREFIX => "AnimationElement_";
        public static string PANEL_NAME_ANIMATION_ROUTE => "AnimationRoute";
        public static string PANEL_NAME_BEAT => "Beat";

        public Dictionary<string, TLEPanel> Panels = new Dictionary<string, TLEPanel>();
        //public Dictionary<string, TLEPanel> GroupPanels = new Dictionary<string, TLEPanel>();
        //public Dictionary<string, TLEPanel> AllPanels = new Dictionary<string, TLEPanel>();

        private static Dictionary<string, int> PANEL_DEFAULT_HEIGHTS = new Dictionary<string, int>()
        {
            { PANEL_NAME_BEAT, 20},
            { PANEL_NAME_ANIMATION_ROUTE, 100},
        };

        public TimeSpan PanelOffsetTime { get; set; } = new TimeSpan();
        public TimeSpan PanelWidthTime { get; set; } = new TimeSpan(0, 0, 1, 0, 0);

        public TLEPanelTimeGraphicIndicator TimeIdentyficator { get; set; }

        public AnimationGroupElement SelectedGroup { get; set; }
        public AnimationSingleElement SelectedElement { get; set; }
        //public string SelectedPanelName { get; set; }

        public TLEPanel SelectedPanel { get; set; }

        public float Width = 600;

        public TLEPanelHub()
        {

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

        public void InitializePanels(TimeLine timeLine)
        {
            Panels.Clear();

            InitializePanel(PANEL_NAME_BEAT, timeLine.BeatGuider.Beats);
            InitializePanel(ANIMATIONELEMENT_PANEL_PREFIX + "0", timeLine.AnimationElements);

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
            int lastIndex = GetPanelGroupIndexes().LastOrDefault();
            if (lastIndex == default(int))
                throw new NesuException("PanelHub: Index is default... DO SOMETHING ABOUT IT!!!");

            lastIndex++;
            InitializePanel(ANIMATIONELEMENT_PANEL_PREFIX + lastIndex, animationElements);
        }

        private TLEPanel CreatePanelWithDefaultSettings(string panelName = "")
        {
            TLEPanel panel = new TLEPanel();
            panel.StartTime = PanelOffsetTime;
            panel.EndTime = PanelOffsetTime + PanelWidthTime;
            panel.PanelName = panelName;

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
            TimeSpan destenationTime = new TimeSpan(PanelOffsetTime.Ticks + (long)(PanelWidthTime.Ticks * wayPrecentage));

            //TODO: think about it
            foreach (var panel in Panels.Values)
            {
                panel.MovePanelCellTime(elementName, destenationTime);
            }
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
            var lastGroupPanelKey = Panels.Keys.Where(s => s.StartsWith(ANIMATIONELEMENT_PANEL_PREFIX)).OrderBy(o => o).LastOrDefault();
            if (lastGroupPanelKey != null && Panels.ContainsKey(lastGroupPanelKey))
            {
                return Panels[lastGroupPanelKey];
            }
            else
                return null;
        }

        public void MoveForwardPanelTimeSection()
        {
            PanelOffsetTime = new TimeSpan(PanelOffsetTime.Ticks + (PanelWidthTime.Ticks / 2));
            UpdateAllPanelTimeScope();
            TimeIdentyficator.UpdateTimeScope(PanelOffsetTime, PanelOffsetTime + PanelWidthTime);
        }

        public void MoveBackwardPanelTimeSection()
        {
            if (PanelOffsetTime.Ticks - (PanelWidthTime.Ticks / 2) < 0)
                return;

            PanelOffsetTime = new TimeSpan(PanelOffsetTime.Ticks - (PanelWidthTime.Ticks / 2));
            UpdateAllPanelTimeScope();
            TimeIdentyficator.UpdateTimeScope(PanelOffsetTime, PanelOffsetTime + PanelWidthTime);
        }

        public object GetLastPanelAnimationElementIndex()
        {
            var panelIndexe = GetPanelGroupIndexes().LastOrDefault();
            return panelIndexe;
        }

        public void UpdateAllPanelTimeScope()
        {
            var endTime = PanelOffsetTime + PanelWidthTime;

            foreach (var panel in Panels.Values)
            {
                panel.UpdateTimeScope(PanelOffsetTime, endTime);
            }
        }

        public void SelectPanel(string panelName)
        {
            SelectedPanel = Panels[panelName];

            SelectPanel_RemovePanels(panelName);
        }

        public void SelectPanel_RemovePanels(string panelName)
        {
            if (panelName.Contains(ANIMATIONELEMENT_PANEL_PREFIX))
            {
                var groupPanelIndexes = GetPanelGroupIndexes();
                var panelSelectionIndex = int.Parse(panelName.Replace(ANIMATIONELEMENT_PANEL_PREFIX, ""));

                foreach (var groupPanelIndex in groupPanelIndexes)
                {
                    if (groupPanelIndex < panelSelectionIndex)
                        Panels.Remove(ANIMATIONELEMENT_PANEL_PREFIX + groupPanelIndex);
                }

                Panels.Remove(PANEL_NAME_ANIMATION_ROUTE);
            }
        }

        public List<int> GetPanelGroupIndexes()
        {
            return Panels.Keys.Where(s => s.StartsWith(ANIMATIONELEMENT_PANEL_PREFIX)).Select(s => int.Parse(s.Replace(ANIMATIONELEMENT_PANEL_PREFIX, ""))).OrderBy(o => o).ToList();
        }

        private string GetNewGroupPanelName()
        {
            string lastPanelName = GetLastGroupPanel().PanelName;
            var splitedName = lastPanelName.Split('_');

            int lastPanelNameIndex = -1;
            if (!int.TryParse(splitedName[1], out lastPanelNameIndex))
                throw new NesuException("PanelHub: InitializeNewElementGroupPanel index is not int " + splitedName[1]);

            string newPanelName = ANIMATIONELEMENT_PANEL_PREFIX + "_" + (lastPanelNameIndex + 1);

            return newPanelName;
        }

        private void ReCalculatePlacementYForPanels()
        {
            float currentHeight = 0;

            if (Panels.ContainsKey(PANEL_NAME_BEAT))
            {
                Panels[PANEL_NAME_BEAT].Placement.Y = 0;
                currentHeight += Panels[PANEL_NAME_BEAT].Placement.Height;
            }

            var groupPanelIndexes = GetPanelGroupIndexes();
            foreach (var groupPanelIndex in groupPanelIndexes)
            {
                string groupKey = ANIMATIONELEMENT_PANEL_PREFIX + groupPanelIndex;
                if (Panels.ContainsKey(groupKey))
                {
                    Panels[groupKey].Placement.Y = currentHeight;
                    currentHeight += Panels[groupKey].Placement.Height;
                }
            }

            if (Panels.ContainsKey(PANEL_NAME_ANIMATION_ROUTE))
            {
                Panels[PANEL_NAME_ANIMATION_ROUTE].Placement.Y = currentHeight;
                currentHeight += Panels[PANEL_NAME_ANIMATION_ROUTE].Placement.Height;
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
