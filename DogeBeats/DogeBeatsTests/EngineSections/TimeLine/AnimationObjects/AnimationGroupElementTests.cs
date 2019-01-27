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
    public class AnimationGroupElementTests
    {
        public AnimationGroupElement element = new AnimationGroupElement();

        [TestInitialize]
        public void Init()
        {
            element = new AnimationGroupElement();
        }

        [TestMethod()]
        public void GetAnimationGroupElementsTest()
        {
            element.Elements.Add(new AnimationGroupElement());
            if (element.Elements.Count == 0)
                Assert.Fail();

        }
    }
}