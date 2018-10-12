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

        public static TimeLineEditorPanel GroupPanel { get; set; }

        public static TimeLineEditorPanel ElementPanel { get; set; }
        public static TimeLineEditorPanel BeatPanel { get; set; }

        public static TimeSpan PanelOffsetTime { get; set; } = new TimeSpan();
        public static TimeSpan PanelWidthTime { get; set; } = new TimeSpan(0, 0, 1, 0, 0);

        static TimeLineEditor()
        {

        }

        public static void AttachTimeLineToEditor(TimeLine timeline)
        {
            TimeLine = timeline;

            GroupPanel = new TimeLineEditorPanel();
            GroupPanel.TimeCellWidth = new TimeSpan(0, 0, 0, 1, 0);
            GroupPanel.StartTime = PanelOffsetTime;
            GroupPanel.EndTime = PanelOffsetTime + PanelWidthTime;
            List<TimedGraphicElement> timedElements = TimedGraphicElement.Parse(timeline.AnimationGroupElements);
            GroupPanel.InitialineElements(timedElements);

            BeatPanel = new TimeLineEditorPanel();
            BeatPanel.TimeCellWidth = new TimeSpan(0, 0, 0, 1, 0);
            BeatPanel.StartTime = PanelOffsetTime;
            BeatPanel.EndTime = PanelOffsetTime + PanelWidthTime;
            timedElements = timeline.BeatGuider.Beats;
            BeatPanel.InitialineElements(timedElements);

            InitializeAnimationElementPanel(null);
        }

        private static void InitializeAnimationElementPanel(AnimationGroupElement group)
        {
            ElementPanel = new TimeLineEditorPanel();
            if (group != null)
            {
                var timedElements = TimedGraphicElement.Parse(group.Elements);
                ElementPanel.InitialineElements(timedElements);
            }
            ElementPanel.TimeCellWidth = new TimeSpan(0, 0, 0, 1, 0);
            ElementPanel.StartTime = PanelOffsetTime;
            ElementPanel.EndTime = PanelOffsetTime + PanelWidthTime;
        }

        internal static void MoveTimeForPanelElement(string elementName, float wayPrecentage)
        {
            TimeSpan destenationTime = new TimeSpan(PanelOffsetTime.Ticks + (long)(PanelWidthTime.Ticks * wayPrecentage));
            ElementPanel.MoveElementTime(elementName, destenationTime);
            GroupPanel.MoveElementTime(elementName, destenationTime);
            BeatPanel.MoveElementTime(elementName, destenationTime);
        }

        internal static void UpdateRoutePlacement(string graphicName, Placement placement)
        {
            UpdateRouteGroupPlacement(graphicName, placement);
            UpdateRouteElementPlacement(graphicName, placement);

            TimeLine.RefreshCurrentlyAnimatingElementList();
        }

        internal static void AddAnimationElement(string graphicGroupName, AnimationElement element)
        {
            var groups = TimeLine.AnimationGroupElements.Where(w => w.GroupName == graphicGroupName).ToList();
            foreach (var group in groups)
            {
                group.Elements.Add(element);
            }

            TimeLine.RefreshCurrentlyAnimatingElementList();
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
                    Placement placement = group.GroupInitPlacement;
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

        internal static void AddAnimationGroup(AnimationGroupElement group)
        {
            TimeLine.AnimationGroupElements.Add(group);

            TimeLine.RefreshCurrentlyAnimatingElementList();
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
                groupWithName.GroupInitPlacement = placement;
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

        public static void RegisterBeat()
        {
            TimeLine.BeatGuider.RegisterBeat(TimeLine.Stopper.Elapsed);
        }

        //No intel about succesful removal
        public static void RemovePanelElement(string graphicName)
        {
            ElementPanel.RemoveElement(graphicName);
            GroupPanel.RemoveElement(graphicName);
            BeatPanel.RemoveElement(graphicName);

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

            GroupPanel.UpdateSectionTime(PanelOffsetTime, endTime);
            ElementPanel.UpdateSectionTime(PanelOffsetTime, endTime);
            BeatPanel.UpdateSectionTime(PanelOffsetTime, endTime);
        }
    }
}
