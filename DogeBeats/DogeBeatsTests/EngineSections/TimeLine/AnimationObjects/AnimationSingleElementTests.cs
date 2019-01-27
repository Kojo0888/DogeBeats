using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testowy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testowy.Model.Tests
{
    [TestClass()]
    public class AnimationSingleElementTests
    {
        private AnimationSingleElement element;

        [TestInitialize]
        public void Init()
        {
            element = new AnimationSingleElement();
        }

        [TestMethod()]
        public void UpdateTest()
        {
            //nothing yet to test
        }

        [TestMethod()]
        public void RenderTest()
        {
            //nothing yet to test
        }
    }
}