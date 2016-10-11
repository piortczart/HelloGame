﻿using System.Collections.Generic;
using HelloGame.Common.Model;
using System.Drawing;
using System.Linq;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Model.GameObjects.Ships;
using HelloGame.Common.Physicsish;
using HelloGame.Common.Settings;
using HelloGame.Common.TimeStuffs;

namespace HelloGame.Common
{
    public class Overlay
    {
        private readonly GeneralSettings _settings;
        readonly Font _font = new Font(FontFamily.GenericMonospace, 12);
        private readonly EventPerSecond _paintCounter;
        private int _collisionCalculations;
        private int _thingsCount;
        private Position _screenCenterGeneral = new Position(0, 0);
        private Size _windowSize = Size.Empty;
        private IReadOnlyCollection<ThingBase> _things = new List<ThingBase>();
        private Point _overlayPositionGeneral = Point.Empty;

        public Overlay(TimeSource timeSource, GeneralSettings settings)
        {
            _settings = settings;
            _paintCounter = new EventPerSecond(timeSource);
        }

        internal void UpdateDuringModelUpdate(ModelManager modelManager)
        {
            _thingsCount = modelManager.ThingsThreadSafe.Count;
            _collisionCalculations = (int) modelManager.CollisionCalculations.GetPerSecond();
            _things = modelManager.ThingsThreadSafe.GetThingsReadOnly();
        }

        public void Render(Graphics graphics)
        {
            _paintCounter.Add();

            graphics.DrawString($"Paints/s: {_paintCounter.GetPerSecond()}", _font, Brushes.Black, new PointF(10, 15));
            graphics.DrawString($"Collision calc/s: {_collisionCalculations}", _font, Brushes.Black, new PointF(10, 30));
            graphics.DrawString($"Things: {_thingsCount}", _font, Brushes.Black, new PointF(10, 45));
            graphics.DrawString($"Centered at: {_screenCenterGeneral}", _font, Brushes.Black, new PointF(10, 60));

            DrawShipPointers(graphics);

            if (_settings.ShowThingsList)
            {
                int i = 0;
                foreach (ThingBase thing in _things)
                {
                    string description =
                        $"{thing.GetType().Name} D:{thing.IsDestroyed} X:{thing.ThingAdditionalInfo.Score}";
                    graphics.DrawString(description, _font, Brushes.Black,
                        new PointF(_windowSize.Width - 200, 20 + 15*i));
                    i++;
                }
            }
        }

        private void DrawShipPointers(Graphics graphics)
        {
            Point centerRelative = new Point(_windowSize.Width/2, _windowSize.Height/2);
            var shipPositions = _things.Where(t => t is ShipBase).Select(t => t.Physics.Position);
            foreach (Position shipPosition in shipPositions)
            {
                if (shipPosition.DistanceTo(_screenCenterGeneral) < 400)
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