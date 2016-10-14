using HelloGame.Common.MathStuff;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelloGame.Tests
{
    [TestClass]
    public class Vector2D_Reversability_Tests
    {
        [TestMethod]
        public void Vector2D_Reversibility_NegNeg()
        {
            Vector2D sample = Vector2D.GetFromCoords(-0.7f, -0.8f);

            var newVector = Vector2D.Zero();
            newVector.Set(sample.Angle, sample.Size);

            Assert.IsTrue(MathX.IsAlmostSame(sample.X, newVector.X));
            Assert.IsTrue(MathX.IsAlmostSame(sample.Y, newVector.Y));
        }

        [TestMethod]
        public void Vector2D_AngleReversibility_NegPos()
        {
            Vector2D sample = Vector2D.GetFromCoords(-0.7f, 0.8f);

            var newVector = Vector2D.Zero();
            newVector.Set(sample.Angle, sample.Size);

            Assert.IsTrue(MathX.IsAlmostSame(sample.X, newVector.X));
            Assert.IsTrue(MathX.IsAlmostSame(sample.Y, newVector.Y));
        }

        [TestMethod]
        public void Vector2D_AngleReversibility_PosNes()
        {
            Vector2D sample = Vector2D.GetFromCoords(0.7f, -0.8f);

            var newVector = Vector2D.Zero();
            newVector.Set(sample.Angle, sample.Size);

            Assert.IsTrue(MathX.IsAlmostSame(sample.X, newVector.X));
            Assert.IsTrue(MathX.IsAlmostSame(sample.Y, newVector.Y));
        }

        [TestMethod]
        public void Vector2D_AngleReversibility_PosPos()
        {
            Vector2D sample = Vector2D.GetFromCoords(0.7f, 0.8f);

            var newVector = Vector2D.Zero();
            newVector.Set(sample.Angle, sample.Size);

            Assert.IsTrue(MathX.IsAlmostSame(sample.X, newVector.X));
            Assert.IsTrue(MathX.IsAlmostSame(sample.Y, newVector.Y));
        }
    }
}