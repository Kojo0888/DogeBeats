using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogeBeats.Modules.Centers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.Modules.Centers.Tests
{
    [TestClass()]
    public class CenterTrigonometricTests
    {
        [TestMethod()]
        public void CenterTrigonometricTest()
        {
            TrigonometricCache centerTrigonometric = new TrigonometricCache("sin", 0.0001);
        }

        [TestMethod()]
        [DataRow(0.0001)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(0.0341)]
        [DataRow(1.0301)]
        public void GetTest(double testValue)
        {
            TrigonometricCache centerTrigonometric = new TrigonometricCache("sin", 0.0001);
            //var testValue = 0.003;
            var centerTrig = centerTrigonometric.Get(testValue);
            Assert.AreEqual(centerTrig, Math.Sin(testValue));
        }
    }
}