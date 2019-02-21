using Testowy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DogeBeatsTests;
using DogeBeats.EngineSections.Shared;

namespace Testowy.Model.Tests
{
    public class AnimationSingleElementTests
    {
        private AnimationSingleElement element;

        public AnimationSingleElementTests()
        {
            element = new AnimationSingleElement();
        }

        [Fact]
        public void UpdateTest()
        {
            //nothing yet to test
        }

        [Fact]
        public void RenderTest()
        {
            //nothing yet to test
        }

        [Fact]
        public void SearchParentAnimationElement()
        {
            AnimationRouteFrame frame = new AnimationRouteFrame()
            {
                Amplitude = 123,
                Cycles = 1
            };

            element.Route.Frames = new List<AnimationRouteFrame>();
            element.Route.Frames.Add(frame);

            var returnedElement = element.SearchParentAnimationElement(frame);
            if (returnedElement != element)
                throw new NesuException("References differs");
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