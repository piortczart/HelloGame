using HelloGame.Common.MathStuff;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelloGame.Tests
{
    [TestClass]
    public class Real2DVector_Reversability_Tests
    {

        [TestMethod]
        public void Real2DVector_Reversibility_NegNeg()
        {
            Vector2D sample = new Vector2D
            {
                X = -0.7m,
                Y = -0.8m
            };

            var newVector = new Vector2D();
            newVector.Set(sample.Angle, sample.Size);

            Assert.IsTrue(MathX.IsAlmostSame(sample.X, newVector.X));
            Assert.IsTrue(MathX.IsAlmostSame(sample.Y, newVector.Y));
        }

        [TestMethod]
        public void Real2DVector_AngleReversibility_NegPos()
        {
            Vector2D sample = new Vector2D
            {
                X = -0.7m,
                Y = 0.8m
            };

            decimal deg = MathX.RadianToDegree(sample.Angle);

            var newVector = new Vector2D();
            newVector.Set(sample.Angle, sample.Size);

            Assert.IsTrue(MathX.IsAlmostSame(sample.X, newVector.X));
            Assert.IsTrue(MathX.IsAlmostSame(sample.Y, newVector.Y));
        }

        [TestMethod]
        public void Real2DVector_AngleReversibility_PosNes()
        {
            Vector2D sample = new Vector2D
            {
                X = 0.7m,
                Y = -0.8m
            };

            var newVector = new Vector2D();
            newVector.Set(sample.Angle, sample.Size);

            Assert.IsTrue(MathX.IsAlmostSame(sample.X, newVector.X));
            Assert.IsTrue(MathX.IsAlmostSame(sample.Y, newVector.Y));
        }

        [TestMethod]
        public void Real2DVector_AngleReversibility_PosPos()
        {
            Vector2D sample = new Vector2D
            {
                X = 0.7m,
                Y = 0.8m
            };

            var newVector = new Vector2D();
            newVector.Set(sample.Angle, sample.Size);

            Assert.IsTrue(MathX.IsAlmostSame(sample.X, newVector.X));
            Assert.IsTrue(MathX.IsAlmostSame(sample.Y, newVector.Y));
        }

    }
}
