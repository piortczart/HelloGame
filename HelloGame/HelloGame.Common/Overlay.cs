﻿using System;
using System.Collections.Generic;
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
        private readonly DisplayedFadingOutTexts _displayTexts;

        public Overlay(TimeSource timeSource, GeneralSettings settings)
        {
            _settings = settings;
            _paintCounter = new EventPerSecond(timeSource);
            _displayTexts = new DisplayedFadingOutTexts(timeSource);
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

            ShowShipList(graphics);

            int i = 0;
            foreach (DisplayedFadingOutText text in _displayTexts.GetCurrent())
            {
                PointF position = new PointF(30, _windowSize.Height - 100 - 15*i);
                graphics.DrawString($"{text.Text}", _font, Brushes.Black, position);
                i++;
            }

            i = 0;
            foreach (DisplayedFadingOutText text in _displayTexts.GetCurrent(true))
            {
                Font f = new Font(_font, FontStyle.Bold);
                PointF position = new PointF(_windowSize.Width/2 - 100, 200 - 15*i);
                graphics.DrawString($"{text.Text}", f, Brushes.Black, position);
                i++;
            }
        }

        private void ShowShipList(Graphics graphics)
        {
            if (_settings.ShowShipList)
            {
                int i = 0;
                foreach (
                    ShipBase ship in
                        _things.Where(t => t is ShipBase)
                            .OrderByDescending(t => t.ThingAdditionalInfo.Score)
                            .Cast<ShipBase>())
                {
                    string name = ship.Name; //thing.GetType().Name.SubstringSafe(0, 10);
                    if (ship is AiShip)
                    {
                        name += " [AI]";
                    }
                    string description =
                        $"{name} - {ship.ThingAdditionalInfo.Score}";
                    graphics.DrawString(description, _font, Brushes.Black,
                        new PointF(_windowSize.Width - 300, 20 + 15*i));
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

                float x = shipPosition.X - _screenCenterGeneral.X;
                float y = shipPosition.Y - _screenCenterGeneral.Y;
                var vectorA = Vector2D.GetFromCoords(x, y);
                vectorA.Set(vectorA.Angle, 40);

                var vectorB = Vector2D.GetFromCoords(x, y);
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
        }

        public void AddDisplayText(string text, TimeSpan timeToLive, bool big = false)
        {
            _displayTexts.Add(text, timeToLive, big);
        }
    }
}