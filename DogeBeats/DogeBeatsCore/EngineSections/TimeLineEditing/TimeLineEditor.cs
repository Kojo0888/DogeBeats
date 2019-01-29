using DogeBeats.EngineSections.Resources;
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

namespace DogeBeats.Modules
{
    public class TimeLineEditor
    {
        public TimeLine TimeLine { get; set; }

        public AnimationGroupElement SelectedGroup { get; set; }
        public AnimationSingleElement SelectedElement { get; set; }

        public TLEPanel PanelGroup { get; set; }
        public TLEPanel PanelElement { get; set; }
        public TLEPanel PanelRoute { get; set; }
        public TLEPanel PanelBeat { get; set; }

        public TimeSpan PanelOffsetTime { get; set; } = new TimeSpan();
        public TimeSpan PanelWidthTime { get; set; } = new TimeSpan(0, 0, 1, 0, 0);

        public TLEPanelTimeGraphicIndicator TimeIdentyficator { get; set; }

        public float Width = 600;

        public TimeLineEditor()
        {

        }

        public void InitializeGraphicIdentyficator()
        {
            float panelHeight = PanelBeat.Height + PanelElement.Height + PanelGroup.Height + PanelRoute.Height;
            TimeIdentyficator = new TLEPanelTimeGraphicIndicator(panelHeight, PanelOffsetTime, PanelOffsetTime + PanelWidthTime);
        }

        public void AttachTimeLineToEditor(TimeLine timeline)
        {
            TimeLine = timeline;

            PanelGroup = new TLEPanel();
            PanelGroup.TimeCellWidth = new TimeSpan(0, 0, 0, 1, 0);
            PanelGroup.StartTime = PanelOffsetTime;
            PanelGroup.EndTime = PanelOffsetTime + PanelWidthTime;
            List<TLEPanelCell> timedElements = TLEPanelCell.Parse(timeline.AnimationElements);
            PanelGroup.InitialineElements(timedElements);
            PanelGroup.Height = 100;
            PanelGroup.OffsetHeight = 0;
            PanelGroup.Width = Width;

            PanelBeat = new TLEPanel();
            PanelBeat.TimeCellWidth = new TimeSpan(0, 0, 0, 1, 0);
            PanelBeat.StartTime = PanelOffsetTime;
            PanelBeat.EndTime = PanelOffsetTime + PanelWidthTime;
            timedElements = TLEPanelCell.Parse(timeline.BeatGuider.Beats);
            PanelBeat.InitialineElements(timedElements);
            PanelBeat.Height = 40;
            PanelBeat.OffsetHeight = 220;
            PanelBeat.Width = Width;

            PanelRoute = new TLEPanel();
            //PanelRoute.TimeCellWidth = null;// for undefined
            PanelRoute.StartTime = PanelOffsetTime;
            PanelRoute.EndTime = PanelOffsetTime + PanelWidthTime;
            //timedElements = timeline.;

            InitializeAnimationElementPanel(null);
            InitializeGraphicIdentyficator();
        }

        private void InitializeAnimationRoutePanel(AnimationGroupElement group)
        {
            PanelRoute = new TLEPanel();
            PanelRoute.StartTime = PanelOffsetTime;
            PanelRoute.EndTime = PanelOffsetTime + PanelWidthTime;
            var timedElements = TLEPanelCell.Parse(group.Route.Frames);
            PanelRoute.InitialineElements(timedElements);
            PanelRoute.Height = 20;
            PanelRoute.OffsetHeight = 200;
            PanelRoute.Width = Width;
        }

        private void InitializeAnimationRoutePanel(AnimationGroupElement group, AnimationSingleElement element)
        {
            PanelRoute = new TLEPanel();
            PanelRoute.StartTime = PanelOffsetTime;
            PanelRoute.EndTime = PanelOffsetTime + PanelWidthTime;
            var timedElements = TLEPanelCell.Parse(element.Route.Frames);
            PanelRoute.InitialineElements(timedElements);
            PanelRoute.Height = 20;
            PanelRoute.OffsetHeight = 200;
            PanelRoute.Width = Width;
        }

        private void InitializeAnimationElementPanel(AnimationGroupElement group)
        {
            PanelElement = new TLEPanel();
            if (group != null)
            {
                var timedElements = TLEPanelCell.Parse(group.Elements);
                PanelElement.InitialineElements(timedElements);
            }
            PanelElement.TimeCellWidth = new TimeSpan(0, 0, 0, 1, 0);
            PanelElement.StartTime = PanelOffsetTime;
            PanelElement.EndTime = PanelOffsetTime + PanelWidthTime;
            PanelElement.Height = 100;
            PanelElement.OffsetHeight = 100;
            PanelElement.Width = Width;
        }

        public void MoveTimeForPanelElement(string elementName, float wayPrecentage)
        {
            TimeSpan destenationTime = new TimeSpan(PanelOffsetTime.Ticks + (long)(PanelWidthTime.Ticks * wayPrecentage));
            PanelElement.MovePanelCellTime(elementName, destenationTime);
            PanelGroup.MovePanelCellTime(elementName, destenationTime);
            PanelBeat.MovePanelCellTime(elementName, destenationTime);
        }

        public void AddNewAnimationElement(string graphicGroupName)
        {
            AnimationSingleElement element = new AnimationSingleElement();
            ITLEPanelCellElement panelElement = PanelGroup.GetObjectFromCellElementName(graphicGroupName);
            if (panelElement as AnimationGroupElement != null)
            {
                AnimationGroupElement group = panelElement as AnimationGroupElement;
                group.Elements.Add(element);
            }
            else
                throw new Exception("Nesu... zjebales");

            TimeLine.Refresh();
            //PanelElement.RefreshPanelCells();
        }

        public void AddNewAnimationGroup(string groupName)
        {
            AnimationGroupElement group = new AnimationGroupElement();
            TimeLine.AnimationElements.Add(group);

            TLEPanelCell timedElement = TLEPanelCell.Parse(group);
            PanelGroup.AllElements.Add(timedElement);

            TimeLine.Refresh();
            //PanelGroup.RefreshPanelCells();
        }

        public void SetTimeCursorToPrecentage(float precentage)
        {
            TimeSpan time = new TimeSpan(PanelOffsetTime.Ticks + (long)(PanelWidthTime.Ticks * precentage));
            bool running = TimeLine.Stopper.IsRunning;
            TimeLine.Stopper.Stop();
            TimeLine.Stopper.Elapsed = time;
            if (running)
                TimeLine.Stopper.Start();
        }

        public void UpdateManual(string graphicName, NameValueCollection values)
        {
            ITLEPanelCellElement refferencedObject = GetObjectFromAllPanelCellElementName(graphicName);

            if (refferencedObject as AnimationGroupElement != null)
            {
                AnimationGroupElement group = refferencedObject as AnimationGroupElement;
                if (group != null)
                {
                    //Placement placement = group.InitPlacement;
                    //Placement.UpdateManual(placement, values);
                    //group.UpdateManual(values);
                }
            }
            else if (refferencedObject as AnimationSingleElement != null)
            {
                AnimationSingleElement element = refferencedObject as AnimationSingleElement;
                if (element != null)
                {
                    //Placement placement = element.InitPlacement;
                    //Placement.UpdateManual(placement, values);
                    //element.UpdateManual(values);
                }
            }
            else if (refferencedObject as AnimationRouteFrame != null)
            {
                AnimationRouteFrame route = refferencedObject as AnimationRouteFrame;
                if (route != null)
                {
                    //Placement placement = route.Placement;
                    //Placement.UpdateManual(placement, values);
                    //AnimationRoute.UpdateManual(route, values);
                }
            }
            else if (refferencedObject as Beat != null)
            {
                //no actionforBeat
            }
        }

        private ITLEPanelCellElement GetObjectFromAllPanelCellElementName(string graphicName)
        {
            return PanelGroup.GetObjectFromCellElementName(graphicName) ??
                PanelElement.GetObjectFromCellElementName(graphicName) ??
                PanelRoute.GetObjectFromCellElementName(graphicName) ??
                PanelBeat.GetObjectFromCellElementName(graphicName);
        }

        public void SaveTimeLine()
        {
            StaticHub.TimeLineCentre.Save(TimeLine);
        }

        public void LoadTimeLine(string timelineName)
        {
            TimeLine timeline = StaticHub.TimeLineCentre.Get(timelineName);
            if (timeline != null)
                AttachTimeLineToEditor(timeline);
        }

        //private  void RefreshAllPanelCells()
        //{
        //    PanelGroup.RefreshPanelCells();
        //    PanelElement.RefreshPanelCells();
        //    PanelBeat.RefreshPanelCells();
        //}

        public void UpdateRoutePlacement(string graphicName, Placement placement)
        {
            var refferencedObject = PanelElement.GetObjectFromCellElementName(graphicName);
            if (refferencedObject as AnimationRouteFrame == null)
                return;

            AnimationRouteFrame element = refferencedObject as AnimationRouteFrame;
            element.CheckpointPosition = placement;

            TimeLine.Refresh();
        }

        public void UpdatePlacement(string graphicName, Placement placement)
        {
            //UpdateGroupPlacement(graphicName, placement);
            //UpdateElementPlacement(graphicName, placement);
            UpdateRoutePlacement(graphicName, placement);

            TimeLine.Refresh();
        }

        //public  void UpdateGroupPlacement(string graphicName, Placement placement)
        //{
        //    var refferencedObject = PanelElement.GetObjectFromCellElementName(graphicName);
        //    if (refferencedObject as AnimationGroupElement == null)
        //        return;

        //    AnimationGroupElement element = refferencedObject as AnimationGroupElement;
        //    element.InitPlacement = placement;
        //}

        //public  void UpdateElementPlacement(string graphicName, Placement placement)
        //{
        //    var refferencedObject = PanelElement.GetObjectFromCellElementName(graphicName);
        //    if (refferencedObject as AnimationSingleElement == null)
        //        return;

        //    AnimationSingleElement element = refferencedObject as AnimationSingleElement;
        //    element.InitPlacement = placement;
        //}

        //public  void SelectAnimationGroup(string name)
        //{
        //    var group = TimeLine.AnimationGroupElements.FirstOrDefault(f => f.GraphicName == name);
        //    if (group != null)
        //    {
        //        InitializeAnimationElementPanel(group);
        //    }
        //}

        public void RegisterBeat()
        {
            TimeLine.BeatGuider.RegisterBeat(TimeLine.Stopper.Elapsed);
            //PanelBeat.RefreshPanelCells();
        }

        //No intel about succesful removal
        public void RemovePanelElement(string graphicName)
        {
            ITLEPanelCellElement elementToRemove = GetObjectFromAllPanelCellElementName(graphicName);
            RemoveTimeLineElement(elementToRemove);

            PanelElement.RemovePanelCell(graphicName);
            PanelGroup.RemovePanelCell(graphicName);
            PanelBeat.RemovePanelCell(graphicName);
            PanelRoute.RemovePanelCell(graphicName);

            TimeLine.Refresh();
        }

        //temporary... To be decided if removal should go to TimeLine class
        private void RemoveTimeLineElement(ITLEPanelCellElement elementToRemove)
        {
            if (elementToRemove as AnimationGroupElement != null)
            {
                var group = elementToRemove as AnimationGroupElement;
                if (group != null)
                {
                    TimeLine.AnimationElements.Remove(group);
                }
            }
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

        public void Play()
        {
            TimeLine.ResumeStoryboard();
        }

        public void PlayFromTo(TimeSpan from, TimeSpan to)
        {
            TimeLine.Stopper.Stop();
            TimeLine.Stopper.Elapsed = from;
            TimeLine.RegisterPlayToTimeSpan(to);
            TimeLine.Stopper.Start();
        }

        public void Stop()
        {
            TimeLine.PauseStoryboard(false);
        }

        public void ChangeCurrentTime(TimeSpan newTimespamp)
        {
            TimeLine.Stopper.Stop();
            TimeLine.Stopper.Elapsed = newTimespamp;
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

            PanelGroup.UpdateSectionTime(PanelOffsetTime, endTime);
            PanelElement.UpdateSectionTime(PanelOffsetTime, endTime);
            PanelBeat.UpdateSectionTime(PanelOffsetTime, endTime);
            PanelRoute.UpdateSectionTime(PanelOffsetTime, endTime);
        }

        //public  List<string> GetKeysForManualUpdate(Type type)
        //{
        //    List<string> keys = new List<string>();
        //    keys.AddRange(Placement.GetKeysManualUpdate());
        //    if (type == typeof(AnimationGroupElement))
        //    {
        //        keys.AddRange(Placement.GetKeysManualUpdate());
        //        keys.AddRange(AnimationGroupElement.GetKeysManualUpdate());
        //    }
        //    else if (type == typeof(AnimationSingleElement))
        //    {
        //        keys.AddRange(Placement.GetKeysManualUpdate());
        //        keys.AddRange(AnimationSingleElement.GetKeysManualUpdate());
        //    }
        //    else if (type == typeof(AnimationRouteFrame))
        //    {
        //        keys.AddRange(Placement.GetKeysManualUpdate());
        //        keys.AddRange(AnimationRouteFrame.GetKeysManualUpdate());
        //    }
        //    else if (type == typeof(Beat))
        //    {
        //        // noting to return 
        //        return new List<string>();
        //    }

        //    return keys;
        //}

        public void SelectPanel(string panelName)
        {
            GetPanel(panelName).Selected = true;
        }

        public void ClearPanelSelection()
        {
            PanelRoute.Selected = false;
            PanelElement.Selected = false;
            PanelGroup.Selected = false;
            PanelBeat.Selected = false;
        }

        public TLEPanel GetPanel(string panelName)
        {
            switch (panelName)
            {
                case "PanelElement":
                    return PanelElement;
                case "PanelGroup":
                    return PanelGroup;
                case "PanelBeat":
                    return PanelBeat;
                case "PanelRoute":
                    return PanelRoute;
            }

            return null;
        }
    }
}
