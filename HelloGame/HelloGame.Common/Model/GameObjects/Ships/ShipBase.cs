﻿using System;
using System.Drawing;
using System.Windows.Forms;
using HelloGame.Common.MathStuff;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public abstract class ShipBase : ThingBase
    {
        protected readonly GameThingCoordinator GameCoordinator;
        public string Name { get; }
        protected readonly Limiter BombLimiter;
        protected readonly Limiter LaserLimiter;
        protected readonly ShipBaseSettings ShipBaseSettings;
        public int Score { get; set; }

        protected ShipBase(ThingBaseInjections injections, GameThingCoordinator gameCoordinator, ShipBaseSettings baseSettings, decimal size, string name, int? id, ThingBase creator = null, int score = 0)
            : base(injections, baseSettings, creator, id)
        {
            GameCoordinator = gameCoordinator;
            Name = name;
            ShipBaseSettings = baseSettings;
            Score = score;

            LaserLimiter = new Limiter(baseSettings.LazerLimit, TimeSource);
            BombLimiter = new Limiter(TimeSpan.FromSeconds(1), TimeSource);

            Physics.Size = size;
            Physics.SelfPropelling = new Real2DVector(baseSettings.MaxEnginePower);
            Physics.Interia = new Real2DVector(baseSettings.MaxInteria);
        }

        protected void PewPew(bool isKeyBased = false)
        {
            if (LaserLimiter.CanHappen())
            {
                var laser = new LazerBeamPew(Injections, this, -1);

                Real2DVector inertia = Physics.GetDirection(30);
                laser.Spawn(Physics.PositionPoint, inertia);
                laser.Physics.Angle = Physics.Angle;

                if (isKeyBased)
                {
                    GameCoordinator.AskServerToSpawn(laser);
                }
                else
                {
                    GameCoordinator.UpdateThing(laser);
                }
            }
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
                g.DrawArc(shipPen, new Rectangle((int)Physics.Position.X - width / 2, (int)Physics.Position.Y - width / 2, width, width), 0, 360);
            }
            else
            {
                var shipPen = new Pen(Brushes.DarkBlue);

                if (GeneralSettings.ShowPlayerPhysicsDetails && this is PlayerShip)
                {
                    g.DrawString($"Ship angle: {Physics.Angle * 57.296m:0}", Font, Brushes.Black, new PointF(155, 155));
                    g.DrawString($"Engine: {Physics.SelfPropelling.Size:0.00}", Font, Brushes.Black, new PointF(155, 185));
                    g.DrawString($"Inertia: {Physics.Interia}", Font, Brushes.Black, new PointF(155, 215));
                    g.DrawString($"Engine: {Physics.SelfPropelling}", Font, Brushes.Black, new PointF(155, 245));
                }

                // This vector will point where the ship is going.
                Point p2 = Physics.GetPointInDirection(10);
                g.DrawLine(shipPen, Physics.PositionPoint, p2);

                // This is the circle around the ship.
                g.DrawArc(shipPen, new Rectangle((int)(Physics.Position.X - Physics.Size / 2), (int)(Physics.Position.Y - Physics.Size / 2), (int)Physics.Size, (int)Physics.Size), 0, 360);

                string text =  $"{Name} ({Score})";
                Size nameSize = TextRenderer.MeasureText(text, Font);
                var nameLocation = new PointF((int)Physics.Position.X - nameSize.Width / 2, (int)Physics.Position.Y - nameSize.Height * 2);
                g.DrawString(text, Font, Brushes.Black, nameLocation);
            }

            PaintStuffInternal(g);
        }

        public override void CollidesWith(ThingBase other)
        {
            // AIs can't hurt each other.
            if (this is AiShip && other.Creator is AiShip)
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

            // Collision with a ship?
            ShipBase ship = other.Creator as ShipBase;
            if (ship != null)
            {
                ship.Score += 1;
            }

            Destroy(ShipBaseSettings.DespawnTime);
        }
    }
}
