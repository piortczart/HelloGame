using System;

namespace HelloGame.MathStuff
{
    public class Real2DVector
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
        private decimal? _maxSize;

        public decimal Size => (decimal)Math.Pow((double)(X * X + Y * Y), 0.5);

        public decimal Angle
        {
            get
            {
                if (X == 0 || (X == 0 && Y == 0)) { return 0; }


                bool sameSign = (X > 0 && Y > 0) || (X < 0 && Y < 0);
                decimal baseAngle = 
                    sameSign ?
                        (X == 0 ? 0 : (decimal)Math.Abs(Math.Atan((double)(Y / X)))) :
                        (Y == 0 ? 0 : (decimal)Math.Abs(Math.Atan((double)(X / Y))));

                if (X < 0 && Y < 0)
                {
                    // Bottom left.
                    return baseAngle + (decimal)Math.PI;
                }
                if (X < 0 && Y > 0)
                {
                    // Top left.
                    return baseAngle + (decimal)Math.PI / 2;
                }
                if (X > 0 && Y < 0)
                {
                    // Bottom Right.
                    return baseAngle + (decimal)Math.PI * 3 / 2;
                }

                return baseAngle;
            }
        }

        public decimal AngleDegree { get { return Angle * 57.296m; } }

        public Real2DVector(decimal? maxSize = null)
        {
            _maxSize = maxSize;
        }

        public static Real2DVector GetFromCoords(decimal x, decimal y, decimal? maxSize = null)
        {
            return new Real2DVector
            {
                X = x,
                Y = y,
                _maxSize = maxSize
            };
        }


        public Real2DVector(decimal angle, decimal bigness, decimal? maxSize = null)
        {
            _maxSize = maxSize;
            Set(angle, bigness);
        }

        public Real2DVector GetScaled(decimal by, bool withRestriction = true)
        {
            var result = Copy(withRestriction);
            result.Set(Angle, Size * by);
            return result;
        }

        public Real2DVector GetOpposite()
        {
            var result = Copy();
            result.X = -result.X;
            result.Y = -result.Y;
            return result;
        }

        public void Set(decimal newAngle, decimal size)
        {
            if (newAngle > 2 * (decimal)Math.PI)
            {
                newAngle -= 2 * (decimal)Math.PI;
            }
            else if (newAngle < 0)
            {
                newAngle += 2 * (decimal)Math.PI;
            }

            if (_maxSize.HasValue && size > _maxSize.Value)
            {
                size = _maxSize.Value;
            }

            var newX = GetX(newAngle, size);
            X = newX;
            var newY = (decimal)Math.Sin((double)newAngle) * size;
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
            return (decimal)Math.Cos((double)angle) * bigness;
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

        public static Real2DVector Combine(params Real2DVector[] vectors)
        {
            Real2DVector result = new Real2DVector();
            foreach (Real2DVector vector in vectors)
            {
                if (vector != null)
                {
                    result.Add(vector);
                }
            }
            return result;
        }

        public void Add(Real2DVector vector)
        {

            if (_maxSize.HasValue && Size > _maxSize)
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

        public Real2DVector Copy(bool withRestriction = true)
        {
            return new Real2DVector(withRestriction ? _maxSize : null)
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
