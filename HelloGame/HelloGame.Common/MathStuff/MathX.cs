using System;

namespace HelloGame.Common.MathStuff
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

        public static float DegreeToRadian(float angle)
        {
            return (float) Math.PI*angle/180.0f;
        }

        public static float RadianToDegree(float angle)
        {
            return angle*(180.0f/(float) Math.PI);
        }

        public static bool IsAlmostSame(float number1, float number2)
        {
            return Math.Abs(number1 - number2) < (float) Math.Pow(2, -14);
        }

        public static bool IsAlmostZero(float number)
        {
            return IsAlmostSame(number, 0);
        }

        /// <summary>
        /// Make sure the number has the correct sign.
        /// </summary>
        public static float SetSign(float number, Sign sign)
        {
            if ((sign == Sign.Positive && number < 0) || (sign == Sign.Negative && number > 0))
            {
                return -number;
            }
            return number;
        }
    }
}