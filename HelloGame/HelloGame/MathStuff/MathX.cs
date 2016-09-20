using System;

namespace HelloGame
{
    /// <summary>
    /// Mathematical extensions.
    /// </summary>
    public static class MathX
    {
        public static readonly Random Random = new Random();

        public enum Sign
        {
            Negative,
            Positive
        }

        public static decimal DegreeToRadian(decimal angle)
        {
            return (decimal)Math.PI * angle / 180.0m;
        }

        public static decimal RadianToDegree(decimal angle)
        {
            return angle * (180.0m / (decimal)Math.PI);
        }

        public static bool IsAlmostSame(decimal number1, decimal number2)
        {
            return Math.Abs(number1 - number2) < (decimal)Math.Pow(2, -14);
        }

        public static bool IsAlmostZero(decimal number)
        {
            return IsAlmostSame(number, 0);
        }

        /// <summary>
        /// Make sure the number has the correct sign.
        /// </summary>
        public static decimal SetSign(decimal number, Sign sign)
        {
            if ((sign == Sign.Positive && number < 0) || (sign == Sign.Negative && number > 0))
            {
                return -number;
            }
            return number;
        }

    }
}
