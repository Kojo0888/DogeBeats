using DogeBeats.Modules.Centers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DogeBeats.Modules.Centers.Tests
{
    public class CenterTrigonometricTests
    {
        [Fact]
        public void CenterTrigonometricTest()
        {
            TrigonometricCache centerTrigonometric = new TrigonometricCache("sin", 0.0001);
        }

        [Theory]
        [InlineData(0.0001)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(0.0341)]
        [InlineData(1.0301)]
        public void GetTest(double testValue)
        {
            TrigonometricCache centerTrigonometric = new TrigonometricCache("sin", 0.0001);
            //var testValue = 0.003;
            var centerTrig = centerTrigonometric.Get(testValue);
            Assert.Equal(centerTrig, Math.Sin(testValue));
        }
    }
}