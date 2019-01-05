using DogeBeats.Model;
using DogeBeats.Model.Route;
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
    public static class TimeLineEditor
    {
        public static TimeLine TimeLine { get; set; }

        public static AnimationGroupElement SelectedGroup { get; set; }
        public static AnimationElement SelectedElement { get; set; }

        public static TLEPanel PanelGroup { get; set; }
        public static TLEPanel PanelElement { get; set; }
        public static TLEPanel PanelRoute { get; set; }
        public static TLEPanel PanelBeat { get; set; }

        public static TimeSpan PanelOffsetTime { get; set; } = new TimeSpan();
        public static TimeSpan PanelWidthTime { get; set; } = new TimeSpan(0, 0, 1, 0, 0);

        public static TLEPanelTimeGraphicIndicator TimeIdentyficator { get; set; }

        public static float Width = 600;

        static TimeLineEditor()
        {

        }

        public static void InitializeGraphicIdentyficator()
        {
            float panelHeight = PanelBeat.Height + PanelElement.Height + PanelGroup.Height + PanelRoute.Height;
            TimeIdentyficator = new TLEPanelTimeGraphicIndicator(panelHeight, PanelOffsetTime, PanelOffsetTime + PanelWidthTime);
        }

        public static void AttachTimeLineToEditor(TimeLine timeline)
        {
            TimeLine = timeline;

            PanelGroup = new TLEPanel();
            PanelGroup.TimeCellWidth = new TimeSpan(0, 0, 0, 1, 0);
            PanelGroup.StartTime = PanelOffsetTime;
            PanelGroup.EndTime = PanelOffsetTime + PanelWidthTime;
            List<TimedTLEPanelElement> timedElements = TimedTLEPanelElement.Parse(timeline.AnimationGroupElements);
            PanelGroup.InitialineElements(timedElements);
            PanelGroup.Height = 100;
            PanelGroup.OffsetHeight = 0;
            PanelGroup.Width = Width;

            PanelBeat = new TLEPanel();
            PanelBeat.TimeCellWidth = new TimeSpan(0, 0, 0, 1, 0);
            PanelBeat.StartTime = PanelOffsetTime;
            PanelBeat.EndTime = PanelOffsetTime + PanelWidthTime;
            timedElements = timeline.BeatGuider.Beats;
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

        private static void InitializeAnimationRoutePanel(AnimationGroupElement group)
        {
            PanelRoute = new TLEPanel();
            PanelRoute.StartTime = PanelOffsetTime;
            PanelRoute.EndTime = PanelOffsetTime + PanelWidthTime;
            var timedElements = TimedTLEPanelElement.Parse(group.GroupRoute.Frames, group.GroupRoute.AnimationStartTime);
            PanelRoute.InitialineElements(timedElements);
            PanelRoute.Height = 20;
            PanelRoute.OffsetHeight = 200;
            PanelRoute.Width = Width;
        }

        private static void InitializeAnimationRoutePanel(AnimationGroupElement group, AnimationElement element)
        {
            PanelRoute = new TLEPanel();
            PanelRoute.StartTime = PanelOffsetTime;
            PanelRoute.EndTime = PanelOffsetTime + PanelWidthTime;
            var timedElements = TimedTLEPanelElement.Parse(element.Route.Frames, group.GroupRoute.AnimationStartTime);
            PanelRoute.InitialineElements(timedElements);
            PanelRoute.Height = 20;
            PanelRoute.OffsetHeight = 200;
            PanelRoute.Width = Width;
        }

        private static void InitializeAnimationElementPanel(AnimationGroupElement group)
        {
            PanelElement = new TLEPanel();
            if (group != null)
            {
                var timedElements = TimedTLEPanelElement.Parse(group.Elements);
                PanelElement.InitialineElements(timedElements);
            }
            PanelElement.TimeCellWidth = new TimeSpan(0, 0, 0, 1, 0);
            PanelElement.StartTime = PanelOffsetTime;
            PanelElement.EndTime = PanelOffsetTime + PanelWidthTime;
            PanelElement.Height = 100;
            PanelElement.OffsetHeight = 100;
            PanelElement.Width = Width;
        }

        internal static void MoveTimeForPanelElement(string elementName, float wayPrecentage)
        {
            TimeSpan destenationTime = new TimeSpan(PanelOffsetTime.Ticks + (long)(PanelWidthTime.Ticks * wayPrecentage));
            PanelElement.MovePanelCellTime(elementName, destenationTime);
            PanelGroup.MovePanelCellTime(elementName, destenationTime);
            PanelBeat.MovePanelCellTime(elementName, destenationTime);
        }

        internal static void AddNewAnimationElement(string graphicGroupName)
        {
            AnimationElement element = new AnimationElement();
            ITLEPanelElement panelElement = PanelGroup.GetObjectFromCellElementName(graphicGroupName);
            if (panelElement as AnimationGroupElement != null)
            {
                AnimationGroupElement group = panelElement as AnimationGroupElement;
                group.Elements.Add(element);
            }
            else
                throw new Exception("Nesu... zjebales");

            TimeLine.RefreshCurrentlyAnimatingElementList();
            PanelElement.RefreshPanelCells();
        }

        internal static void AddNewAnimationGroup(string groupName)
        {
            AnimationGroupElement group = new AnimationGroupElement();
            TimeLine.AnimationGroupElements.Add(group);

            TimedTLEPanelElement timedElement = TimedTLEPanelElement.Parse(group);
            PanelGroup.AllElements.Add(timedElement);

            TimeLine.RefreshCurrentlyAnimatingElementList();
            PanelGroup.RefreshPanelCells();
        }

        internal static void SetTimeCursorToPrecentage(float precentage)
        {
            TimeSpan time = new TimeSpan(PanelOffsetTime.Ticks + (long)(PanelWidthTime.Ticks * precentage));
            bool running = TimeLine.Stopper.IsRunning;
            TimeLine.Stopper.Stop();
            TimeLine.Stopper.Elapsed = time;
            if (running)
                TimeLine.Stopper.Start();
        }

        internal static void UpdateManual(string graphicName, NameValueCollection values)
        {
            ITLEPanelElement refferencedObject = GetObjectFromAllPanelCellElementName(graphicName);

            if (refferencedObject as AnimationGroupElement != null)
            {
                AnimationGroupElement group = refferencedObject as AnimationGroupElement;
                if (group != null)
                {
                    Placement placement = group.InitPlacement;
                    Placement.UpdateManual(placement, values);
                    AnimationGroupElement.UpdateManual(group, values);
                }
            }
            else if (refferencedObject as AnimationElement != null)
            {
                AnimationElement element = refferencedObject as AnimationElement;
                if (element != null)
                {
                    Placement placement = element.InitPlacement;
                    Placement.UpdateManual(placement, values);
                    AnimationElement.UpdateManual(element, values);
                }
            }
            else if (refferencedObject as AnimationRouteFrame != null)
            {
                AnimationRouteFrame route = refferencedObject as AnimationRouteFrame;
                if (route != null)
                {
                    Placement placement = route.Placement;
                    Placement.UpdateManual(placement, values);
                    AnimationRoute.UpdateManual(route, values);
                }
            }
            else if (refferencedObject as Beat != null)
            {
                //no actionforBeat
            }
        }

        private static ITLEPanelElement GetObjectFromAllPanelCellElementName(string graphicName)
        {
            return PanelGroup.GetObjectFromCellElementName(graphicName) ??
                PanelElement.GetObjectFromCellElementName(graphicName) ??
                PanelRoute.GetObjectFromCellElementName(graphicName) ??
                PanelBeat.GetObjectFromCellElementName(graphicName);
        }

        internal static void SaveTimeLine()
        {
            CenterTimeLine.SaveTimeLine(TimeLine);
        }

        internal static void LoadTimeLine(string timelineName)
        {
            TimeLine timeline = CenterTimeLine.TimeLines.FirstOrDefault(f => f.TimeLineName == timelineName);
            if (timeline != null)
                AttachTimeLineToEditor(timeline);
        }

        private static void RefreshAllPanelCells()
        {
            PanelGroup.RefreshPanelCells();
            PanelElement.RefreshPanelCells();
            PanelBeat.RefreshPanelCells();
        }

        internal static void UpdateRoutePlacement(string graphicName, Placement placement)
        {
            var refferencedObject = PanelElement.GetObjectFromCellElementName(graphicName);
            if (refferencedObject as AnimationRouteFrame == null)
                return;

            AnimationRouteFrame element = refferencedObject as AnimationRouteFrame;
            element.Placement = placement;

            TimeLine.RefreshCurrentlyAnimatingElementList();
        }

        internal static void UpdatePlacement(string graphicName, Placement placement)
        {
            UpdateGroupPlacement(graphicName, placement);
            UpdateElementPlacement(graphicName, placement);
            UpdateRoutePlacement(graphicName, placement);

            TimeLine.RefreshCurrentlyAnimatingElementList();
        }

        public static void UpdateGroupPlacement(string graphicName, Placement placement)
        {
            var refferencedObject = PanelElement.GetObjectFromCellElementName(graphicName);
            if (refferencedObject as AnimationGroupElement == null)
                return;

            AnimationGroupElement element = refferencedObject as AnimationGroupElement;
            element.InitPlacement = placement;
        }

        public static void UpdateElementPlacement(string graphicName, Placement placement)
        {
            var refferencedObject = PanelElement.GetObjectFromCellElementName(graphicName);
            if (refferencedObject as AnimationElement == null)
                return;

            AnimationElement element = refferencedObject as AnimationElement;
            element.InitPlacement = placement;
        }

        //public static void SelectAnimationGroup(string name)
        //{
        //    var group = TimeLine.AnimationGroupElements.FirstOrDefault(f => f.GraphicName == name);
        //    if (group != null)
        //    {
        //        InitializeAnimationElementPanel(group);
        //    }
        //}

        public static void AddNewBeat()
        {
            TimeLine.BeatGuider.RegisterBeat(TimeLine.Stopper.Elapsed);
            PanelBeat.RefreshPanelCells();
        }

        //No intel about succesful removal
        public static void RemovePanelElement(string graphicName)
        {
            ITLEPanelElement elementToRemove = GetObjectFromAllPanelCellElementName(graphicName);
            RemoveTimeLineElement(elementToRemove);

            PanelElement.RemovePanelCell(graphicName);
            PanelGroup.RemovePanelCell(graphicName);
            PanelBeat.RemovePanelCell(graphicName);
            PanelRoute.RemovePanelCell(graphicName);

            TimeLine.RefreshCurrentlyAnimatingElementList();
        }

        //temporary... To be decided if removal should go to TimeLine class
        private static void RemoveTimeLineElement(ITLEPanelElement elementToRemove)
        {
            if (elementToRemove as AnimationGroupElement != null)
            {
                var group = elementToRemove as AnimationGroupElement;
                if (group != null)
                {
                    TimeLine.AnimationGroupElements.Remove(group);
                }
            }
            else if (elementToRemove as AnimationElement != null)
            {
                var element = elementToRemove as AnimationElement;
                if (element != null)
                {
                    foreach (var group in TimeLine.AnimationGroupElements)
                    {
                        group.Elements.Remove(element);
                    }
                }
            }
            else if (elementToRemove as AnimationRouteFrame != null)
            {
                var frame = elementToRemove as AnimationRouteFrame;
                if (frame != null)
                {
                    foreach (var group in TimeLine.AnimationGroupElements)
                    {
                        foreach (var element in group.Elements)
                        {
                            element.Route.Frames.Remove(frame);
                        }
                    }
                }
            }
        }

        //will it be even used?
        //public static List<AnimationGroupElement> GetAnimationGroupElementsFromTimePoint(TimeSpan timestamp)
        //{
        //    List<AnimationGroupElement> groups = new List<AnimationGroupElement>();

        //    foreach (var animationGroup in TimeLine.AnimationGroupElements)
        //    {
        //        var slider = animationGroup.GroupRoute.GetFrameSlider(timestamp);
        //        if (slider.NextFrame != null && slider.PreviousFrame != null)
        //            groups.Add(animationGroup);
        //    }

        //    return groups;
        //}

        public static void Play()
        {
            TimeLine.ResumeStoryboardAnimation();
        }

        internal static void PlayFromTo(TimeSpan from, TimeSpan to)
        {
            TimeLine.Stopper.Stop();
            TimeLine.Stopper.Elapsed = from;
            TimeLine.RegisterPlayToTimeSpan(to);
            TimeLine.Stopper.Start();
        }

        public static void Stop()
        {
            TimeLine.PauseStoryboardAnimation(false);
        }

        public static void ChangeCurrentTime(TimeSpan newTimespamp)
        {
            TimeLine.Stopper.Stop();
            TimeLine.Stopper.Elapsed = newTimespamp;
        }

        internal static void MoveForwardPanelTimeSection()
        {
            PanelOffsetTime = new TimeSpan(PanelOffsetTime.Ticks + (PanelWidthTime.Ticks / 2));
            UpdatePanelSectionTimeForAllPanels();
            InitializeGraphicIdentyficator();
        }

        internal static void MoveBackPanelTimeSection()
        {
            if (PanelOffsetTime.Ticks - (PanelWidthTime.Ticks / 2) < 0)
                return;

            PanelOffsetTime = new TimeSpan(PanelOffsetTime.Ticks - (PanelWidthTime.Ticks / 2));
            UpdatePanelSectionTimeForAllPanels();
            InitializeGraphicIdentyficator();
        }

        internal static void UpdatePanelSectionTimeForAllPanels()
        {
            var endTime = PanelOffsetTime + PanelWidthTime;

            PanelGroup.UpdateSectionTime(PanelOffsetTime, endTime);
            PanelElement.UpdateSectionTime(PanelOffsetTime, endTime);
            PanelBeat.UpdateSectionTime(PanelOffsetTime, endTime);
            PanelRoute.UpdateSectionTime(PanelOffsetTime, endTime);
        }

        public static List<string> GetKeysForManualUpdate(Type type)
        {
            List<string> keys = new List<string>();
            keys.AddRange(Placement.GetKeysManualUpdate());
            if (type == typeof(AnimationGroupElement))
            {
                keys.AddRange(Placement.GetKeysManualUpdate());
                keys.AddRange(AnimationGroupElement.GetKeysManualUpdate());
            }
            else if (type == typeof(AnimationElement))
            {
                keys.AddRange(Placement.GetKeysManualUpdate());
                keys.AddRange(AnimationElement.GetKeysManualUpdate());
            }
            else if (type == typeof(AnimationRouteFrame))
            {
                keys.AddRange(Placement.GetKeysManualUpdate());
                keys.AddRange(AnimationRouteFrame.GetKeysManualUpdate());
            }
            else if (type == typeof(Beat))
            {
                // noting to return 
                return new List<string>();
            }

            return keys;
        }

        public static void SelectPanel(string panelName)
        {
            GetPanel(panelName).Selected = true;
        }

        public static void ClearPanelSelection()
        {
            PanelRoute.Selected = false;
            PanelElement.Selected = false;
            PanelGroup.Selected = false;
            PanelBeat.Selected = false;
        } 

        public static TLEPanel GetPanel(string panelName)
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
