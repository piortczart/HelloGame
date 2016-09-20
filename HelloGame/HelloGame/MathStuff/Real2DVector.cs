using System;

namespace HelloGame.MathStuff
{
    public class Real2DVector
    {
        public double X { get; set; }
        public double Y { get; set; }
        private double? _maxBigness;

        public double Bigness { get { return Math.Pow(X * X + Y * Y, 0.5); } }

        public double Angle
        {
            get
            {
                if (X == 0 || (X == 0 && Y == 0)) { return 0; }


                bool sameSign = (X > 0 && Y > 0) || (X < 0 && Y < 0);
                var baseAngle = sameSign ? Math.Abs(Math.Atan(Y / X)) : Math.Abs(Math.Atan(X / Y));

                if (X < 0 && Y < 0)
                {
                    // Bottom left.
                    return baseAngle + Math.PI;
                }
                if (X < 0 && Y > 0)
                {
                    // Top left.
                    return baseAngle + Math.PI / 2;
                }
                if (X > 0 && Y < 0)
                {
                    // Bottom Right.
                    return baseAngle + Math.PI * 3 / 2;
                }

                return baseAngle;
            }
        }

        public double AngleDegree { get { return Angle * 57.296d; } }

        public Real2DVector(double? maxBigness = null)
        {
            _maxBigness = maxBigness;
        }

        public Real2DVector(double angle, double bigness, double? maxBigness = null)
        {
            _maxBigness = maxBigness;
            Set(angle, bigness);
        }

        public Real2DVector GetScaled(double by, bool withRestriction = true)
        {
            var result = Copy(withRestriction);
            result.Set(Angle, Bigness * by);
            return result;
        }

        public Real2DVector GetOpposite()
        {
            var result = Copy();
            result.X = -result.X;
            result.Y = -result.Y;
            return result;
        }

        public void Set(double newAngle, double bigness)
        {
            if (newAngle > 2 * Math.PI)
            {
                newAngle -= 2 * Math.PI;
            }
            else if (newAngle < 0)
            {
                newAngle += 2 * Math.PI;
            }

            if (_maxBigness.HasValue && bigness > _maxBigness.Value)
            {
                bigness = _maxBigness.Value;
            }

            var newX = GetX(newAngle, bigness);
            X = newX;
            var newY = Math.Sin(newAngle) * bigness;
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

        public double GetX(double angle, double bigness)
        {
            return Math.Cos(angle) * bigness;
        }

        public void Change(double newAngle, double bignessDelta)
        {
            double newBigness = Bigness + bignessDelta;

            Set(newAngle, newBigness);
        }

        public static Real2DVector Combine(params Real2DVector[] vectors)
        {
            Real2DVector result = new Real2DVector();
            foreach (Real2DVector vector in vectors)
            {
                result.Add(vector);
            }
            return result;
        }

        public void Add(Real2DVector vector)
        {

            if (_maxBigness.HasValue && Bigness > _maxBigness)
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
            return new Real2DVector(withRestriction ? _maxBigness : null)
            {
                X = X,
                Y = Y
            };
        }

        public override string ToString()
        {
            return $"X_{X:0.00} Y_{Y:0.00} B_{Bigness:0.0} A_{Angle:0.0}";
        }
    }

}
