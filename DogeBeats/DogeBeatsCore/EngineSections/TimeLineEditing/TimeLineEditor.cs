using DogeBeats.EngineSections.Resources;
using DogeBeats.EngineSections.TimeLineEditing.TLEPanels;
using DogeBeats.Model;
using DogeBeats.Modules.TimeLines;
using DogeBeats.Renderer;
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

        public TimeLineEditorPanelHub PanelHub { get; set; }

        public void AttachTimeLineToEditor(TimeLine timeline)
        {
            TimeLine = timeline;
        }

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

        public void RegisterBeat()
        {
            TimeLine.BeatGuider.RegisterBeat(TimeLine.Stopper.Elapsed);
            //PanelBeat.RefreshPanelCells();
        }

        public void AddNewAnimationElement(string graphicGroupName)
        {
            AnimationSingleElement element = new AnimationSingleElement();
            ITLEPanelCellElement panelElement = PanelHub.PanelGroup.GetCellElementBasedOnGraphicName(graphicGroupName);
            if (panelElement as AnimationGroupElement != null)
            {
                AnimationGroupElement group = panelElement as AnimationGroupElement;
                group.Elements.Add(element);
                PanelHub.InitializeAnimationElementPanel(group);
                TimeLine.Refresh();
            }
            else
                throw new Exception("Nesu... zjebales");

            //PanelHub.Refresh();
        }

        public void AddNewAnimationGroup(string groupName, string parentGroupName = "")
        {
            AnimationGroupElement element = new AnimationGroupElement();
            //TODO this crap
            ITLEPanelCellElement panelElement = PanelHub.PanelGroup.GetCellElementBasedOnGraphicName (parentGroupName);
            if(panelElement == null)
               TimeLine.AnimationElements.Add(element);
            else
            {
                AnimationGroupElement parentGroup = panelElement as AnimationGroupElement;
                parentGroup.Elements.Add(element);
            }

            TLEPanelCell timedElement = TLEPanelCell.Parse(element);
            PanelHub.PanelGroup.Elements.Add(timedElement);

            TimeLine.Refresh();
            //PanelGroup.RefreshPanelCells();
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

        public void RemovePanelElement(string graphicName)
        {
            var elementToRemove = PanelHub.RemovePanelElement(graphicName);
            RemoveTimeLineElement(elementToRemove);
            TimeLine.Refresh();
        }

        public void SetTimeCursorToPrecentage(float precentage)
        {
            var time = PanelHub.SetTimeCursorToPrecentage(precentage);
            bool running = TimeLine.Stopper.IsRunning;
            TimeLine.Stopper.Stop();
            TimeLine.Stopper.Elapsed = time;
            if (running)
                TimeLine.Stopper.Start();
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
    }
}
