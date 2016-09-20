using System;

namespace HelloGame
{
    /// <summary>
    /// Mathematical extensions.
    /// </summary>
    public static class MathX
    {
        public enum Sign
        {
            Negative,
            Positive
        }

        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        public static bool IsAlmostSame(double number1, double number2)
        {
            return Math.Abs(number1 - number2) < Math.Pow(2, -14);
        }

        public static bool IsAlmostZero(double number)
        {
            return IsAlmostSame(number, 0);
        }

        /// <summary>
        /// Make sure the number has the correct sign.
        /// </summary>
        public static double SetSign(double number, Sign sign)
        {
            if ((sign == Sign.Positive && number < 0) || (sign == Sign.Negative && number > 0))
            {
                return -number;
            }
            return number;
        }

    }
}
