using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testowy.Model;

namespace DogeBeatsTests
{
    public static class MockObjects
    {
        public static AnimationSingleElement GetAnimationElement()
        {
            var values = new System.Collections.Specialized.NameValueCollection();
            values.Add("Prediction", "False");
            values.Add("ShapeTypeName", "IdkYet");
            values.Add("Name", "TestName");
            var aElem = AnimationSingleElement.Create(values);
            return aElem;
        }

        public static AnimationSingleElement GetAnimationElement2()
        {
            var values = new System.Collections.Specialized.NameValueCollection();
            values.Add("Prediction", "False");
            values.Add("ShapeTypeName", "IdkYet32");
            var aElem = AnimationSingleElement.Create(values);
            return aElem;
        }

        public static TimeLine GetTimeLine()
        {
            TimeLine timeLine = new TimeLine();
            timeLine.AnimationGroupElementsAll = new List<AnimationGroupElement>() { new AnimationGroupElement("TestGroup1") };
            timeLine.Name = "TEstLimeLint1";
            return timeLine;
        }
    }
}
