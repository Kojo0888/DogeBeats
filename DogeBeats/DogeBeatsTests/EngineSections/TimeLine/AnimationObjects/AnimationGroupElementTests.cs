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
        public void GetAnimationGroupElementsTest()
        {
            element.Elements.Add(new AnimationGroupElement());
            if (element.Elements.Count == 0)
                throw new Exception("Assert Fails");

        }
    }
}