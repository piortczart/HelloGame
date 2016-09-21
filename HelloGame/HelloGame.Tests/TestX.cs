using HelloGame.MathStuff;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelloGame.Tests
{
    static class TestX
    {
        public static void AssertAreAlmostSame(decimal a, decimal b)
        {
            Assert.IsTrue(MathX.IsAlmostSame(a, b));
        }

        public static void AssertIsAlmostZero(decimal a)
        {
            AssertAreAlmostSame(a, 0);
        }
    }
}
