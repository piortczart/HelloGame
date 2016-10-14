using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
        public Point Point => new Point((int) X, (int) Y);

        public float X { get; private set; }
        public float Y { get; private set; }
        public float? MaxSize { get; set; }

        public float Size => (float) Math.Pow(X*X + Y*Y, 0.5);

        public float Angle
        {
            get
            {
                if (X == 0 || (X == 0 && Y == 0))
                {
                    return 0;
                }


                bool sameSign = (X > 0 && Y > 0) || (X < 0 && Y < 0);
                float baseAngle =
                    sameSign
                        ? (X == 0 ? 0 : (float) Math.Abs(Math.Atan(Y/X)))
                        : (Y == 0 ? 0 : (float) Math.Abs(Math.Atan(X/Y)));

                if (X < 0 && Y <= 0)
                {
                    // Bottom left.
                    return baseAngle + (float) Math.PI;
                }
                if (X < 0 && Y >= 0)
                {
                    // Top left.
                    return baseAngle + (float) Math.PI/2;
                }
                if (X >= 0 && Y < 0)
                {
                    // Bottom Right.
                    return baseAngle + (float) Math.PI*3/2;
                }

                return baseAngle;
            }
        }

        /// <summary>
        /// Gets the angle in degrees.
        /// </summary>
        public float AngleDegree => Angle*57.296f;

        private Vector2D()
        {
        }

        private Vector2D(float? maxSize = null)
        {
            MaxSize = maxSize;
        }

        public static Vector2D Zero(float? maxLength = null)
        {
            return GetFromCoords(0, 0, maxLength);
        }

        public static Vector2D GetFromAngleLength(float angle, float length, float? maxLength = null)
        {
            var result = Zero(maxLength);
            result.Set(angle, length);
            return result;
        }

        public static Vector2D GetFromCoords(float x, float y, float? maxSize = null)
        {
            return new Vector2D
            {
                X = x,
                Y = y,
                MaxSize = maxSize
            };
        }

        public Vector2D GetScaled(float by, bool withRestriction = true)
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

        public void Set(float newAngle, float size)
        {
            if (newAngle > 2*(float) Math.PI)
            {
                newAngle -= 2*(float) Math.PI;
            }
            else if (newAngle < 0)
            {
                newAngle += 2*(float) Math.PI;
            }

            if (MaxSize.HasValue && size > MaxSize.Value)
            {
                size = MaxSize.Value;
            }

            var newX = GetX(newAngle, size);
            X = newX;
            var newY = (float) Math.Sin(newAngle)*size;
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

        public float GetX(float angle, float bigness)
        {
            return (float) Math.Cos(angle)*bigness;
        }

        public void ChangeSize(float sizeIncrease)
        {
            Change(Angle, sizeIncrease);
        }

        public void Change(float newAngle, float lengthDelta)
        {
            float newBigness = Size + lengthDelta;
            Set(newAngle, newBigness);
        }

        public static Vector2D Combine(params Vector2D[] vectors)
        {
            return Combine(vectors.AsEnumerable());
        }

        public static Vector2D Combine(IEnumerable<Vector2D> vectors)
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