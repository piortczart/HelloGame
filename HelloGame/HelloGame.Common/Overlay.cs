using System.Collections.Generic;
using HelloGame.Common.Model;
using System.Drawing;
using System.Linq;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Model.GameObjects.Ships;
using HelloGame.Common.Physicsish;
using HelloGame.Common.TimeStuffs;

namespace HelloGame.Common
{
    public class Overlay
    {
        readonly Font _font = new Font(FontFamily.GenericMonospace, 12);
        private readonly EventPerSecond _paintCounter;
        private int _collisionCalculations;
        private int _thingsCount;
        private Position _screenCenterGeneral = new Position(0, 0);
        private Size _windowSize = Size.Empty;
        private List<Position> _shipPositionsGeneral = new List<Position>();
        private Point _overlayPositionGeneral = Point.Empty;

        private PlayerShipMovable _shipRemoveMe = null;
        private Queue<bool?> _isDestroyeds = new Queue<bool?>();

        public Overlay(TimeSource timeSource)
        {
            _paintCounter = new EventPerSecond(timeSource);
        }

        internal void UpdateDuringModelUpdate(ModelManager modelManager)
        {
            _thingsCount = modelManager.ThingsThreadSafe.Count;
            _collisionCalculations = (int) modelManager.CollisionCalculations.GetPerSecond();
            _shipPositionsGeneral =
                modelManager.ThingsThreadSafe.GetThingsReadOnly()
                    .Where(t => t is ShipBase)
                    .Select(t => t.Physics.Position)
                    .ToList();

            _shipRemoveMe = (PlayerShipMovable) modelManager.ThingsThreadSafe.GetThingsReadOnly().Where(t=> t is PlayerShipMovable).FirstOrDefault();
        }

        public void Render(Graphics graphics)
        {
            _paintCounter.Add();

            graphics.DrawString($"Paints/s: {_paintCounter.GetPerSecond()}", _font, Brushes.Black, new PointF(10, 15));
            graphics.DrawString($"Collision calc/s: {_collisionCalculations}", _font, Brushes.Black, new PointF(10, 30));
            graphics.DrawString($"Things: {_thingsCount}", _font, Brushes.Black, new PointF(10, 45));
            graphics.DrawString($"Centered at: {_screenCenterGeneral}", _font, Brushes.Black, new PointF(10, 60));

            _isDestroyeds.Enqueue(_shipRemoveMe?.IsDestroyed);
            if (_isDestroyeds.Count > 10)
            {
                _isDestroyeds.Dequeue();
            }
            graphics.DrawString($"{string.Join(",", _isDestroyeds.ToList())}", _font, Brushes.Black, new PointF(10, 75));

            DrawShipPointers(graphics);
        }

        private void DrawShipPointers(Graphics graphics)
        {
            Point centerRelative = new Point(_windowSize.Width/2, _windowSize.Height/2);
            foreach (Position shipPosition in _shipPositionsGeneral)
            {
                if (shipPosition.DistanceTo(_screenCenterGeneral) < 100)
                {
                    continue;
                }

                decimal x = shipPosition.X - _screenCenterGeneral.X;
                decimal y = shipPosition.Y - _screenCenterGeneral.Y;
                var vectorA = new Real2DVector {X = x, Y = y};
                vectorA.Set(vectorA.Angle, 40);

                var vectorB = new Real2DVector {X = x, Y = y};
                vectorB.Set(vectorB.Angle, 50);

                graphics.DrawLine(new Pen(Color.Black),
                    AddPoints(centerRelative, vectorA.Point),
                    AddPoints(centerRelative, vectorB.Point));
            }
        }

        private static Point AddPoints(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        private static Point MovePointGeneralToLocal(Point pointGeneral, Point localViewGeneralCoords)
        {
            return new Point(pointGeneral.X - localViewGeneralCoords.X, pointGeneral.Y - localViewGeneralCoords.Y);
        }

        public void UpdatePositions(Position screenCenter, Size windowSize, Point overlayPosition)
        {
            _screenCenterGeneral = screenCenter;
            _windowSize = windowSize;
            _overlayPositionGeneral = overlayPosition;
        }
    }
}