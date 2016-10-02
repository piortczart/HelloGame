using HelloGame.Common.MathStuff;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelloGame.Tests
{
    [TestClass]
    public class Real2DVector_Angle_Test
    {
        [TestMethod]
        public void Real2DVector_Angle_NegZero()
        {
            Real2DVector sample = new Real2DVector
            {
                X = -0.8m,
                Y = 0
            };

            decimal actual = sample.Angle;
            decimal expected = MathX.DegreeToRadian(180);

            TestX.AssertAreAlmostSame(actual, expected);
        }

        [TestMethod]
        public void Real2DVector_Angle_PosPos()
        {
            Real2DVector sample = new Real2DVector
            {
                X = 0.8m,
                Y = 0.3m
            };

            var actual = sample.Angle;
            var expected = MathX.DegreeToRadian(20.556m);
            TestX.AssertAreAlmostSame(actual, expected);
        }

        [TestMethod]
        public void Real2DVector_Angle_NegNeg()
        {
            Real2DVector sample = new Real2DVector
            {
                X = -0.8m,
                Y = -0.3m
            };

            var actual = sample.Angle;
            var expected = MathX.DegreeToRadian(180m + 20.556m);
            TestX.AssertAreAlmostSame(actual, expected);
        }


        [TestMethod]
        public void Real2DVector_Angle_PosNeg()
        {
            Real2DVector sample = new Real2DVector
            {
                X = 0.8m,
                Y = -0.3m
            };

            var actual = sample.Angle;
            var expected = MathX.DegreeToRadian(270m + 69.444m);
            TestX.AssertAreAlmostSame(actual, expected);
        }

        [TestMethod]
        public void Real2DVector_Angle_NegPos()
        {
            Real2DVector sample = new Real2DVector
            {
                X = -0.8m,
                Y = 0.3m
            };

            var actual = sample.Angle;
            var expected = MathX.DegreeToRadian(90m + 69.444m);
            TestX.AssertAreAlmostSame(actual, expected);
        }
    }
}
