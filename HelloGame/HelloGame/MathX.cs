using System;

namespace HelloGame
{
    public static class MathX
    {
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

        public static double SetSign(double number, bool positive)
        {
            if ((positive && number < 0) || (!positive && number > 0))
            {
                return -number;
            }
            return number;
        }

    }
}
