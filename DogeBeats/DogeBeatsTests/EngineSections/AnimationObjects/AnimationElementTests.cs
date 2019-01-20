using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testowy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogeBeatsTests;

namespace Testowy.Model.Tests
{
    [TestClass()]
    public class AnimationElementTests
    {
        public AnimationSingleElement aElem { get; set; }

        [TestInitialize]
        public void Init()
        {
            aElem = new AnimationSingleElement();
        }

        [TestMethod()]
        public void AnimationElementTest()
        {
            aElem = new AnimationSingleElement();
        }

        [TestMethod()]
        public void CreateTest()
        {
            var values = new System.Collections.Specialized.NameValueCollection();
            values.Add("Prediction", "False");
            values.Add("ShapeName", "IdkYet");
            var aElem = AnimationSingleElement.Create(values);
            if(aElem == null)
                Assert.Fail();
        }

        [TestMethod()]
        public void GetKeysManualUpdateTest()
        {
            var keys = AnimationSingleElement.GetKeysManualUpdate();
            if (keys == null || keys.Count() == 0 || string.IsNullOrEmpty(keys.ElementAt(0)))
                Assert.Fail();
        }

        [TestMethod()]
        public void UpdateTest()
        {
            //Nothing to test. Update method not implemented.
        }

        [TestMethod()]
        public void RenderTest()
        {
            //Nothing to test. 
        }

        [TestMethod()]
        public void UpdateManualTest()
        {
            var aElem = MockObjects.GetAnimationElement();
            if (aElem == null)
                Assert.Fail();

            var values2 = new System.Collections.Specialized.NameValueCollection();
            values2.Add("Prediction", "False");
            values2.Add("ShapeTypeName", "IdkYet3");
            aElem.UpdateManual(values2);
            if (aElem.Shape.TypeName != "IdkYet3")
                Assert.Fail();
        }
    }
}