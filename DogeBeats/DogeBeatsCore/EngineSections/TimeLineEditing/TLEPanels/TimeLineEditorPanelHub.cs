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
    public class TimeLineEditorPanelHub
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
            { "Beat", 20},
            { "AnimationElement", 100},
            { "AnimationGroup", 100},
            { "AnimationRoute", 100},
        };

        public TimeSpan PanelOffsetTime { get; set; } = new TimeSpan();
        public TimeSpan PanelWidthTime { get; set; } = new TimeSpan(0, 0, 1, 0, 0);

        public TLEPanelTimeGraphicIndicator TimeIdentyficator { get; set; }

        public AnimationGroupElement SelectedGroup { get; set; }
        public AnimationSingleElement SelectedElement { get; set; }

        public float Width = 600;

        public TimeLineEditorPanelHub()
        {

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

            InitializePanel("Beats", timeLine.BeatGuider.Beats);

            //InitializeBeatPanel(timeLine.BeatGuider.GetTLECellElements());
            //InitializeNewElementGroupPanel(timeLine.AnimationElements);
            //InitializeAnimationElementPanel(null);
            //InitializeAnimationRoutePanel(null);

            InitializeGraphicIdentyficator();
        }

        public void InitializePanel<T>(string panelName, List<T> elements)
        {
            Panels[panelName] = CreatePanelWithDefaultSettings();
            Panels[panelName].PanelCells = TLEPanelCell.Parse(elements);

            ReCalculatePlacementYForPanels();
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


        //public void InitializeAnimationRoutePanel(AnimationGroupElement group, AnimationSingleElement element)
        //{
        //    Panels["Route"] = CreatePanelWithDefaultSettings();
        //    var timedElements = TLEPanelCell.Parse(element.Route.Frames);
        //    Panels["Route"].PanelCells = timedElements;
        //    Panels["Route"].Placement.Height = 20;
        //    Panels["Route"].Placement.Y = 200;
        //}

        //public void InitializeAnimationElementPanel(List<AnimationSingleElement> elements)
        //{
        //    Panels["Element"] = CreatePanelWithDefaultSettings();
        //    if (elements != null)
        //    {
        //        var timedElements = TLEPanelCell.Parse(elements.OfType<ITLEPanelCellElement>().ToList());
        //        Panels["Element"].PanelCells = timedElements;
        //    }
        //    Panels["Element"].TimeCellWidth = new TimeSpan(0, 0, 0, 1, 0);
        //    Panels["Element"].Placement.Y = 100;
        //}

        //public void InitializeSpecificGroupPanel(String tLEPanelName, List<IAnimationElement> elements)
        //{
        //    GroupPanels[tLEPanelName] = CreatePanelWithDefaultSettings();
        //    GroupPanels[tLEPanelName].TimeCellWidth = new TimeSpan(0, 0, 0, 1, 0);
        //    List<TLEPanelCell> cells = TLEPanelCell.Parse(elements);
        //    GroupPanels[tLEPanelName].PanelCells = cells;

        //    ReCalculatePlacementYForPanels();
        //}

        //public void InitializeBeatPanel(List<ITLEPanelCellElement> beats)
        //{
        //    Panels["Beat"] = CreatePanelWithDefaultSettings();
        //    Panels["Beat"].TimeCellWidth = new TimeSpan(0, 0, 0, 1, 0);
        //    var cells = TLEPanelCell.Parse(beats);
        //    Panels["Beat"].PanelCells = cells;
        //    Panels["Beat"].Placement.Height = 40;

        //    ReCalculatePlacementYForPanels();
        //}

        //public void InitializeNewElementGroupPanel(List<IAnimationElement> elements)
        //{
        //    string newPanelName = GetNewGroupPanelName();

        //    Panels[newPanelName] = CreatePanelWithDefaultSettings();
        //    Panels[newPanelName].TimeCellWidth = new TimeSpan(0, 0, 0, 1, 0);
        //    List<TLEPanelCell> cells = TLEPanelCell.Parse(elements);
        //    Panels[newPanelName].PanelCells = cells;

        //    ReCalculatePlacementYForPanels();
        //}

        //private void InitializeAnimationRoutePanel(List<ITLEPanelCellElement> route)
        //{
        //    Panels["Route"] = CreatePanelWithDefaultSettings();
        //    var cells = TLEPanelCell.Parse(route);
        //    Panels["Route"].PanelCells = cells;
        //    Panels["Route"].Placement.Height = 20;

        //    ReCalculatePlacementYForPanels();
        //}

        #endregion

        public void MoveTimeForPanelsElement(string elementName, float wayPrecentage)
        {
            TimeSpan destenationTime = new TimeSpan(PanelOffsetTime.Ticks + (long)(PanelWidthTime.Ticks * wayPrecentage));

            //TODO: think about it
            foreach (var panel in Panels.Values)
            {
                panel.MovePanelCellTime(elementName, destenationTime);
            }

            //foreach (var panel in GroupPanels.Values)
            //{
            //    panel.MovePanelCellTime(elementName, destenationTime);
            //}
        }

        public TimeSpan SetTimeCursorToPrecentage(float precentage)
        {
            TimeSpan time = new TimeSpan(PanelOffsetTime.Ticks + (long)(PanelWidthTime.Ticks * precentage));
            return time;
        }

        //public void UpdateManual(string graphicName, NameValueCollection values)
        //{
        //    ITLEPanelCellElement refferencedObject = GetObjectFromAllPanelCellElementName(graphicName);

        //    if (refferencedObject as AnimationGroupElement != null)
        //    {
        //        AnimationGroupElement group = refferencedObject as AnimationGroupElement;
        //        if (group != null)
        //        {
        //            group.UpdateManual(values);
        //        }
        //    }
        //    else if (refferencedObject as AnimationSingleElement != null)
        //    {
        //        AnimationSingleElement element = refferencedObject as AnimationSingleElement;
        //        if (element != null)
        //        {
        //            element.UpdateManual(values);
        //        }
        //    }
        //    else if (refferencedObject as AnimationRouteFrame != null)
        //    {
        //        AnimationRouteFrame route = refferencedObject as AnimationRouteFrame;
        //        if (route != null)
        //        {
        //            route.UpdateManual(values);
        //        }
        //    }
        //    else if (refferencedObject as Beat != null)
        //    {
        //        //no actionforBeat
        //    }
        //}

        //private ITLEPanelCellElement GetObjectFromAllPanelCellElementName(string graphicName)
        //{
        //    foreach (var panel in Panels.Values)
        //    {
        //        var result = panel.GetCellElementBasedOnGraphicName(graphicName);
        //        if (result != null)
        //            return result;
        //    }

        //    foreach (var panel in GroupPanels.Values)
        //    {
        //        var result = panel.GetCellElementBasedOnGraphicName(graphicName);
        //        if (result != null)
        //            return result;
        //    }
        //    return null;
        //}

        //will it be even used?
        //public  List<AnimationGroupElement> GetAnimationGroupElementsFromTimePoint(TimeSpan timestamp)
        //{
        //    List<AnimationGroupElement> groups = new List<AnimationGroupElement>();

        //    foreach (var animationGroup in TimeLine.AnimationGroupElements)
        //    {
        //        var slider = animationGroup.Route.GetFrameSlider(timestamp);
        //        if (slider.NextFrame != null && slider.PreviousFrame != null)
        //            groups.Add(animationGroup);
        //    }

        //    return groups;
        //}

        //No intel about succesful removal
        //public ITLEPanelCellElement RemovePanelsElement(string graphicName)
        //{
        //    //TODO why is it here
        //    ITLEPanelCellElement elementToRemove = GetObjectFromAllPanelCellElementName(graphicName);

        //    foreach (var panel in Panels.Values)
        //    {
        //        panel.RemovePanelCell(graphicName);
        //    }

        //    foreach (var panel in GroupPanels.Values)
        //    {
        //        panel.RemovePanelCell(graphicName);
        //    }

        //    return elementToRemove;
        //}

        public void PanelSelected(string index)
        {
            int actualIndex = 0;
            if (int.TryParse(index, out actualIndex)){
                for (int i = actualIndex; i < Panels.Keys.Count; i++)
                {
                    Panels.Remove(Panels.Keys.ElementAt(i));
                }
            }

            RefreshGroupPanels();
        }

        private void RefreshGroupPanels()
        {
            ReCalculatePlacementYForPanels();
        }

        public TLEPanel GetLastGroupPanel()
        {
            var lastGroupPanelKey = Panels.Keys.Where(s => s.StartsWith("Group")).OrderBy(o => o).LastOrDefault();
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
            UpdatePanelSectionTimeForAllPanels();
            InitializeGraphicIdentyficator();
        }

        public void MoveBackPanelTimeSection()
        {
            if (PanelOffsetTime.Ticks - (PanelWidthTime.Ticks / 2) < 0)
                return;

            PanelOffsetTime = new TimeSpan(PanelOffsetTime.Ticks - (PanelWidthTime.Ticks / 2));
            UpdatePanelSectionTimeForAllPanels();
            InitializeGraphicIdentyficator();
        }

        public void UpdatePanelSectionTimeForAllPanels()
        {
            var endTime = PanelOffsetTime + PanelWidthTime;

            foreach (var panel in Panels.Values)
            {
                panel.UpdateSectionTime(PanelOffsetTime, endTime);
            }
        }

        public void SelectPanel(string panelName)
        {
            foreach (var panel in Panels.Values)
            {
                panel.Selected = false;
            }

            if (Panels.ContainsKey(panelName))
                Panels[panelName].Selected = true;
        }

        public void ClearPanelSelection()
        {
            foreach (var panel in Panels.Values)
            {
                panel.Selected = false;
            }
        }

       

        private string GetNewGroupPanelName()
        {
            string lastPanelName = GetLastGroupPanel().PanelName;
            var splitedName = lastPanelName.Split('_');

            int lastPanelNameIndex = -1;
            if (!int.TryParse(splitedName[1], out lastPanelNameIndex))
                throw new NesuException("PanelHub: InitializeNewElementGroupPanel index is not int " + splitedName[1]);

            string newPanelName = splitedName[0] + "_" + (lastPanelNameIndex + 1);

            return newPanelName;
        }

        

        private void ReCalculatePlacementYForPanels()
        {
            float currentHeight = 0;

            foreach (var panel in Panels)
            {
                panel.Value.Placement.Y = currentHeight;
                currentHeight += panel.Value.Placement.Height;
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
