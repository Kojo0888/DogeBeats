using DogeBeats.EngineSections.AnimationObjects;
using DogeBeats.EngineSections.Resources;
using DogeBeats.EngineSections.Shared;
using DogeBeats.Model;
using DogeBeats.Modules.TimeLines;
using DogeBeats.Modules.TimeLines.Shapes;
using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testowy.Model
{
    public class AnimationGroupElement : ITLEPanelCellElement, INamedElement, IAnimationElement
    {
        public List<IAnimationElement> Elements { get; set; }

        public string Name { get; set; }

        public AnimationRoute Route { get; set; }

        public bool Prediction { get; set; }

        public Placement Placement { get; set; }

        public AnimationGroupElement()
        {
            Placement = new Placement();
            Route = new AnimationRoute();
            Elements = new List<IAnimationElement>();
        }

        public AnimationGroupElement(string groupName) : this()
        {
            Name = groupName;
        }

        public void Render()
        {
            foreach (var element in Elements)
            {
                element.Render();
            }
        }

        public void Update(TimeSpan currentStopperTime, Placement parentPlacement)
        {
            TimeSpan elementTime = currentStopperTime.Subtract(Route.AnimationStartTime);
            //TODO Verify this timeSpan. Probably some if sstatements needed

            Placement = Route.CalculatePlacement(currentStopperTime);

            foreach (var element in Elements)
            {
                element.Update(elementTime, Placement);
            }
        }

        public TimeSpan GetDurationTime()
        {
            return Route.CalculateAnimationTime();
        }

        public TimeSpan GetStartTime()
        {
            return Route.AnimationStartTime;
        }

        public void SetStartTime(TimeSpan timeSpan)
        {
            Route.AnimationStartTime = timeSpan;
        }

        public List<AnimationGroupElement> GetAnimationGroupElements()
        {
            List<AnimationGroupElement> toReturn = new List<AnimationGroupElement>();
            if (Elements == null)
                return toReturn;

            foreach (var element in Elements)
            {
                if(element is AnimationGroupElement)
                {
                    var group = element as AnimationGroupElement;
                    toReturn.Add(group);
                    toReturn.AddRange(group.GetAnimationGroupElements());
                }
            }

            return toReturn;
        }

        public void UpdateManual(NameValueCollection values)
        {
            StaticHub.AnimationGroupCentre.RenameElement(this, values["Name"], Name);
            Name = ManualUpdaterParser.Parse(values["Name"], Name);

            Prediction = ManualUpdaterParser.Parse(values["Prediction"], Prediction);
            Placement.UpdateManual(values);
        }

        public void FixParentAnimationTime()
        {
            //TimeSpan ts = Route.AnimationStartTime;
            //foreach (var element in Elements)
            //{
            //    ts += element.GetDurationTime();
            //}

            //if (Route.CalculateAnimationTime() < ts)
            //    Route.DuplicateLastFrame(ts);
        }

        public static IEnumerable<string> GetKeysManualUpdate()
        {
            List<string> keys = new List<string>();
            keys.Add("Name");
            keys.Add("Prediction");
            return keys;
        }

        public static AnimationGroupElement Parse(AnimationSingleElement singleAnimationElement)
        {
            var convertedGroup = new AnimationGroupElement();

            convertedGroup.Name = singleAnimationElement.Name;
            convertedGroup.Placement = singleAnimationElement.Placement;
            convertedGroup.Prediction = singleAnimationElement.Prediction;
            convertedGroup.Route = singleAnimationElement.Route;
            convertedGroup.Elements = new List<IAnimationElement>();

            return convertedGroup;
        }

        public AnimationGroupElement ConvertToGroup(AnimationSingleElement singleAnimationElement)
        {
            Elements.Remove(singleAnimationElement);

            AnimationGroupElement convertedGroup = Parse(singleAnimationElement);
            Elements.Add(convertedGroup);

            return convertedGroup;
        }

        public AnimationGroupElement SearchParentAnimationElement(IAnimationElement singleAnimationElement)
        {
            foreach (var element in Elements)
            {
                if (Elements.Contains(singleAnimationElement))
                    return this;

                if (element is AnimationGroupElement)
                {
                    var group = element as AnimationGroupElement;

                    AnimationGroupElement result = group.SearchParentAnimationElement(singleAnimationElement);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        public IAnimationElement SearchParentAnimationElement(AnimationRouteFrame routeFrame)
        {
            foreach (var frame in Route.Frames)
            {
                if (frame == routeFrame)
                    return this;
            }

            foreach (var element in Elements)
            {
                if (element is AnimationSingleElement)
                {
                    var single = element as AnimationSingleElement;
                    var singleResult = single.SearchParentAnimationElement(routeFrame);
                    if (singleResult != null)
                        return singleResult;
                }
                if (element is AnimationGroupElement)
                {
                    var group = element as AnimationGroupElement;
                    var groupResult = group.SearchParentAnimationElement(routeFrame);
                    if (groupResult != null)
                        return groupResult;
                }
            }

            return null;
        }

        public void SetDefaultData()
        {
            Placement.X = StaticHub.EnvironmentVariables.MainWindowWidth / 2;
            Placement.Y = StaticHub.EnvironmentVariables.MainWindowHeight / 2;
        }

        public List<string> GetKeysUpdateManual()
        {
            return GetKeysManualUpdate().ToList();
        }
    }
}
