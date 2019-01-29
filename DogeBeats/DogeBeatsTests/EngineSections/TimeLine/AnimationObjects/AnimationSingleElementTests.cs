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
    }
}