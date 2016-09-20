using System;
using System.Drawing;
using HelloGame.MathStuff;

namespace HelloGame.GameObjects
{
    public abstract class DaShip : ThingBase
    {
        private static readonly ThingSettings settings = new ThingSettings
        {
            Aerodynamism = 0.1m,
            TimeToLive = TimeSpan.Zero,
            Mass = 3,
            RadPerSecond = (decimal)Math.PI
        };

        protected readonly GameState _scene;
        protected readonly Limiter _bombLimiter = new Limiter(TimeSpan.FromSeconds(1));
        protected readonly Limiter _laserLimiter = new Limiter(TimeSpan.FromMilliseconds(200));

        protected DaShip(GameState scene) : base(settings)
        {
            _scene = scene;

            Physics.SelfPropelling = new Real2DVector(0.5m);
            Physics.Interia = new Real2DVector(5);
        }

        protected void PewPew()
        {
            if (_laserLimiter.CanHappen())
            {
                var laser = new LazerBeamPew(this);

                Real2DVector inertia = Physics.GetDirection(20);
                laser.Spawn(Physics.PositionPoint, inertia);
                laser.Physics.Angle = Physics.Angle;

                _scene.AddThing(laser);
            }
        }

        public override void PaintStuff(Graphics g)
        {
            if (IsDestroyed)
            {
                var shipPen = new Pen(Brushes.Red);

                int width = MathX.Random.Next(5, 10);
                g.DrawArc(shipPen, new Rectangle((int)Physics.PositionX - width / 2, (int)Physics.PositionY - width / 2, width, width), 0, 360);
            }
            else
            {
                var font = new Font("Courier", 24, GraphicsUnit.Pixel);
                var shipPen = new Pen(Brushes.DarkBlue);

                if (this is PlayerShip)
                {
                    g.DrawString($"Ship angle: {Physics.Angle * 57.296m:0}", font, Brushes.Black, new PointF(155, 155));
                    g.DrawString($"Engine: {Physics.SelfPropelling.Bigness:0.00}", font, Brushes.Black, new PointF(155, 185));
                    g.DrawString($"Inertia: {Physics.Interia}", font, Brushes.Black, new PointF(155, 215));
                    g.DrawString($"Engine: {Physics.SelfPropelling}", font, Brushes.Black, new PointF(155, 245));
                }

                // This vector will point where the ship is going.
                Point p2 = Physics.GetPointInDirection(10);
                g.DrawLine(shipPen, Physics.PositionPoint, p2);

                // This is the circle around the ship.
                int width = 20;
                g.DrawArc(shipPen, new Rectangle((int)Physics.PositionX - width / 2, (int)Physics.PositionY - width / 2, width, width), 0, 360);
            }
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

            Destroy();
            ElapseIn(TimeSpan.FromSeconds(5));
        }
    }
}
