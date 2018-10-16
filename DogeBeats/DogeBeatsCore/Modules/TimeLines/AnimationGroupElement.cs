using DogeBeats.Model;
using DogeBeats.Model.Route;
using DogeBeats.Modules.TimeLines;
using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testowy.Model
{
    public class AnimationGroupElement : ITLEPanelElement
    {
        public List<AnimationElement> Elements { get; set; }
        public AnimationGroupRoute GroupRoute { get; set; }
        public Placement InitPlacement { get; set; }
        public string GroupName { get; set; }

        public AnimationGroupElement()
        {

        }

        //public AnimationGroupElement(string groupName)
        //{
        //    GroupName = groupName;
        //}

        internal void Update(TimeSpan currentStopperTime)
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

        internal void Render()
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
                element.GroupName = values[groupName];

            return element;
        }

        internal static void UpdateManual(AnimationGroupElement group, NameValueCollection values)
        {
            if (!string.IsNullOrEmpty(values["GroupName"].ToString()))
                group.GroupName = ManualUpdateString(values["GroupName"]);
        }

        private static string ManualUpdateString(string v)
        {
            return v;
        }

        internal static IEnumerable<string> GetKeysManualUpdate()
        {
            List<string> keys = new List<string>();
            keys.Add("GroupName");
            return keys;
        }
    }
}
