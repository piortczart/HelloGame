using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelloGame.Common.Model;

namespace HelloGame.Tests
{
    [TestClass]
    public class ShieldTest
    {
        [TestMethod]
        public void ShieldTest_Percentage()
        {
            var shield = new Shield(10) { Current = 5};

            Assert.AreEqual(50, shield.Percentage);
        }
    }
}
