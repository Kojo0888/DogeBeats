using DogeBeats.EngineSections.AnimationObjects;
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
    public class AnimationGroupElement : ITLEPanelElement, INamedElement, IAnimationElement
    {
        public List<AnimationSingleElement> Elements { get; set; }

        public string Name { get; set; }

        public AnimationRoute Route { get; set; }

        public bool Prediction { get; set; }

        public Placement Placement { get; set; }

        public AnimationGroupElement()
        {

        }

        public AnimationGroupElement(string groupName)
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
            TimeSpan elementTime = Route.AnimationStartTime.Subtract(currentStopperTime);
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

        public void UpdateManual(NameValueCollection values)
        {
            if (!string.IsNullOrEmpty(values["GroupName"].ToString()))
                Name = values["GroupName"];
        }

        public static IEnumerable<string> GetKeysManualUpdate()
        {
            List<string> keys = new List<string>();
            keys.Add("GroupName");
            return keys;
        }
    }
}
