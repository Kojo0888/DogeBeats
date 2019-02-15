using DogeBeats.EngineSections.Resources;
using DogeBeats.EngineSections.Shared;
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

        #region Beat Management

        public void AddNewBeat()
        {
            var timeSpan = PanelHub.TimeIdentyficator.SelectedTime;

            if (timeSpan != TimeLine.Stopper.Elapsed)
                throw new Exception("Nesu: Time Spans are not matched");

            TimeLine.BeatGuider.RegisterBeat(timeSpan);

            TimeLine.Refresh();

            PanelHub.InitializeBeatPanel(TimeLine.BeatGuider.GetTLECellElements());
        }

        public void UpdateBeat(NameValueCollection values)
        {
            return;
        }

        public void RemoveBeat()
        {
            var timeSpan = PanelHub.TimeIdentyficator.SelectedTime;

            if (timeSpan != TimeLine.Stopper.Elapsed)
                throw new Exception("Nesu: Time Spans are not matched");

            TimeLine.BeatGuider.RemoveBeat(timeSpan);

            TimeLine.Refresh();

            PanelHub.InitializeBeatPanel(TimeLine.BeatGuider.GetTLECellElements());
        }

        public void MoveBeat(TimeSpan from)
        {
            var timeSpan = PanelHub.TimeIdentyficator.SelectedTime;

            if (timeSpan != TimeLine.Stopper.Elapsed)
                throw new Exception("Nesu: Time Spans are not matched");

            TimeLine.BeatGuider.RemoveBeat(from);
            TimeLine.BeatGuider.RegisterBeat(timeSpan);

            TimeLine.Refresh();

            PanelHub.InitializeBeatPanel(TimeLine.BeatGuider.GetTLECellElements());
        }

        #endregion

        #region AnimationElement Management

        public void AddNewAnimationElement()
        {
            AnimationSingleElement element = new AnimationSingleElement();
            element.SetStartTime(PanelHub.TimeIdentyficator.SelectedTime);

            ITLEPanelCellElement panelElement = PanelHub.GetLastGroupPanel().SelectedPanelCell.AnimationElement;
            if (panelElement as AnimationGroupElement != null)
            {
                AnimationGroupElement group = panelElement as AnimationGroupElement;
                group.Elements.Add(element);
                PanelHub.InitializeAnimationElementPanel(group.Elements.OfType<AnimationSingleElement>().ToList());
            }
            else if (panelElement as AnimationSingleElement != null)
            {
                throw new Exception("Nesu: TimeLineEditor - Parent animation element is single type");
                //AnimationSingleElement singleAnimationElement = panelElement as AnimationSingleElement;
                //var parent = PanelHub.GetLastGroupPanel
            }
            else
            {
                TimeLine.AnimationElements.Add(element);
                PanelHub.InitializeAnimationElementPanel(TimeLine.GetAnimationSingleElementFirstLayer());
                //throw new Exception("Nesu: TimeLineEditor - Parent animation element is null");
            }

            TimeLine.Refresh();
        }

        #endregion

        #region AnimationGroup Management

        public void AddNewAnimationGroup()
        {
            AnimationGroupElement element = new AnimationGroupElement();

            ITLEPanelCellElement panelElement = PanelHub.GetLastGroupPanel().SelectedPanelCell?.AnimationElement;

            if (panelElement == null)
               TimeLine.AnimationElements.Add(element);
            else if(panelElement is AnimationGroupElement)
            {
                AnimationGroupElement parentGroup = panelElement as AnimationGroupElement;
                parentGroup.Elements.Add(element);
                PanelHub.InitializeSpecificGroupPanel(PanelHub.GetLastGroupPanel());
            }
            else
                throw new NesuException("Nesu: AddNewAnimationGroup - panelElement not null & not group");

            TimeLine.Refresh();
        }

        public void MoveAnimationGroup(string graphicalName = "")
        {
            ITLEPanelCellElement panelElement = PanelHub.GetLastGroupPanel().SelectedPanelCell?.AnimationElement;
            var time = PanelHub.TimeIdentyficator.SelectedTime;

            if (panelElement == null)
                throw new NesuException("TLE: MoveAnimationGroup - panelElement is null");
            else if (panelElement is AnimationGroupElement)
            {
                AnimationGroupElement parentGroup = panelElement as AnimationGroupElement;
                parentGroup.SetStartTime(time);
            }
            else
                throw new NesuException("TLE: MoveAnimationGroup - panelElement not null & not group");

            TimeLine.Refresh();
            

        }

        #endregion

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
            var elementToRemove = PanelHub.RemovePanelsElement(graphicName);
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
