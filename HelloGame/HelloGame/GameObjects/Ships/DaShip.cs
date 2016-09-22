using System;
using System.Drawing;
using HelloGame.MathStuff;

namespace HelloGame.GameObjects.Ships
{
    public abstract class DaShip : ThingBase
    {
        private static readonly ThingSettings Settings = new ThingSettings
        {
            Aerodynamism = 0.1m,
            TimeToLive = TimeSpan.Zero,
            Mass = 3,
            RadPerSecond = (decimal)Math.PI
        };

        protected readonly GameState Scene;
        protected readonly Limiter BombLimiter = new Limiter(TimeSpan.FromSeconds(1));
        protected readonly Limiter LaserLimiter = new Limiter(TimeSpan.FromMilliseconds(200));
        protected readonly Font Font = new Font("Courier", 24, GraphicsUnit.Pixel);

        protected DaShip(GameState scene, decimal size) : base(Settings)
        {
            Scene = scene;

            Physics.Size = size;
            Physics.SelfPropelling = new Real2DVector(0.5m);
            Physics.Interia = new Real2DVector(5);
        }

        protected void PewPew()
        {
            if (LaserLimiter.CanHappen())
            {
                var laser = new LazerBeamPew(this);

                Real2DVector inertia = Physics.GetDirection(20);
                laser.Spawn(Physics.PositionPoint, inertia);
                laser.Physics.Angle = Physics.Angle;

                Scene.AddThing(laser);
            }
        }

        protected virtual void PaintStuffInternal(Graphics g)
        {
        }

        public override void PaintStuff(Graphics g)
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

                if (this is PlayerShip)
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
            }

            PaintStuffInternal(g);
        }

        public override void CollidesWith(ThingBase other)
        {
            if (other is Bomb)
            {
                var bomb = (Bomb)other;
                if (!bomb.IsArmed)
                {
                    return;
                }
            }
            if (other is LazerBeamPew)
            {
                if (other.Creator == this)
                {
                    return;
                }
            }

            Destroy(TimeSpan.FromSeconds(3));
        }
    }
}
