using System;
using System.Drawing;

namespace HelloGame.Common.MathStuff
{
    /// <summary>
    /// An ugly and buggy implementation of 2D vector methematics.
    /// 
    /// I imagined this vector to be in XY coordinates, the angle is the angle between the vector and the X axis (starging from 0 up) measured in radians.
    /// This means that:
    ///     - a vector pointing plus infinity (right) on the X axis has an angle of 0
    ///     - a vector pointing minus infinity (left) on the X axis has an algne of PI
    ///     - a vector pointing plus infinity (up) on the Y axis has an algne of PI/2
    ///     - a vector pointing minus infinity (down) on the Y axis has an algne of PI * 3/2
    /// 
    /// TODO: This class can be accessed via multiple threads! Needs some synchro!
    /// </summary>
    public class Vector2D
    {
        public static Vector2D GetZero()
        {
            return new Vector2D(0, 0);
        }

        public Point Point => new Point((int) X, (int) Y);

        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal? MaxSize { get; set; }

        public decimal Size => (decimal) Math.Pow((double) (X*X + Y*Y), 0.5);

        public decimal Angle
        {
            get
            {
                if (X == 0 || (X == 0 && Y == 0))
                {
                    return 0;
                }


                bool sameSign = (X > 0 && Y > 0) || (X < 0 && Y < 0);
                decimal baseAngle =
                    sameSign
                        ? (X == 0 ? 0 : (decimal) Math.Abs(Math.Atan((double) (Y/X))))
                        : (Y == 0 ? 0 : (decimal) Math.Abs(Math.Atan((double) (X/Y))));

                if (X < 0 && Y <= 0)
                {
                    // Bottom left.
                    return baseAngle + (decimal) Math.PI;
                }
                if (X < 0 && Y >= 0)
                {
                    // Top left.
                    return baseAngle + (decimal) Math.PI/2;
                }
                if (X >= 0 && Y < 0)
                {
                    // Bottom Right.
                    return baseAngle + (decimal) Math.PI*3/2;
                }

                return baseAngle;
            }
        }

        public decimal AngleDegree
        {
            get { return Angle*57.296m; }
        }


        public Vector2D()
        {
        }

        public Vector2D(decimal? maxSize = null)
        {
            MaxSize = maxSize;
        }

        public static Vector2D GetFromCoords(decimal x, decimal y, decimal? maxSize = null)
        {
            return new Vector2D
            {
                X = x,
                Y = y,
                MaxSize = maxSize
            };
        }


        public Vector2D(decimal angle, decimal bigness, decimal? maxSize = null)
        {
            MaxSize = maxSize;
            Set(angle, bigness);
        }

        public Vector2D GetScaled(decimal by, bool withRestriction = true)
        {
            var result = Copy(withRestriction);
            result.Set(Angle, Size*by);
            return result;
        }

        public Vector2D GetOpposite()
        {
            var result = Copy();
            result.X = -result.X;
            result.Y = -result.Y;
            return result;
        }

        public void Set(decimal newAngle, decimal size)
        {
            if (newAngle > 2*(decimal) Math.PI)
            {
                newAngle -= 2*(decimal) Math.PI;
            }
            else if (newAngle < 0)
            {
                newAngle += 2*(decimal) Math.PI;
            }

            if (MaxSize.HasValue && size > MaxSize.Value)
            {
                size = MaxSize.Value;
            }

            var newX = GetX(newAngle, size);
            X = newX;
            var newY = (decimal) Math.Sin((double) newAngle)*size;
            Y = newY;

            var newAngleDeg = MathX.RadianToDegree(newAngle);

            if (newAngleDeg > 90 && newAngleDeg <= 180)
            {
                X = MathX.SetSign(X, MathX.Sign.Negative);
                Y = MathX.SetSign(Y, MathX.Sign.Positive);
            }
            else if (newAngleDeg > 180 && newAngleDeg <= 270)
            {
                X = MathX.SetSign(X, MathX.Sign.Negative);
                Y = MathX.SetSign(Y, MathX.Sign.Negative);
            }
            else if (newAngleDeg > 270)
            {
                X = MathX.SetSign(X, MathX.Sign.Positive);
                Y = MathX.SetSign(Y, MathX.Sign.Negative);
            }
        }

        public decimal GetX(decimal angle, decimal bigness)
        {
            return (decimal) Math.Cos((double) angle)*bigness;
        }

        public void ChangeSize(decimal sizeIncrease)
        {
            Change(Angle, sizeIncrease);
        }


        public void Change(decimal newAngle, decimal bignessDelta)
        {
            decimal newBigness = Size + bignessDelta;
            Set(newAngle, newBigness);
        }

        public static Vector2D Combine(params Vector2D[] vectors)
        {
            Vector2D result = new Vector2D();
            foreach (Vector2D vector in vectors)
            {
                if (vector != null)
                {
                    result.Add(vector);
                }
            }
            return result;
        }

        public void Add(Vector2D vector)
        {
            if (MaxSize.HasValue && Size > MaxSize)
            {
                X += vector.X;
                Y += vector.Y;
            }
            else
            {
                X += vector.X;
                Y += vector.Y;
            }
        }

        public Vector2D Copy(bool withRestriction = true)
        {
            return new Vector2D(withRestriction ? MaxSize : null)
            {
                X = X,
                Y = Y
            };
        }

        public override string ToString()
        {
            return $"X_{X:0.00} Y_{Y:0.00} B_{Size:0.0} A_{Angle:0.0}";
        }
    }
}