using Testowy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogeBeatsTests;
using Xunit;

namespace Testowy.Model.Tests
{
    public class AnimationElementTests
    {
        public AnimationSingleElement aElem { get; set; }

        public AnimationElementTests()
        {
            aElem = new AnimationSingleElement();
        }

        [Fact]
        public void AnimationElementTest()
        {
            aElem = new AnimationSingleElement();
        }

        [Fact]
        public void GetKeysManualUpdateTest()
        {
            var keys = AnimationSingleElement.GetKeysManualUpdate();
            if (keys == null || keys.Count() == 0 || string.IsNullOrEmpty(keys.ElementAt(0)))
                throw new Exception("fail");
        }

        [Fact]
        public void UpdateTest()
        {
            //Nothing to test. Update method not implemented.
        }

        [Fact]
        public void RenderTest()
        {
            //Nothing to test. 
        }

        [Fact]
        public void UpdateManualTest()
        {
            var aElem = MockObjects.GetAnimationElement();
            if (aElem == null)
                throw new Exception("Assert Fails");

            var values2 = new System.Collections.Specialized.NameValueCollection();
            values2.Add("Prediction", "False");
            values2.Add("ShapeTypeName", "IdkYet3");
            values2.Add("Name", "IdkYet1233");
            aElem.UpdateManual(values2);
            if (aElem.Shape.TypeName != "IdkYet3")
                throw new Exception("Assert Fails");
        }
    }
}