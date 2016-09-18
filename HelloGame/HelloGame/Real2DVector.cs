using System;

namespace HelloGame
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
                // tan(a) = Y / X
                // a = atan(Y/X) ?
                if (X == 0)
                {
                    return 0;
                }
                return Math.Atan(Y/X);
            }
        }

        public double AngleDegree { get { return Angle * 57.296d; } }

        public Real2DVector(double? maxBigness = null)
        {
            _maxBigness = maxBigness;
        }

        public Real2DVector GetScaled(double by)
        {
            var result = Copy();
            result.Set(Angle, Bigness * by);
            return result;
        }

        public void Set(double newAngle, double bigness)
        {
            if (_maxBigness.HasValue && bigness > _maxBigness.Value)
            {
                bigness = _maxBigness.Value;
            }

            //double angle = Angle;

            // cos(a) = X / bigness
            // X = cos(a) * bigness
            var newX = GetX(newAngle, bigness);
            X = newX;
            var newY = Math.Sin(newAngle) * bigness;
            Y = newY;
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

        public Real2DVector Copy()
        {
            return new Real2DVector(_maxBigness)
            {
                X = X,
                Y = Y
            };
        }

        public override string ToString()
        {
            return String.Format("{0:0.0} {1:0.0} - {2:0.0} {3:0.0}", X, Y, Bigness, Angle);
        }
    }

}
