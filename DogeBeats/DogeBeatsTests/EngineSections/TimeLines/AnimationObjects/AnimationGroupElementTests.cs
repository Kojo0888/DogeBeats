using Testowy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DogeBeats.EngineSections.Shared;

namespace Testowy.Model.Tests
{
    public class AnimationGroupElementTests
    {
        public AnimationGroupElement element = new AnimationGroupElement();

        public AnimationGroupElementTests()
        {
            element = new AnimationGroupElement();
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
        public void GetAnimationGroupElementsTest()
        {
            element.Elements.Add(new AnimationGroupElement());
            if (element.Elements.Count == 0)
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void SearchParentAnimationElement_Route()
        {
            var testFrame = new AnimationRouteFrame();
            testFrame.Amplitude = 123;

            element.Route.Frames = new List<AnimationRouteFrame>();
            element.Route.Frames.Add(testFrame);

            var resultEelemtn = element.SearchParentAnimationElement(testFrame);
            if (resultEelemtn != element)
                throw new NesuException("REf differs");
        }

        [Fact]
        public void SearchParentAnimationElement_RouteChild()
        {
            var testFrame = new AnimationRouteFrame();
            testFrame.Amplitude = 123;

            element.Route.Frames = new List<AnimationRouteFrame>();
            element.Route.Frames.Add(new AnimationRouteFrame());

            var childElem = new AnimationSingleElement();
            childElem.Route.Frames = new List<AnimationRouteFrame>();
            childElem.Route.Frames.Add(testFrame);

            element.Elements.Add(childElem);

            var resultEelemtn = element.SearchParentAnimationElement(testFrame);
            if (resultEelemtn != childElem)
                throw new NesuException("REf differs");
        }

        [Fact]
        public void SearchParentAnimationElement_IAnimationElement()//IAnimationElemnt
        {
            var childElem = new AnimationSingleElement();
            childElem.Route.Frames = new List<AnimationRouteFrame>();

            element.Elements.Add(childElem);

            var resultEelemtn = element.SearchParentAnimationElement(childElem);
            if (resultEelemtn != element)
                throw new NesuException("REf differs");
        }

        [Fact]
        public void ConvertToGroup()
        {
            var singleElement = new AnimationSingleElement();
            string testName = "tst1";
            singleElement.Name = testName;

            var returnedGroupElement = element.ConvertToGroup(singleElement);

            if (returnedGroupElement == null)
                throw new NesuException(nameof(returnedGroupElement) + " is null");
            if (returnedGroupElement.Name != testName)
                throw new NesuException("Name is not " + testName);
            if (element.Elements.Contains(singleElement))
                throw new NesuException("Parent element still holds single element");
            if (!element.Elements.Contains(returnedGroupElement))
                throw new NesuException("Parent element does not hold new goup element");
        }

        [Fact]
        public void UpdateManual()
        {
            var values2 = new System.Collections.Specialized.NameValueCollection();
            values2.Add("Prediction", "False");
            values2.Add("Name", "IdkYet1233");
            element.UpdateManual(values2);
            if (element.Name != "IdkYet1233")
                throw new Exception("Assert Fails");
        }

        [Fact]
        public void GetAnimationGroupElements()
        {
            element.Elements = new List<DogeBeats.EngineSections.AnimationObjects.IAnimationElement>();
            element.Elements.Add(new AnimationSingleElement() { Name = "S1" });
            element.Elements.Add(new AnimationGroupElement() { Name = "G1" });
            element.Elements.Add(new AnimationGroupElement() { Name = "G2" });
            element.Elements.Add(new AnimationSingleElement() { Name = "S2" });
            element.Elements.Add(new AnimationGroupElement() { Name = "G3" });

            var results = element.GetAnimationGroupElements();
            if (results == null)
                throw new NesuException("Results are null");
            if (results.Count != 3)
                throw new NesuException("Results count is " + results.Count);
        }
    }
}