using System;
using System.Diagnostics;
using System.Drawing;

namespace HelloGame
{
    public class Limiter
    {
        private static Stopwatch stopwatch = Stopwatch.StartNew();
        TimeSpan lastEvent = TimeSpan.Zero;
        TimeSpan _frequency;

        public Limiter(TimeSpan frequency)
        {
            _frequency = frequency;
        }

        public bool CanHappen()
        {
            TimeSpan nextEvent = lastEvent.Add(_frequency);
            if (stopwatch.Elapsed > nextEvent)
            {
                lastEvent = stopwatch.Elapsed;
                return true;
            }
            return false;
        }
    }

    public class ThingForce : AThing
    {
        public KeysInfo KeysInfo { get; set; }

        Stopwatch stopwatch = Stopwatch.StartNew();
        long lastUpdate = 0;
        Scene _scene;

        Limiter bombLimiter = new Limiter(TimeSpan.FromSeconds(1));
        Limiter laserLimiter = new Limiter(TimeSpan.FromMilliseconds(200));

        public ThingForce(KeysInfo keysInfo, Scene scene) : base(TimeSpan.Zero)
        {
            KeysInfo = keysInfo;
            _scene = scene;

            Physics.SelfPropelling = new Real2DVector(1);
            Physics.Interia = new Real2DVector(5);
            Physics.Drag = new Real2DVector();
        }

        private static double GetUpdatedShipAngle(KeysInfo keysInfo, double shipAngle)
        {
            if (keysInfo.IsA && keysInfo.IsD)
            {
                return shipAngle;
            }

            if (keysInfo.IsA)
            {
                shipAngle -= 0.1;
                if (shipAngle < 0)
                {
                    shipAngle = 2 * Math.PI - shipAngle;
                }
            }
            else if (keysInfo.IsD)
            {
                shipAngle += 0.1;
                if (shipAngle > 2 * Math.PI)
                {
                    shipAngle -= 2*Math.PI;
                }
            }

            return shipAngle;
        }

        public override void UpdateModelInternal()
        {
            long now = stopwatch.ElapsedTicks;
            if (lastUpdate == 0)
            {
                lastUpdate = now;
            }
            long ticksSinceLast = now - lastUpdate;
            double secSinceLast = TimeSpan.FromTicks(ticksSinceLast).TotalSeconds;

            Model.ShipAngle = GetUpdatedShipAngle(KeysInfo, Model.ShipAngle);

            UpdateEngineAcc(Physics.SelfPropelling, Model.ShipAngle, KeysInfo);

            // Inercja (bezwładność) F = m * a
            // a - przyspieszenie (wektorowe)
            Physics.Interia.Add(Physics.SelfPropelling);

            // Drag changes the inertia?
            Physics.Drag = Physics.Interia.GetOpposite().GetScaled(0.01);

            Physics.Interia.Add(Physics.Drag);

            Real2DVector totalForce = Physics.TotalForce;
            Model.PositionX += totalForce.X / 10;
            Model.PositionY += totalForce.Y / 10;

            if (KeysInfo.IsJ)
            {
                if (bombLimiter.CanHappen())
                {
                    var bomb = new Projectile(KeysInfo, this);
                    bomb.Spawn(Model.PositionPoint, Physics.Interia.GetScaled(1.1, false));

                    _scene.AddThing(bomb);
                }
            }

            if (KeysInfo.IsSpace)
            {
                if (laserLimiter.CanHappen())
                {
                    var laser = new LazerBeamPew();

                    Real2DVector inertia = Model.GetDirection(40);

                    laser.Spawn(Model.PositionPoint, inertia);

                    _scene.AddThing(laser);
                }
            }
        }

        private static void UpdateEngineAcc(Real2DVector engineForce, double shipAngle, KeysInfo keys)
        {
            // Nothing is pressed.
            if ((keys.IsW && keys.IsS) || (!keys.IsW && !keys.IsS))
            {
                engineForce.Set(0, 0);
            }
            else if (keys.IsW) // W is pressed
            {
                engineForce.Change(shipAngle, 5);
            }
            else if (keys.IsS) // S is pressed
            {
                engineForce.Change(shipAngle, -1.5);
            }
        }

        public override void PaintStuff(Graphics g)
        {
            var font = new Font("Courier", 24, GraphicsUnit.Pixel);
            var textBrush = Brushes.Black;
            var shipPen = new Pen(Brushes.DarkBlue);

            g.DrawString(String.Format("Ship angle: {0:0}", Model.ShipAngle * 57.296d), font, Brushes.Black, new PointF(155, 155));
            g.DrawString(String.Format("Engine: {0:0.00}", Physics.SelfPropelling.Bigness), font, Brushes.Black, new PointF(155, 185));
            g.DrawString(String.Format("Inertia: {0}", Physics.Interia), font, Brushes.Black, new PointF(155, 215));
            g.DrawString(String.Format("Engine: {0:}", Physics.SelfPropelling), font, Brushes.Black, new PointF(155, 245));

            // This vector will point where the ship is going.
            //Real2DVector direction = Model.GetDirection(10);
            //Point p2 = new Point((int)(direction.X + Model.PositionX), (int)(direction.Y + Model.PositionY));
            Point p2 = Model.GetPointInDirection(10);
            g.DrawLine(shipPen, Model.PositionPoint, p2);

            // This is the circle around the ship.
            int width = 20;
            g.DrawArc(shipPen, new Rectangle((int)Model.PositionX - width / 2, (int)Model.PositionY - width / 2, width, width), 0, 360);
        }
    }
}
