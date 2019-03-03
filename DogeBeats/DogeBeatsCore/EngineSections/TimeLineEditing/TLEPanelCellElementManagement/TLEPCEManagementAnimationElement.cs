using DogeBeats.EngineSections.AnimationObjects;
using DogeBeats.EngineSections.Shared;
using DogeBeats.EngineSections.TimeLineEditing.TLEPanels;
using DogeBeats.Modules;
using DogeBeats.Modules.TimeLines;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.EngineSections.TimeLineEditing.TLEPanelCellElementManagement
{
    public class TLEPCEManagementAnimationElement : ITLEPanelCellElementManagement
    {
        public TimeLineEditor ParentTLE { get; set; }

        public TLEPCEManagementAnimationElement(TimeLineEditor parent)
        {
            ParentTLE = parent;
        }

        public void AddNewElement()
        {
            AnimationSingleElement element = new AnimationSingleElement();
            element.SetStartTime(ParentTLE.PanelHub.TimeIdentyficator.SelectedTime);

            var panel = ParentTLE.PanelHub.SelectedPanel;
            if (panel == null)
                throw new Exception("SelectedPanel is null");

            var selectedCell = panel.SelectedPanelCell;
            if (selectedCell == null)
                throw new Exception("SelectedCell is null");

            ITLEPanelCellElement selectedPanelElement = ParentTLE.PanelHub.SelectedPanel.SelectedPanelCell.ReferenceElement;

            IAnimationElement animElem = selectedPanelElement as IAnimationElement;
            if (animElem == null)
                throw new Exception("AddNewElement: Reference element is not an AnimationElement");

            var parent = ParentTLE.TimeLine.SearchParentAnimationElement(animElem);
            if(parent != null)
            {
                var group = parent as AnimationGroupElement;
                if (group == null)
                    throw new Exception("AddNewElement: Parent is not an GroupElement");
                group.Elements.Add(element);
                ParentTLE.TimeLine.Refresh();
                ParentTLE.PanelHub.InitializePanel(ParentTLE.PanelHub.SelectedPanel.PanelName, group.Elements);
            }
            else
            {
                ParentTLE.TimeLine.AnimationElements.Add(element);
                ParentTLE.TimeLine.Refresh();
                ParentTLE.PanelHub.InitializePanel(ParentTLE.PanelHub.SelectedPanel.PanelName, ParentTLE.TimeLine.AnimationElements);
            }
        }

        public void AddNewChildElement()
        {
            AnimationSingleElement element = new AnimationSingleElement();
            element.SetStartTime(ParentTLE.PanelHub.TimeIdentyficator.SelectedTime);

            var panel = ParentTLE.PanelHub.SelectedPanel;
            if (panel == null)
                throw new Exception("SelectedPanel is null");

            var selectedCell = panel.SelectedPanelCell;
            if (selectedCell == null)
                throw new Exception("SelectedCell is null");

            ITLEPanelCellElement panelElement = ParentTLE.PanelHub.SelectedPanel.SelectedPanelCell.ReferenceElement;
            if (panelElement as AnimationGroupElement != null)
            {
                AnimationGroupElement group = panelElement as AnimationGroupElement;
                group.Elements.Add(element);
                ParentTLE.PanelHub.InitializeNewAnimationPanel(group.Elements.ToList());
            }
            else if (panelElement as AnimationSingleElement != null)
            {
                AnimationSingleElement singleAnimationElement = panelElement as AnimationSingleElement;
                IAnimationElement parentParentElement = ParentTLE.TimeLine.SearchParentAnimationElement(singleAnimationElement);

                if (parentParentElement is AnimationGroupElement)
                {
                    var par = parentParentElement as AnimationGroupElement;
                    if (par != null)
                    {
                        var convertedGroup = par.ConvertToGroup(singleAnimationElement);
                        convertedGroup.Elements.Add(element);

                        ParentTLE.PanelHub.InitializeNewAnimationPanel(convertedGroup.Elements.ToList());
                    }
                }
                else if (parentParentElement == null)// no parent, timeline
                {
                    var group = ParentTLE.TimeLine.ConvertToGroup(singleAnimationElement);
                    group.Elements.Add(element);

                    ParentTLE.PanelHub.InitializeNewAnimationPanel(group.Elements.ToList());
                }
            }
            else //null no group, directly attached to TimeLine. This shouldn't be used, although just in case.
            {
                ParentTLE.TimeLine.AnimationElements.Add(element);
                ParentTLE.PanelHub.InitializeNewAnimationPanel(ParentTLE.TimeLine.GetAnimationSingleElementFirstLayer());
                //throw new Exception("Nesu: TimeLineEditor - Parent animation element is null");
            }

            ParentTLE.TimeLine.Refresh();
        }

        public void RemoveElement()
        {
            var timeSpan = ParentTLE.PanelHub.TimeIdentyficator.SelectedTime;

            if (timeSpan != ParentTLE.TimeLine.Stopper.Elapsed)
                throw new Exception("Nesu: Time Spans are not matched");

            var panel = ParentTLE.PanelHub.SelectedPanel;
            if (panel == null)
                throw new Exception("SelectedPanel is null");

            var selectedCell = panel.SelectedPanelCell;
            if (selectedCell == null)
                throw new Exception("SelectedCell is null");

            var element = panel.SelectedPanelCell.ReferenceElement;
            if (element is IAnimationElement)
            {
                var animationElementI = element as IAnimationElement;
                var parentElement = ParentTLE.TimeLine.SearchParentAnimationElement(animationElementI);
                if (parentElement is AnimationGroupElement)
                {
                    var par = parentElement as AnimationGroupElement;
                    par.Elements.Remove(animationElementI);
                    ParentTLE.TimeLine.Refresh();
                    ParentTLE.PanelHub.InitializePanel(panel.PanelName, ParentTLE.TimeLine.BeatGuider.GetTLECellElements());
                }
                else
                {   
                    ParentTLE.TimeLine.AnimationElements.Remove(element as IAnimationElement);
                    ParentTLE.TimeLine.Refresh();
                    ParentTLE.PanelHub.InitializePanel(panel.PanelName, ParentTLE.TimeLine.AnimationElements);
                }
            }

            //var lastIndex = ParentTLE.PanelHub.GetLastPanelAnimationElementIndex();

            //PanelHub.InitializePanel(TLEPanelNames.BEAT, TimeLine.BeatGuider.GetTLECellElements());
        }

        public void MoveElement()
        {
            var timeSpan = ParentTLE.PanelHub.TimeIdentyficator.SelectedTime;

            if (timeSpan != ParentTLE.TimeLine.Stopper.Elapsed)
                throw new NesuException("TLE: MoveAnimationElement: Time Spans are not matched");

            var panel = ParentTLE.PanelHub.SelectedPanel;
            if (panel == null)
                throw new Exception("SelectedPanel is null");

            var selectedCell = panel.SelectedPanelCell;
            if (selectedCell == null)
                throw new Exception("SelectedCell is null");

            var selectedElement = ParentTLE.PanelHub.SelectedPanel.SelectedPanelCell.ReferenceElement;
            selectedElement.SetStartTime(timeSpan);

            ParentTLE.TimeLine.Refresh();

            var lastIndex = ParentTLE.PanelHub.GetLastPanelAnimationElementIndex();
            //ParentTLE.PanelHub.InitializePanel(panel.PanelName, );
        }

        public void UpdateElement(NameValueCollection values)
        {
            var panel = ParentTLE.PanelHub.SelectedPanel;
            if (panel == null)
                throw new Exception("SelectedPanel is null");

            var selectedCell = panel.SelectedPanelCell;
            if (selectedCell == null)
                throw new Exception("SelectedCell is null");

            ITLEPanelCellElement panelElement = ParentTLE.PanelHub.SelectedPanel.SelectedPanelCell.ReferenceElement;
            if (panelElement is AnimationSingleElement)
            {
                AnimationSingleElement elem = panelElement as AnimationSingleElement;
                elem.UpdateManual(values);
            }
            else if (panelElement is AnimationGroupElement)
            {
                AnimationGroupElement elem = panelElement as AnimationGroupElement;
                elem.UpdateManual(values);
            }

            var lastIndex = ParentTLE.PanelHub.GetLastPanelAnimationElementIndex();
            ParentTLE.PanelHub.InitializePanel(TLEPanelNames.ANIMATION_ELEMENT_PREFIX + lastIndex, ParentTLE.TimeLine.BeatGuider.GetTLECellElements());
        }
    }
}
