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
            Vector2D sample = new Vector2D
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
            Vector2D sample = new Vector2D
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
            Vector2D sample = new Vector2D
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
            Vector2D sample = new Vector2D
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
            Vector2D sample = new Vector2D
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
