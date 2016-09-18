using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace HelloGame.Tests
{
    [TestClass]
    public class Real2DVectorTest
    {
        [TestMethod]
        public void Real2DVector_Change_Simple()
        {
            Real2DVector vector = new Real2DVector();
            vector.Change(0, 3); // Go "right"

            Assert.AreEqual(3, vector.X);
            Assert.AreEqual(0, vector.Y);
        }

        [TestMethod]
        public void Real2DVector_Change_Simple2()
        {
            Real2DVector vector = new Real2DVector();
            vector.Change(Math.PI, 3); // Go "left"

            Assert.AreEqual(-3, vector.X);
            // The Math.PI is not perfect?
            Assert.IsTrue(IsAlmostZero(vector.Y));
        }

        [TestMethod]
        public void Real2DVector_Change_Simple3()
        {
            Real2DVector vector = new Real2DVector();
            vector.Change(Math.PI/2, 3); // Go "up"

            Assert.IsTrue(IsAlmostZero(vector.X));
            // The Math.PI is not perfect?
            Assert.IsTrue(vector.Y == 3);
        }

        [TestMethod]
        public void Real2DVector_Add_Simple()
        {
            Real2DVector sample = new Real2DVector
            {
                X = -5,
                Y = 10
            };

            Real2DVector vector = new Real2DVector();
            vector.Add(sample);
            vector.Add(sample);

            Assert.AreEqual(-10, vector.X);
            Assert.AreEqual(20, vector.Y);
        }

        [TestMethod]
        public void Real2DVector_Add_Simple2()
        {
            Real2DVector sample = new Real2DVector
            {
                X = -0.7,
                Y = -0.8
            };

            Real2DVector vector = new Real2DVector(5);
            vector.Add(sample);
            Trace.WriteLine(vector.ToString());
            vector.Add(sample);
            Trace.WriteLine(vector.ToString());
            vector.Add(sample);
            Trace.WriteLine(vector.ToString());
            vector.Add(sample);
            Trace.WriteLine(vector.ToString());
            vector.Add(sample);
            Trace.WriteLine(vector.ToString());
            vector.Add(sample);
            Trace.WriteLine(vector.ToString());
            vector.Add(sample);
            Trace.WriteLine(vector.ToString());
            vector.Add(sample);
            Trace.WriteLine(vector.ToString());
            vector.Add(sample);
            Trace.WriteLine(vector.ToString());
            vector.Add(sample);
            Trace.WriteLine(vector.ToString());
            vector.Add(sample);
            Trace.WriteLine(vector.ToString());

            Assert.AreEqual(-0.7, vector.X);
        }

        public static bool IsAlmostZero(double number)
        {
            return number < Math.Pow(1, -10);
        }
    }
}
