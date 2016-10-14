using HelloGame.Common.MathStuff;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelloGame.Tests
{
    static class TestX
    {
        public static void AssertAreAlmostSame(float a, float b)
        {
            Assert.IsTrue(MathX.IsAlmostSame(a, b));
        }

        public static void AssertIsAlmostZero(float a)
        {
            AssertAreAlmostSame(a, 0);
        }
    }
}