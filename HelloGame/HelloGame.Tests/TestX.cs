using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelloGame.Tests
{
    static class TestX
    {
        public static void AssertAreAlmostSame(double a, double b)
        {
            Assert.IsTrue(MathX.IsAlmostSame(a, b));
        }

        public static void AssertIsAlmostZero(double a)
        {
            AssertAreAlmostSame(a, 0);
        }
    }
}
