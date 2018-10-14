using DogeBeats.Model;
using DogeBeats.Modules.TimeLines;
using DogeBeats.Renderer;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.Modules.Renderer.RendererModules
{
    public class GraphicProxyTimeLineEditor
    {
        public void TimePlay()
        {
            TimeLineEditor.Play();
        }

        public void TimeStop()
        {
            TimeLineEditor.Stop();
        }

        public void TimeShift(TimeSpan ts)
        {
            TimeLineEditor.ChangeCurrentTime(ts);
        }

        public void NextPanelSection()
        {
            TimeLineEditor.ShowNextPanelSection();
        }

        public void PreviousPanelSection()
        {
            TimeLineEditor.ShowPreviousPanelSection();
        }

        public TLEPanel GetPanel(string panelName)
        {
            switch (panelName)
            {
                case "ElementPanel":
                    return TimeLineEditor.PanelElement;
                case "GroupPanel":
                    return TimeLineEditor.PanelGroup;
                case "BeatPanel":
                    return TimeLineEditor.PanelBeat;
            }

            return null;
        }

        public void LeftMouseClickOnPanelElement(string elementName)
        {
            //nothing to do
        }

        public void LeftMouseClickOnPanel(string elementName)
        {
            //todo
        }

        public void LeftMouseOnDropClickOnPanelElement(string elementName, float wayPrecentage)
        {
            TimeLineEditor.MoveTimeForPanelElement(elementName, wayPrecentage);
        }

        public void RightMouseClickOnPanelElement(string elementName)
        {
            TimeLineEditor.RemovePanelElement(elementName);
        }

        public void UpdatePlacement(string graphicName, Placement placement)
        {
            TimeLineEditor.UpdatePlacement(graphicName, placement);
            GraphicProxy.UpdatePlacement(graphicName, placement);
        }

        public void UpdateRoutePlacement(string graphicName, Placement placement)
        {
            TimeLineEditor.UpdateRoutePlacement(graphicName, placement);
            GraphicProxy.UpdatePlacement(graphicName, placement);
        }

        public void AddAnimationElement(string graphicGroupName)
        {
            TimeLineEditor.AddNewAnimationElement(graphicGroupName);
        }

        public void AddAnimationGroup(string groupName)
        {
            TimeLineEditor.AddNewAnimationGroup(groupName);
        }

        public void SaveTimeLine()
        {
            TimeLineEditor.SaveTimeLine();
        }

        public void LoadTimeLine(string timelineName)
        {
            TimeLineEditor.LoadTimeLine(timelineName);
        }

        public List<string> GetAllTimeLineNames()
        {
            return CenterTimeLine.TimeLines.Select(s => s.TimeLineName).ToList();
        }

        public List<string> GetAllAnimationGroupElements()
        {
            return CenterTimeLine.GetAllAnimationGroupElements().Select(s => s.GroupName).ToList();
        }

        public void SetTimeCursorToPrecentage(float precentage)
        {
            TimeLineEditor.SetTimeCursorToPrecentage(precentage);
        }

        public void PlayFromTimeToTime(TimeSpan from, TimeSpan to)
        {
            TimeLineEditor.PlayFromTo(from,to);
        }

        public void UpdateAnimationElementManual(string graphicName, EditAnimationElementType type,  NameValueCollection values)
        {
            TimeLineEditor.UpdateManual(graphicName, type, values);
        }

        public List<string> GetKeysForManualUpdate(Type type)
        {
            return TimeLineEditor.GetKeysForManualUpdate(type);
        }

        public void PerformManualUpdate(NameValueCollection values)
        {
            //todo
        }
    }
}
