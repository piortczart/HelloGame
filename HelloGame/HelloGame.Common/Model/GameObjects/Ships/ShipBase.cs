﻿using System;
using System.Drawing;
using System.Windows.Forms;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Settings;
using HelloGame.Common.TimeStuffs;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public abstract class ShipBase : ThingBase
    {
        protected readonly GameThingCoordinator GameCoordinator;
        public string Name { get; }
        protected readonly Limiter BombLimiter;
        protected readonly Limiter LaserLimiter;
        public readonly ShipBaseSettings ShipSettings;
        public int Score { get; set; }

        protected ShipBase(ThingBaseInjections injections, GameThingCoordinator gameCoordinator,
            ShipBaseSettings settings, string name, int? id, ThingAdditionalInfo additionalInfo = null)
            : base(injections, settings, additionalInfo, id)
        {
            GameCoordinator = gameCoordinator;
            Name = name;
            ShipSettings = settings;
            Score = additionalInfo.Score ?? 0;

            LaserLimiter = new Limiter(settings.LazerLimit, TimeSource);
            BombLimiter = new Limiter(TimeSpan.FromSeconds(1), TimeSource);

            Physics.Size = settings.Size;
            Physics.SelfPropelling = new Real2DVector(settings.MaxEnginePower);
            Physics.Interia = new Real2DVector(settings.MaxInteria);
        }

        public bool PewPew()
        {
            if (LaserLimiter.CanHappen())
            {
                Coordinator.ShootLazer(this);
                return true;
            }
            return false;
        }

        protected virtual void PaintStuffInternal(Graphics g)
        {
        }

        protected override void Render(Graphics g)
        {
            if (IsDestroyed)
            {
                var shipPen = new Pen(Brushes.Red);

                int width = MathX.Random.Next(5, 10);
                g.DrawArc(shipPen,
                    new Rectangle((int) Physics.Position.X - width/2, (int) Physics.Position.Y - width/2, width, width),
                    0, 360);
            }
            else
            {
                var shipPen = new Pen(Brushes.DarkBlue);

                // This vector will point where the ship is going.
                Point p2 = Physics.GetPointInDirection(Settingz.Size/2 + 3);
                g.DrawLine(shipPen, Physics.PositionPoint, p2);

                // This is the circle around the ship.
                g.DrawArc(shipPen,
                    new Rectangle((int) (Physics.Position.X - Physics.Size/2),
                        (int) (Physics.Position.Y - Physics.Size/2), (int) Physics.Size, (int) Physics.Size), 0, 360);

                string text = $"{Name} ({Score})";
                Size nameSize = TextRenderer.MeasureText(text, Font);
                var nameLocation = new PointF((int) Physics.Position.X - nameSize.Width/2,
                    (int) Physics.Position.Y - nameSize.Height*2);
                g.DrawString(text, Font, Brushes.Black, nameLocation);
            }

            PaintStuffInternal(g);
        }

        protected override void CollidesWithInternal(ThingBase other)
        {
            // AIs can't hurt each other.
            if (this is AiShip && other.Creator is AiShip)
            {
                return;
            }

            // Some stuff ignores planets.
            if (!Settingz.CollidesWithPlanets && other is BigMass)
            {
                return;
            }

            // Collision with a bomb?
            Bomb bomb = other as Bomb;
            if (bomb != null)
            {
                if (!bomb.IsArmed)
                {
                    return;
                }
            }

            // Collision with a Lazer?
            LazerBeamPew pew = other as LazerBeamPew;
            if (pew != null)
            {
                if (pew.Creator == this)
                {
                    return;
                }
            }

            // Collides with anything else - gets destroyed.
            Destroy(ShipSettings.DespawnTime);
        }
    }
}