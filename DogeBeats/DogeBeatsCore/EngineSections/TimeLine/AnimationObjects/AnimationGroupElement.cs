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
        }

        public void FixParentAnimationTime()
        {
            TimeSpan ts = Route.AnimationStartTime;
            foreach (var element in Elements)
            {
                ts += element.GetDurationTime();
            }

            if (Route.CalculateAnimationTime() < ts)
                Route.DuplicateLastFrame(ts);
        }

        public static IEnumerable<string> GetKeysManualUpdate()
        {
            List<string> keys = new List<string>();
            keys.Add("Name");
            keys.Add("Prediction");
            return keys;
        }
    }
}
