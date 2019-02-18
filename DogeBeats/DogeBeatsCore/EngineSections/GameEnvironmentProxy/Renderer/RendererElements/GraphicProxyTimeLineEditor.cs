using DogeBeats.EngineSections.Resources;
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
            StaticHub.TimeLineEditor.Play();
        }

        public void TimeStop()
        {
            StaticHub.TimeLineEditor.Stop();
        }

        public void TimeShift(TimeSpan ts)
        {
            StaticHub.TimeLineEditor.ChangeCurrentTime(ts);
        }

        public void NextPanelSection()
        {
            //StaticHub.TimeLineEditor.MoveForwardPanelTimeSection();
        }

        public void PreviousPanelSection()
        {
            //StaticHub.TimeLineEditor.MoveBackPanelTimeSection();
        }

        public TLEPanel GetPanel(string panelName)
        {
            //return StaticHub.TimeLineEditor.GetPanel(panelName);
            throw new NotImplementedException();
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
            //StaticHub.TimeLineEditor.MoveTimeForPanelElement(elementName, wayPrecentage);
        }

        public void RightMouseClickOnPanelElement(string elementName)
        {
            //StaticHub.TimeLineEditor.RemovePanelElement(elementName);
        }

        public void UpdatePlacement(string graphicName, Placement placement)
        {
            //StaticHub.TimeLineEditor.UpdatePlacement(graphicName, placement);
            //GraphicProxy.UpdatePlacement(graphicName, placement);
        }

        public void UpdateRoutePlacement(string graphicName, Placement placement)
        {
            //StaticHub.TimeLineEditor.UpdateRoutePlacement(graphicName, placement);
            //GraphicProxy.UpdatePlacement(graphicName, placement);
        }

        public void AddAnimationElement(string graphicGroupName)
        {
            //StaticHub.TimeLineEditor.AddNewAnimationElement(graphicGroupName);
        }

        public void AddAnimationGroup(string groupName)
        {
            //StaticHub.TimeLineEditor.AddNewAnimationGroup(groupName);
        }

        public void SaveTimeLine()
        {
            StaticHub.TimeLineEditor.SaveTimeLine();
        }

        public void LoadTimeLine(string timelineName)
        {
            StaticHub.TimeLineEditor.LoadTimeLine(timelineName);
        }

        public List<string> GetAllTimeLineNames()
        {
            return null;// StaticHub.TimeLineCentre.TimeLines.Select(s => s.Key).ToList();
        }

        public List<string> GetAllAnimationGroupElements()
        {
            return StaticHub.TimeLineCentre.GetAllAnimationGroupElements().Select(s => s.Key).ToList();
        }

        public void SetTimeCursorToPrecentage(float precentage)
        {
            StaticHub.TimeLineEditor.SetTimeCursorToPrecentage(precentage);
        }

        public void PlayFromTimeToTime(TimeSpan from, TimeSpan to)
        {
            StaticHub.TimeLineEditor.PlayFromTo(from,to);
        }

        public void UpdateAnimationElementManual(string graphicName, NameValueCollection values)
        {
            //StaticHub.TimeLineEditor.UpdateManual(graphicName, values);
        }

        //public List<string> GetKeysForManualUpdate(Type type)
        //{
        //    return StaticHub.TimeLineEditor.GetKeysForManualUpdate(type);
        //}

        public void PerformManualUpdate(NameValueCollection values)
        {
            //todo
        }
    }
}
