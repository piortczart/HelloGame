using System;
using System.Diagnostics;
using System.Drawing;
using HelloGame.MathStuff;

namespace HelloGame.GameObjects
{
    public class DaShip : ThingBase
    {
        public KeysInfo KeysInfo { get; set; }

        readonly Scene _scene;

        readonly Limiter _bombLimiter = new Limiter(TimeSpan.FromSeconds(1));
        readonly Limiter _laserLimiter = new Limiter(TimeSpan.FromMilliseconds(200));

        public DaShip(KeysInfo keysInfo, Scene scene) : base(TimeSpan.Zero)
        {
            KeysInfo = keysInfo;
            _scene = scene;

            Physics.SelfPropelling = new Real2DVector(1);
            Physics.Interia = new Real2DVector(5);

            Physics.Aerodynamism = 0.5m;
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
                    shipAngle -= 2 * Math.PI;
                }
            }

            return shipAngle;
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate)
        {
            Model.ShipAngle = GetUpdatedShipAngle(KeysInfo, Model.ShipAngle);

            UpdateEngineAcc(Physics.SelfPropelling, Model.ShipAngle, KeysInfo);

            if (KeysInfo.IsJ)
            {
                if (_bombLimiter.CanHappen())
                {
                    var bomb = new Bomb();
                    bomb.Spawn(Model.PositionPoint, Physics.Interia.GetScaled(1.1m, false));

                    _scene.AddThing(bomb);
                }
            }

            if (KeysInfo.IsSpace)
            {
                if (_laserLimiter.CanHappen())
                {
                    var laser = new LazerBeamPew();

                    Real2DVector inertia = Model.GetDirection(40);
                    laser.Spawn(Model.PositionPoint, inertia);
                    laser.Model.ShipAngle = Model.ShipAngle;

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
            var shipPen = new Pen(Brushes.DarkBlue);

            g.DrawString($"Ship angle: {Model.ShipAngle * 57.296d:0}", font, Brushes.Black, new PointF(155, 155));
            g.DrawString($"Engine: {Physics.SelfPropelling.Bigness:0.00}", font, Brushes.Black, new PointF(155, 185));
            g.DrawString($"Inertia: {Physics.Interia}", font, Brushes.Black, new PointF(155, 215));
            g.DrawString($"Engine: {Physics.SelfPropelling}", font, Brushes.Black, new PointF(155, 245));

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
