using Testowy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Testowy.Model.Tests
{
    [Collection("Synchronical")]
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
        public void SearchParentAnimationElement()//Route
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void SearchParentAnimationElement2()//IAnimationElemnt
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void ConvertToGroup()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void FixParentAnimationTime()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void UpdateManual()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void GetAnimationGroupElements()
        {
            throw new NotImplementedException();
        }
    }
}