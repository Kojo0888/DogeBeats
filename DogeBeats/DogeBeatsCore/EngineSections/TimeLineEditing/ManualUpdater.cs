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
        public AnimationSingleElement Create(NameValueCollection values)
        {
            AnimationSingleElement element = new AnimationSingleElement();
            if (!string.IsNullOrEmpty(values.Get("ElementName")))
            {
                element.Shape = new AnimationElementShape(values["ElementName"]);
            }
            return element;
        }

        internal void UpdateManual(AnimationSingleElement element, NameValueCollection values)
        {
            if (!string.IsNullOrEmpty(values["Prediction"].ToString()))
                element.Prediction = ManualUpdateBool(values["Prediction"]);
            if (!string.IsNullOrEmpty(values["Shape"].ToString()))
                element.Shape = ManualUpdateShape(values["Shape"]);
        }

        private AnimationElementShape ManualUpdateShape(string v)
        {
            AnimationElementShape shape = new AnimationElementShape(v);
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
