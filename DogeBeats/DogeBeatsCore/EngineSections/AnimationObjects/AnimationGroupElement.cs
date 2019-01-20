using DogeBeats.EngineSections.AnimationObjects;
using DogeBeats.EngineSections.Shared;
using DogeBeats.Model;
using DogeBeats.Model.Route;
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
        public AnimationGroupRoute GroupRoute { get; set; }
        public Placement InitPlacement { get; set; }
        public string Name { get; set; }

        public AnimationRoute Route { get; set; }

        public bool Prediction { get; set; }

        public AnimationGroupElement()
        {

        }

        public AnimationGroupElement(string groupName)
        {
            Name = groupName;
        }

        public void Update(TimeSpan currentStopperTime)
        {
            TimeSpan elementTime = GroupRoute.AnimationStartTime.Subtract(currentStopperTime);
            UpdateElementPlacements(elementTime);
        }

        private void UpdateElementPlacements(TimeSpan currentStopperTime)
        {
            var groupPlacement = GroupRoute.CalculatePlacement(currentStopperTime, InitPlacement);
            foreach (var element in Elements)
            {
                TimeSpan elementTime = GroupRoute.AnimationStartTime.Subtract(currentStopperTime);
                element.Update(elementTime, groupPlacement);
            }
        }

        public void Render()
        {
            foreach (var element in Elements)
            {
                element.Render();
            }
        }

        public static AnimationGroupElement Create(NameValueCollection values)
        {
            AnimationGroupElement element = new AnimationGroupElement();

            string groupName = "ElementGroupName";

            if (!string.IsNullOrEmpty(values.Get(groupName)))
                element.Name = values[groupName];

            return element;
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

        public void Update(TimeSpan currentStopperTimeRaw, Placement groupPlacement)
        {
            throw new NotImplementedException();
        }
    }
}
