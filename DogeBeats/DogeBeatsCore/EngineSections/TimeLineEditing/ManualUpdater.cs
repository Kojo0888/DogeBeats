using DogeBeats.Modules.TimeLines.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeats.EngineSections.TimeLineEditing
{
    public class ManualUpdater
    {
        public AnimationElement Create(NameValueCollection values)
        {
            AnimationElement element = new AnimationElement();
            if (!string.IsNullOrEmpty(values.Get("ElementName")))
            {
                element.Shape = new TimeLineShape(values["ElementName"]);
            }
            return element;
        }

        internal void UpdateManual(AnimationElement element, NameValueCollection values)
        {
            if (!string.IsNullOrEmpty(values["Prediction"].ToString()))
                element.Prediction = ManualUpdateBool(values["Prediction"]);
            if (!string.IsNullOrEmpty(values["Shape"].ToString()))
                element.Shape = ManualUpdateShape(values["Shape"]);
        }

        private TimeLineShape ManualUpdateShape(string v)
        {
            TimeLineShape shape = new TimeLineShape(v);
            return shape;
        }

        private bool ManualUpdateBool(string v)
        {
            bool temp;
            if (bool.TryParse(v, out temp))
            {
                return temp;
            }
            return false;
        }

        internal IEnumerable<string> GetKeysManualUpdate()
        {
            List<string> keys = new List<string>();
            keys.Add("Prediction");
            keys.Add("Shape");
            return keys;
        }

        //internal IEnumerable<string> GetKeysManualUpdate(Type type)
        //{
        //    if(type == typeof(AnimationElement))
        //    {
        //        List<string> keys = new List<string>();
        //        keys.Add("Prediction");
        //        keys.Add("Shape");
        //        return keys;
        //    }
        //}
    }
}
