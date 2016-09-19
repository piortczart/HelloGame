using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelloGame.Tests
{
    [TestClass]
    public class Real2DVector_Reversability_Tests
    {

        [TestMethod]
        public void Real2DVector_Reversibility_NegNeg()
        {
            Real2DVector sample = new Real2DVector
            {
                X = -0.7,
                Y = -0.8
            };

            var newVector = new Real2DVector();
            newVector.Set(sample.Angle, sample.Bigness);

            Assert.IsTrue(MathX.IsAlmostSame(sample.X, newVector.X));
            Assert.IsTrue(MathX.IsAlmostSame(sample.Y, newVector.Y));
        }

        [TestMethod]
        public void Real2DVector_AngleReversibility_NegPos()
        {
            Real2DVector sample = new Real2DVector
            {
                X = -0.7,
                Y = 0.8
            };

            double deg = MathX.RadianToDegree(sample.Angle);

            var newVector = new Real2DVector();
            newVector.Set(sample.Angle, sample.Bigness);

            Assert.IsTrue(MathX.IsAlmostSame(sample.X, newVector.X));
            Assert.IsTrue(MathX.IsAlmostSame(sample.Y, newVector.Y));
        }

        [TestMethod]
        public void Real2DVector_AngleReversibility_PosNes()
        {
            Real2DVector sample = new Real2DVector
            {
                X = 0.7,
                Y = -0.8
            };

            var newVector = new Real2DVector();
            newVector.Set(sample.Angle, sample.Bigness);

            Assert.IsTrue(MathX.IsAlmostSame(sample.X, newVector.X));
            Assert.IsTrue(MathX.IsAlmostSame(sample.Y, newVector.Y));
        }

        [TestMethod]
        public void Real2DVector_AngleReversibility_PosPos()
        {
            Real2DVector sample = new Real2DVector
            {
                X = 0.7,
                Y = 0.8
            };

            var newVector = new Real2DVector();
            newVector.Set(sample.Angle, sample.Bigness);

            Assert.IsTrue(MathX.IsAlmostSame(sample.X, newVector.X));
            Assert.IsTrue(MathX.IsAlmostSame(sample.Y, newVector.Y));
        }

    }
}
