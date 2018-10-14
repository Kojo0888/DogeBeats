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
using Testowy.Model.GraphicElements;

namespace DogeBeats.Modules
{
    public static class TimeLineEditor
    {
        public static TimeLine TimeLine { get; set; }

        public static AnimationGroupElement SelectedGroup { get; set; }
        public static AnimationElement SelectedElement { get; set; }

        public static TLEPanel PanelGroup { get; set; }

        public static TLEPanel PanelElement { get; set; }
        public static TLEPanel PanelBeat { get; set; }

        public static TimeSpan PanelOffsetTime { get; set; } = new TimeSpan();
        public static TimeSpan PanelWidthTime { get; set; } = new TimeSpan(0, 0, 1, 0, 0);

        static TimeLineEditor()
        {

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

            PanelBeat = new TLEPanel();
            PanelBeat.TimeCellWidth = new TimeSpan(0, 0, 0, 1, 0);
            PanelBeat.StartTime = PanelOffsetTime;
            PanelBeat.EndTime = PanelOffsetTime + PanelWidthTime;
            timedElements = timeline.BeatGuider.Beats;
            PanelBeat.InitialineElements(timedElements);

            InitializeAnimationElementPanel(null);
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
        }

        internal static void MoveTimeForPanelElement(string elementName, float wayPrecentage)
        {
            TimeSpan destenationTime = new TimeSpan(PanelOffsetTime.Ticks + (long)(PanelWidthTime.Ticks * wayPrecentage));
            PanelElement.MovePanelCellTime(elementName, destenationTime);
            PanelGroup.MovePanelCellTime(elementName, destenationTime);
            PanelBeat.MovePanelCellTime(elementName, destenationTime);
        }

        internal static void UpdateRoutePlacement(string graphicName, Placement placement)
        {
            UpdateRouteGroupPlacement(graphicName, placement);
            UpdateRouteElementPlacement(graphicName, placement);

            TimeLine.RefreshCurrentlyAnimatingElementList();
        }

        internal static void AddNewAnimationElement(string graphicGroupName)
        {
            AnimationElement element = new AnimationElement();
            var groups = TimeLine.AnimationGroupElements.Where(w => w.GroupName == graphicGroupName).ToList();
            foreach (var group in groups)
            {
                group.Elements.Add(element);
            }

            TimeLine.RefreshCurrentlyAnimatingElementList();

            PanelElement.RefreshPanelCells();
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

        internal static void UpdateManual(string graphicName, EditAnimationElementType type, NameValueCollection values)
        {
            if(type == EditAnimationElementType.Group)
            {
                AnimationGroupElement group = TimeLine.SearchForAnimationGroupElement(graphicName);
                if(group != null)
                {
                    Placement placement = group.InitPlacement;
                    Placement.Update(placement, values);
                }
            }
            else if(type == EditAnimationElementType.Element)
            {
                AnimationElement element = TimeLine.SearchForAnimationElement(graphicName);
                if (element != null)
                {
                    Placement placement = element.InitPlacement;
                    Placement.Update(placement, values);
                }
            }
            else if (type == EditAnimationElementType.Route)
            {
                AnimationElement element = TimeLine.SearchForAnimationElement(graphicName);
                if (element != null)
                {
                    Placement placement = element.InitPlacement;
                    Placement.Update(placement, values);
                }
            }
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

        internal static void AddNewAnimationGroup(string groupName)
        {
            AnimationGroupElement group = new AnimationGroupElement(groupName);
            TimeLine.AnimationGroupElements.Add(group);
            TimeLine.RefreshCurrentlyAnimatingElementList();

            PanelGroup.RefreshPanelCells();
        }

        private static void RefreshAllPanelCells()
        {
            PanelGroup.RefreshPanelCells();
            PanelElement.RefreshPanelCells();
            PanelBeat.RefreshPanelCells();
        }

        private static void UpdateRouteElementPlacement(string graphicName, Placement placement)
        {
            var groupsWithName = TimeLine.AnimationGroupElements.Where(w => w.GraphicName == graphicName).ToList();
            foreach (var groupWithName in groupsWithName)
            {
                AnimationRouteFrameSlider slider = groupWithName.GroupRoute.GetFrameSlider(TimeLine.Stopper.Elapsed);

                slider.CurrentFrame.Placement = placement;
            }

            TimeLine.RefreshCurrentlyAnimatingElementList();
        }

        private static void UpdateRouteGroupPlacement(string graphicName, Placement placement)
        {
            var groupsWithName = TimeLine.AnimationGroupElements;
            foreach (var groupWithName in groupsWithName)
            {
                foreach (var element in groupWithName.Elements.Where(w => w.GraphicName == graphicName).ToList())
                {
                    AnimationRouteFrameSlider slider = element.Route.GetFrameSlider(TimeLine.Stopper.Elapsed);
                    slider.CurrentFrame.Placement = placement;
                }
            }

            TimeLine.RefreshCurrentlyAnimatingElementList();
        }

        internal static void UpdatePlacement(string graphicName, Placement placement)
        {
            UpdateGroupPlacement(graphicName, placement);
            UpdateElementPlacement(graphicName, placement);

            TimeLine.RefreshCurrentlyAnimatingElementList();
        }

        public static void UpdateGroupPlacement(string graphicName, Placement placement)
        {
            var groupsWithName = TimeLine.AnimationGroupElements.Where(w => w.GraphicName == graphicName).ToList();
            foreach (var groupWithName in groupsWithName)
            {
                groupWithName.InitPlacement = placement;
            }
        }

        public static void UpdateElementPlacement(string graphicName, Placement placement)
        {
            var groupsWithName = TimeLine.AnimationGroupElements;
            foreach (var groupWithName in groupsWithName)
            {
                foreach (var element in groupWithName.Elements.Where(w => w.GraphicName == graphicName).ToList())
                {
                    element.InitPlacement = placement;
                }
            }
        }

        public static void SelectAnimationGroup(string name)
        {
            var group = TimeLine.AnimationGroupElements.FirstOrDefault(f => f.GraphicName == name);
            if (group != null)
            {
                InitializeAnimationElementPanel(group);
            }
        }

        public static void AddNewBeat()
        {
            TimeLine.BeatGuider.RegisterBeat(TimeLine.Stopper.Elapsed);
        }

        //No intel about succesful removal
        public static void RemovePanelElement(string graphicName)
        {
            PanelElement.RemovePanelCell(graphicName);
            PanelGroup.RemovePanelCell(graphicName);
            PanelBeat.RemovePanelCell(graphicName);

            TimeLine.RefreshCurrentlyAnimatingElementList();
        }

        //will it be even used?
        public static List<AnimationGroupElement> GetAnimationGroupElementsFromTimePoint(TimeSpan timestamp)
        {
            List<AnimationGroupElement> groups = new List<AnimationGroupElement>();

            foreach (var animationGroup in TimeLine.AnimationGroupElements)
            {
                var slider = animationGroup.GroupRoute.GetFrameSlider(timestamp);
                if (slider.NextFrame != null && slider.PreviousFrame != null)
                    groups.Add(animationGroup);
            }

            return groups;
        }

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

        internal static void ShowNextPanelSection()
        {
            PanelOffsetTime = new TimeSpan(PanelOffsetTime.Ticks + (PanelWidthTime.Ticks/2));
            UpdatePanelSectionTimeForAllPanels();
        }

        internal static void ShowPreviousPanelSection()
        {
            if (PanelOffsetTime.Ticks - (PanelWidthTime.Ticks / 2) < 0)
                return;

            PanelOffsetTime = new TimeSpan(PanelOffsetTime.Ticks - (PanelWidthTime.Ticks / 2));
            UpdatePanelSectionTimeForAllPanels();
        }

        internal static void UpdatePanelSectionTimeForAllPanels()
        {
            var endTime = PanelOffsetTime + PanelWidthTime;

            PanelGroup.UpdateSectionTime(PanelOffsetTime, endTime);
            PanelElement.UpdateSectionTime(PanelOffsetTime, endTime);
            PanelBeat.UpdateSectionTime(PanelOffsetTime, endTime);
        }

        public static List<string> GetKeysForManualUpdate(Type type)
        {
            List<string> keys = new List<string>();
            keys.AddRange(Placement.GetKeysForUpdate());
            if(type == typeof(AnimationGroupElement))
            {
                keys.Add("GroupName");
            }
            else if (type == typeof(AnimationElement))
            {
                keys.Add("Prediction");
                keys.Add("Shape");
            }
            else if(type == typeof(AnimationRouteFrame))
            {
                keys.Add("RunningTime");
                keys.Add("Ease");
            }

            return keys;
        }
    }
}
