using DogeBeats.EngineSections.AnimationObjects;
using DogeBeats.EngineSections.Resources;
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
        public Dictionary<string, TLEPanel> GroupPanels = new Dictionary<string, TLEPanel>();

        public TimeSpan PanelOffsetTime { get; set; } = new TimeSpan();
        public TimeSpan PanelWidthTime { get; set; } = new TimeSpan(0, 0, 1, 0, 0);

        public TLEPanelTimeGraphicIndicator TimeIdentyficator { get; set; }

        public AnimationGroupElement SelectedGroup { get; set; }
        public AnimationSingleElement SelectedElement { get; set; }

        public float Width = 600;

        public TimeLineEditorPanelHub()
        {

        }

        public void InitializePanels(TimeLine timeLine)
        {
            GroupPanels.Clear();
            Panels.Clear();

            InitializeBeatPanel(timeLine.BeatGuider.GetTLECellElements());
            InitializeNewElementGroupPanel(timeLine.AnimationElements);
            InitializeAnimationElementPanel(null);
            InitializeAnimationRoutePanel(null);

            InitializeGraphicIdentyficator();
        }



        public void InitializeGraphicIdentyficator()
        {
            float panelHeight = GetAllPanelsHeight();
            TimeIdentyficator = new TLEPanelTimeGraphicIndicator(panelHeight, PanelOffsetTime, PanelOffsetTime + PanelWidthTime);
        }

        internal void Refresh()
        {
            //throw new NotImplementedException();
        }

        public void InitializeAnimationRoutePanel(AnimationGroupElement group, AnimationSingleElement element)
        {
            Panels["Route"] = InitializePanelWidthDefaultSettings();
            var timedElements = TLEPanelCell.Parse(element.Route.Frames);
            Panels["Route"].PanelCells = timedElements;
            Panels["Route"].Placement.Height = 20;
            Panels["Route"].Placement.Y = 200;
        }

        public void InitializeAnimationElementPanel(AnimationGroupElement group)
        {
            Panels["Element"] = InitializePanelWidthDefaultSettings();
            if (group != null)
            {
                var timedElements = TLEPanelCell.Parse(group.Elements);
                Panels["Element"].PanelCells = timedElements;
            }
            Panels["Element"].TimeCellWidth = new TimeSpan(0, 0, 0, 1, 0);
            Panels["Element"].Placement.Y = 100;
        }

        public void MoveTimeForPanelsElement(string elementName, float wayPrecentage)
        {
            TimeSpan destenationTime = new TimeSpan(PanelOffsetTime.Ticks + (long)(PanelWidthTime.Ticks * wayPrecentage));

            //TODO: think about it
            foreach (var panel in Panels.Values)
            {
                panel.MovePanelCellTime(elementName, destenationTime);
            }

            foreach (var panel in GroupPanels.Values)
            {
                panel.MovePanelCellTime(elementName, destenationTime);
            }
        }

        public TimeSpan SetTimeCursorToPrecentage(float precentage)
        {
            TimeSpan time = new TimeSpan(PanelOffsetTime.Ticks + (long)(PanelWidthTime.Ticks * precentage));
            return time;
        }

        public void UpdateManual(string graphicName, NameValueCollection values)
        {
            ITLEPanelCellElement refferencedObject = GetObjectFromAllPanelCellElementName(graphicName);

            if (refferencedObject as AnimationGroupElement != null)
            {
                AnimationGroupElement group = refferencedObject as AnimationGroupElement;
                if (group != null)
                {
                    group.UpdateManual(values);
                }
            }
            else if (refferencedObject as AnimationSingleElement != null)
            {
                AnimationSingleElement element = refferencedObject as AnimationSingleElement;
                if (element != null)
                {
                    element.UpdateManual(values);
                }
            }
            else if (refferencedObject as AnimationRouteFrame != null)
            {
                AnimationRouteFrame route = refferencedObject as AnimationRouteFrame;
                if (route != null)
                {
                    route.UpdateManual(values);
                }
            }
            else if (refferencedObject as Beat != null)
            {
                //no actionforBeat
            }
        }

        private ITLEPanelCellElement GetObjectFromAllPanelCellElementName(string graphicName)
        {
            foreach (var panel in Panels.Values)
            {
                var result = panel.GetCellElementBasedOnGraphicName(graphicName);
                if (result != null)
                    return result;
            }

            foreach (var panel in GroupPanels.Values)
            {
                var result = panel.GetCellElementBasedOnGraphicName(graphicName);
                if (result != null)
                    return result;
            }
            return null;
        }

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
        public ITLEPanelCellElement RemovePanelsElement(string graphicName)
        {
            //TODO why is it here
            ITLEPanelCellElement elementToRemove = GetObjectFromAllPanelCellElementName(graphicName);

            foreach (var panel in Panels.Values)
            {
                panel.RemovePanelCell(graphicName);
            }

            foreach (var panel in GroupPanels.Values)
            {
                panel.RemovePanelCell(graphicName);
            }

            return elementToRemove;
        }

        public void GroupPanelSelected(string index)
        {
            int actualIndex = 0;
            if (int.TryParse(index, out actualIndex)){
                for (int i = actualIndex; i < GroupPanels.Keys.Count; i++)
                {
                    GroupPanels.Remove(GroupPanels.Keys.ElementAt(i));
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
            return GroupPanels[(GroupPanels.Count - 1).ToString()];
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

            foreach (var panel in GroupPanels.Values)
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

            foreach (var panel in GroupPanels.Values)
            {
                panel.Selected = false;
            }

            if (Panels.ContainsKey(panelName))
                Panels[panelName].Selected = true;
            if (GroupPanels.ContainsKey(panelName))
                GroupPanels[panelName].Selected = true;

            //GetPanel(panelName).Selected = true;
        }

        public void ClearPanelSelection()
        {
            foreach (var panel in Panels.Values)
            {
                panel.Selected = false;
            }

            foreach (var panel in GroupPanels.Values)
            {
                panel.Selected = false;
            }
        }

        private void InitializeBeatPanel(List<ITLEPanelCellElement> beats)
        {
            Panels["Beat"] = InitializePanelWidthDefaultSettings();
            Panels["Beat"].TimeCellWidth = new TimeSpan(0, 0, 0, 1, 0);
            var cells = TLEPanelCell.Parse(beats);
            Panels["Beat"].PanelCells = cells;
            Panels["Beat"].Placement.Height = 40;
            Panels["Beat"].Placement.Y = 220;
        }

        private void InitializeNewElementGroupPanel(List<IAnimationElement> elements)
        {
            GroupPanels[GroupPanels.Count.ToString()] = InitializePanelWidthDefaultSettings();
            GroupPanels[GroupPanels.Count.ToString()].TimeCellWidth = new TimeSpan(0, 0, 0, 1, 0);
            List<TLEPanelCell> cells = TLEPanelCell.Parse(elements);
            GroupPanels[GroupPanels.Count.ToString()].PanelCells = cells;
            GroupPanels[GroupPanels.Count.ToString()].Placement.Y = 0;
        }

        private void InitializeAnimationRoutePanel(List<ITLEPanelCellElement> route)
        {
            Panels["Route"] = InitializePanelWidthDefaultSettings();
            var cells = TLEPanelCell.Parse(route);
            Panels["Route"].PanelCells = cells;
            Panels["Route"].Placement.Height = 20;
            Panels["Route"].Placement.Y = 200;
        }

        private TLEPanel InitializePanelWidthDefaultSettings()
        {
            TLEPanel panel = new TLEPanel();
            panel.StartTime = PanelOffsetTime;
            panel.EndTime = PanelOffsetTime + PanelWidthTime;
            panel.Placement.Height = 100;
            panel.Placement.Width = Width;
            ReCalculatePlacementYForPanels();
            return panel;
        }

        private void ReCalculatePlacementYForPanels()
        {
            float currentHeight = 0;
            foreach (var groupPanel in GroupPanels)
            {
                groupPanel.Value.Placement.Y = currentHeight;
                currentHeight += groupPanel.Value.Placement.Height;
            }

            foreach (var panel in Panels)
            {
                panel.Value.Placement.Y = currentHeight;
                currentHeight += panel.Value.Placement.Height;
            }

        }

        private float GetAllPanelsHeight()
        {
            float currentHeight = 0;
            foreach (var groupPanel in GroupPanels)
            {
                currentHeight += groupPanel.Value.Placement.Height;
            }

            foreach (var panel in Panels)
            {
                currentHeight += panel.Value.Placement.Height;
            }
            return currentHeight;
        }
    }
}
