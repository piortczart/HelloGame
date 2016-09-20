using System;
using HelloGame.MathStuff;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelloGame.Tests
{
    [TestClass]
    public class Real2DVector_Angle_Test
    {
        [TestMethod]
        public void Real2DVector_Angle_PosPos()
        {
            Real2DVector sample = new Real2DVector
            {
                X = 0.8,
                Y = 0.3
            };

            var actual = sample.Angle;
            var expected = MathX.DegreeToRadian(20.556);
            TestX.AssertAreAlmostSame(actual, expected);
        }

        [TestMethod]
        public void Real2DVector_Angle_NegNeg()
        {
            Real2DVector sample = new Real2DVector
            {
                X = -0.8,
                Y = -0.3
            };

            var actual = sample.Angle;
            var expected = MathX.DegreeToRadian(180 + 20.556);
            TestX.AssertAreAlmostSame(actual, expected);
        }


        [TestMethod]
        public void Real2DVector_Angle_PosNeg()
        {
            Real2DVector sample = new Real2DVector
            {
                X = 0.8,
                Y = -0.3
            };

            var actual = sample.Angle;
            var expected = MathX.DegreeToRadian(270 + 69.444);
            TestX.AssertAreAlmostSame(actual, expected);
        }

        [TestMethod]
        public void Real2DVector_Angle_NegPos()
        {
            Real2DVector sample = new Real2DVector
            {
                X = -0.8,
                Y = 0.3
            };

            var actual = sample.Angle;
            var expected = MathX.DegreeToRadian(90 + 69.444);
            TestX.AssertAreAlmostSame(actual, expected);
        }
    }
}
