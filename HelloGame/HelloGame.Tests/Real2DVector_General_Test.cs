using System;
using HelloGame.Common.MathStuff;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelloGame.Tests
{
    [TestClass]
    public class Real2DVector_General_Test
    {
        [TestMethod]
        public void Real2DVector_GetScaled_X_Positive_Y_Zero()
        {
            var vector = new Vector2D
            {
                X = -3,
                Y = 0
            };

            Vector2D scaled = vector.GetScaled(2);

        }

        [TestMethod]
        public void Real2DVector_GetOpposite_Negative()
        {
            var vector = new Vector2D
            {
                X = -3,
                Y = -6
            };

            var opposite = vector.GetOpposite();

            Assert.AreEqual(3, opposite.X);
            Assert.AreEqual(6, opposite.Y);
        }

        [TestMethod]
        public void Real2DVector_GetOpposite_Positive()
        {
            var vector = new Vector2D
            {
                X = 3,
                Y = 6
            };

            var opposite = vector.GetOpposite();

            Assert.AreEqual(-3, opposite.X);
            Assert.AreEqual(-6, opposite.Y);
        }

        [TestMethod]
        public void Real2DVector_Change_Simple()
        {
            Vector2D vector = new Vector2D();
            vector.Change(0, 3); // Go "right"

            Assert.AreEqual(3, vector.X);
            Assert.AreEqual(0, vector.Y);
        }

        [TestMethod]
        public void Real2DVector_Change_Simple2()
        {
            Vector2D vector = new Vector2D();
            vector.Change((decimal)Math.PI, 3); // Go "left"

            Assert.AreEqual(-3, vector.X);
            // The Math.PI is not perfect?
            TestX.AssertIsAlmostZero(vector.Y);
        }

        [TestMethod]
        public void Real2DVector_Change_Simple3()
        {
            Vector2D vector = new Vector2D();
            vector.Change((decimal)Math.PI/2m, 3); // Go "up"

            TestX.AssertIsAlmostZero(vector.X);
            // The Math.PI is not perfect?
            Assert.IsTrue(vector.Y == 3);
        }

        [TestMethod]
        public void Real2DVector_Add_Simple()
        {
            Vector2D sample = new Vector2D
            {
                X = -5m,
                Y = 10m
            };

            Vector2D vector = new Vector2D();
            vector.Add(sample);
            vector.Add(sample);

            Assert.AreEqual(-10m, vector.X);
            Assert.AreEqual(20m, vector.Y);
        }

        [TestMethod]
        public void Real2DVector_GetScaled_Negative()
        {
            Vector2D sample = new Vector2D
            {
                X = -0.7m,
                Y = -0.8m
            };

            var result = sample.GetScaled(0.5m);

            Assert.IsTrue(MathX.IsAlmostSame(-0.35m, result.X));
            Assert.IsTrue(MathX.IsAlmostSame(-0.4m, result.Y));
        }

    }
}
