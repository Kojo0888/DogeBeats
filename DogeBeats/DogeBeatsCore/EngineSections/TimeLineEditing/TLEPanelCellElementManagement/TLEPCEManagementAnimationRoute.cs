using DogeBeats.EngineSections.AnimationObjects;
using DogeBeats.EngineSections.Shared;
using DogeBeats.EngineSections.TimeLineEditing;
using DogeBeats.EngineSections.TimeLineEditing.TLEPanels;
using DogeBeats.Modules;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.EngineSections.TimeLineEditing.TLEPanelCellElementManagement
{
    public class TLEPCEManagementAnimationRoute : ITLEPanelCellElementManagement
    {
        public TimeLineEditor ParentTLE { get; set; }

        public TLEPCEManagementAnimationRoute(TimeLineEditor parent)
        {
            ParentTLE = parent;
        }

        public void AddNewElement()
        {
            var time = ParentTLE.PanelHub.TimeIdentyficator.GetTime();
            var frame = new AnimationRouteFrame();
            frame.FrameTime = time;

            var lastAnimationElementPanel = ParentTLE.PanelHub.GetLastAnimationElementPanel();
            if (lastAnimationElementPanel == null)
                throw new NesuException("There is no LastAnimationElementPanel somehow");

            if (lastAnimationElementPanel.SelectedPanelCell == null)
                throw new NesuException("There is no SelectedCell in panel " + lastAnimationElementPanel.PanelName);

            var parentAnimationElement = lastAnimationElementPanel.SelectedPanelCell.ReferenceElement;
            if (parentAnimationElement is IAnimationElement)
            {
                var ianimationElemnt = parentAnimationElement as IAnimationElement;
                ianimationElemnt.Route.Frames.Add(frame);

                ParentTLE.TimeLine.Refresh();
                ParentTLE.PanelHub.InitializePanel(TLEPanelNames.ANIMATION_ROUTE, ianimationElemnt.Route.Frames);
            }
        }

        public void MoveElement()
        {
            var panel = ParentTLE.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ROUTE);
            if (panel == null)
                return;

            var lastAnimationElementPanel = ParentTLE.PanelHub.GetLastAnimationElementPanel();
            if (lastAnimationElementPanel == null)
                throw new NesuException("There is no LastAnimationElementPanel somehow");

            if (lastAnimationElementPanel.SelectedPanelCell == null)
                throw new NesuException("There is no SelectedCell in panel " + lastAnimationElementPanel.PanelName);

            var routeFrame = panel.SelectedPanelCell.ReferenceElement;
            if (routeFrame is AnimationRouteFrame)
            {
                var animationRouteFrame = routeFrame as AnimationRouteFrame;
                var parentAnimationElement = ParentTLE.TimeLine.SearchParentAnimationElement(animationRouteFrame);

                var time = ParentTLE.PanelHub.TimeIdentyficator.GetTime();
                animationRouteFrame.FrameTime = time;

                ParentTLE.TimeLine.Refresh();

                ParentTLE.PanelHub.InitializePanel(TLEPanelNames.ANIMATION_ROUTE, parentAnimationElement.Route.Frames);
            }
        }

        public void RemoveElement()
        {
            var panel = ParentTLE.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ROUTE);
            if (panel == null)
                return;

            var lastAnimationElementPanel = ParentTLE.PanelHub.GetLastAnimationElementPanel();
            if (lastAnimationElementPanel == null)
                throw new NesuException("There is no LastAnimationElementPanel somehow");

            if (lastAnimationElementPanel.SelectedPanelCell == null)
                throw new NesuException("There is no SelectedCell in panel " + lastAnimationElementPanel.PanelName);

            var routeFrame = panel.SelectedPanelCell.ReferenceElement;
            if (routeFrame is AnimationRouteFrame)
            {
                var animationRouteFrame = routeFrame as AnimationRouteFrame;
                var parentAnimationElement = ParentTLE.TimeLine.SearchParentAnimationElement(animationRouteFrame);
                parentAnimationElement.Route.Frames.Remove(animationRouteFrame);

                ParentTLE.TimeLine.Refresh();

                ParentTLE.PanelHub.InitializePanel(TLEPanelNames.ANIMATION_ROUTE, parentAnimationElement.Route.Frames);
            }
        }

        public void UpdateElement(NameValueCollection values)
        {
            var panel = ParentTLE.PanelHub.GetPanel(TLEPanelNames.ANIMATION_ROUTE);
            if (panel == null)
                throw new NesuException("There is no Animation rote panel selected");

            if (panel.SelectedPanelCell == null)
                throw new NesuException("There is no Panel cell selected in route panel");

            //var lastAnimationElementPanel = ParentTLE.PanelHub.GetLastAnimationElementPanel();
            //if (lastAnimationElementPanel == null)
            //    throw new NesuException("There is no LastAnimationElementPanel somehow");

            //if (lastAnimationElementPanel.SelectedPanelCell == null)
            //    throw new NesuException("There is no SelectedCell in panel " + lastAnimationElementPanel.PanelName);


            var routeFrame = panel.SelectedPanelCell.ReferenceElement;
            if (routeFrame is AnimationRouteFrame)
            {
                var animationRouteFrame = routeFrame as AnimationRouteFrame;
                var parentAnimationElement = ParentTLE.TimeLine.SearchParentAnimationElement(animationRouteFrame);

                animationRouteFrame.UpdateManual(values);

                ParentTLE.TimeLine.Refresh();

                ParentTLE.PanelHub.InitializePanel(TLEPanelNames.ANIMATION_ROUTE, parentAnimationElement.Route.Frames);
            }
        }
    }
}
