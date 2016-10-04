using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelloGame.Common;

namespace HelloGame.Tests
{
    [TestClass]
    public class TimeCounterTests
    {
        [TestMethod]
        public void TimeCounter_BasicTest()
        {
            var timeSource = new TimeSource(false);
            TimeCounter timeCounter = new TimeCounter(timeSource);

            TimeSpan counted1 = timeCounter.GetTimeSinceLastCall();
            Assert.AreEqual(TimeSpan.Zero, counted1);

            // Pretend 1 second has passed, measure and check.
            timeSource.SkipTime(TimeSpan.FromSeconds(1));
            TimeSpan counted2 = timeCounter.GetTimeSinceLastCall();
            Assert.AreEqual(TimeSpan.FromSeconds(1), counted2);

            // Pretend 1 day has passed, measure and check.
            timeSource.SkipTime(TimeSpan.FromDays(1));
            TimeSpan counted3 = timeCounter.GetTimeSinceLastCall();
            Assert.AreEqual(TimeSpan.FromDays(1), counted3);
        }
    }
}
