using System;
using System.Drawing;
using System.Windows.Forms;
using HelloGame.Common.Logging;
using HelloGame.Common.MathStuff;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public abstract class ShipBase : ThingBase
    {
        private readonly ILogger _logger;
        protected readonly GameThingCoordinator GameCoordinator;
        public string Name { get; }
        protected readonly Limiter BombLimiter = new Limiter(TimeSpan.FromSeconds(1));
        protected readonly Limiter LaserLimiter;

        protected ShipBase(ILogger logger, GameThingCoordinator gameCoordinator, ThingSettings settings, decimal size, string name, int? id, ThingBase creator = null)
            : base(logger, settings, creator, id)
        {
            _logger = logger;
            GameCoordinator = gameCoordinator;
            Name = name;

            LaserLimiter = new Limiter(settings.LazerLimit);

            Physics.Size = size;
            Physics.SelfPropelling = new Real2DVector(5m);
            Physics.Interia = new Real2DVector(5);
        }

        protected void PewPew(bool isKeyBased = false)
        {
            if (LaserLimiter.CanHappen())
            {
                // 
                var laser = new LazerBeamPew(_logger, this, -1);

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

                Size nameSize = TextRenderer.MeasureText(Name, Font);
                var nameLocation = new PointF((int)Physics.Position.X - nameSize.Width / 2, (int)Physics.Position.Y - nameSize.Height * 2);
                g.DrawString(Name, Font, Brushes.Black, nameLocation);
            }

            PaintStuffInternal(g);
        }

        public override void CollidesWith(ThingBase other)
        {
            if (this is AiShip && other.Creator is AiShip)
            {
                return;
            }

            Bomb bomb = other as Bomb;
            if (bomb != null)
            {
                if (!bomb.IsArmed)
                {
                    return;
                }
            }

            LazerBeamPew pew = other as LazerBeamPew;
            if (pew != null)
            {
                if (pew.Creator == this)
                {
                    return;
                }
            }

            Destroy(TimeSpan.FromSeconds(3));
        }
    }
}
