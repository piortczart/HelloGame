using HelloGame.Common.MathStuff;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelloGame.Tests
{
    [TestClass]
    public class Vector2D_Angle_Test
    {
        [TestMethod]
        public void Vector2D_Angle_NegZero()
        {
            var sample = Vector2D.GetFromCoords(-0.8f, 0);

            float actual = sample.Angle;
            float expected = MathX.DegreeToRadian(180);

            TestX.AssertAreAlmostSame(actual, expected);
        }

        [TestMethod]
        public void Vector2D_Angle_PosPos()
        {
            Vector2D sample = Vector2D.GetFromCoords(0.8f, 0.3f);

            var actual = sample.Angle;
            var expected = MathX.DegreeToRadian(20.556f);
            TestX.AssertAreAlmostSame(actual, expected);
        }

        [TestMethod]
        public void Vector2D_Angle_NegNeg()
        {
            Vector2D sample = Vector2D.GetFromCoords(-0.8f, -0.3f);

            var actual = sample.Angle;
            var expected = MathX.DegreeToRadian(180f + 20.556f);
            TestX.AssertAreAlmostSame(actual, expected);
        }


        [TestMethod]
        public void Vector2D_Angle_PosNeg()
        {
            Vector2D sample = Vector2D.GetFromCoords(0.8f, -0.3f);

            var actual = sample.Angle;
            var expected = MathX.DegreeToRadian(270f + 69.444f);
            TestX.AssertAreAlmostSame(actual, expected);
        }

        [TestMethod]
        public void Vector2D_Angle_NegPos()
        {
            Vector2D sample = Vector2D.GetFromCoords(-0.8f, 0.3f);

            var actual = sample.Angle;
            var expected = MathX.DegreeToRadian(90f + 69.444f);
            TestX.AssertAreAlmostSame(actual, expected);
        }
    }
}