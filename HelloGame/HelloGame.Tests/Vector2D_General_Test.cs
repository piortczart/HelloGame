using System;
using HelloGame.Common.MathStuff;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelloGame.Tests
{
    [TestClass]
    public class Vector2D_General_Test
    {
        [TestMethod]
        public void Vector2D_GetOpposite_Negative()
        {
            var vector = Vector2D.GetFromCoords(-3, -6);

            var opposite = vector.GetOpposite();

            Assert.AreEqual(3, opposite.X);
            Assert.AreEqual(6, opposite.Y);
        }

        [TestMethod]
        public void Vector2D_GetOpposite_Positive()
        {
            var vector = Vector2D.GetFromCoords(3, 6);

            var opposite = vector.GetOpposite();

            Assert.AreEqual(-3, opposite.X);
            Assert.AreEqual(-6, opposite.Y);
        }

        [TestMethod]
        public void Vector2D_Change_Simple()
        {
            Vector2D vector = Vector2D.Zero();
            vector.Change(0, 3); // Go "right"

            Assert.AreEqual(3, vector.X);
            Assert.AreEqual(0, vector.Y);
        }

        [TestMethod]
        public void Vector2D_Change_Simple2()
        {
            Vector2D vector = Vector2D.Zero();
            vector.Change((float) Math.PI, 3); // Go "left"

            Assert.AreEqual(-3, vector.X);
            // The Math.PI is not perfect?
            TestX.AssertIsAlmostZero(vector.Y);
        }

        [TestMethod]
        public void Vector2D_Change_Simple3()
        {
            Vector2D vector = Vector2D.Zero();
            vector.Change((float) Math.PI/2, 3); // Go "up"

            TestX.AssertIsAlmostZero(vector.X);
            // The Math.PI is not perfect?
            Assert.IsTrue(MathX.IsAlmostSame(vector.Y, 3));
        }

        [TestMethod]
        public void Vector2D_Add_Simple()
        {
            Vector2D sample = Vector2D.GetFromCoords(-5f, 10f);

            Vector2D vector = Vector2D.Zero();
            vector.Add(sample);
            vector.Add(sample);

            Assert.AreEqual(-10f, vector.X);
            Assert.AreEqual(20f, vector.Y);
        }

        [TestMethod]
        public void Vector2D_GetScaled_Negative()
        {
            Vector2D sample = Vector2D.GetFromCoords(-0.7f, -0.8f);

            var result = sample.GetScaled(0.5f);

            Assert.IsTrue(MathX.IsAlmostSame(-0.35f, result.X));
            Assert.IsTrue(MathX.IsAlmostSame(-0.4f, result.Y));
        }
    }
}