using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloGame
{
    public class ThingForce : IThing
    {
        Vector engineVector = new Real2DVector(1);

        Real2DVector inertia = new Real2DVector(5);

        ThingModel model = new ThingModel();

        private KeysInfo _keysInfo;

        Stopwatch stopwatch = Stopwatch.StartNew();
        long lastUpdate = 0;

        private List<Real2DVector> _forces = new List<Real2DVector>();

        public ThingForce(KeysInfo keysInfo, Point startingPoint)
        {
            _keysInfo = keysInfo;
            model.PositionX = startingPoint.X;
            model.PositionY = startingPoint.Y;
            _forces.Add(inertia);
            _forces.Add(engineVector);
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
            }
            else if (keysInfo.IsD)
            {
                shipAngle += 0.1;
            }

            return shipAngle;
        }

        public void UpdateModel()
        {
            long now = stopwatch.ElapsedTicks;
            if (lastUpdate == 0)
            {
                lastUpdate = now;
            }
            long ticksSinceLast = now - lastUpdate;
            double secSinceLast = TimeSpan.FromTicks(ticksSinceLast).TotalSeconds;

            model.ShipAngle = GetUpdatedShipAngle(_keysInfo, model.ShipAngle);

            UpdateEngineAcc(engineVector, model.ShipAngle, _keysInfo);

            // Inercja (bezwładność) F = m * a
            // a - przyspieszenie (wektorowe)
            inertia.Add(engineVector);

            Real2DVector totalForce = new Real2DVector();
            foreach (Real2DVector force in _forces)
            {
                totalForce.Add(force);
            }

            model.PositionX += totalForce.X / 10;
            model.PositionY += totalForce.Y / 10;
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

        public void PaintStuff(Graphics g)
        {
            var font = new Font("Courier", 24, GraphicsUnit.Pixel);
            var textBrush = Brushes.Black;
            var shipPen = new Pen(Brushes.DarkBlue);

            g.DrawString(String.Format("Ship angle: {0:0}", model.ShipAngle * 57.296d), font, Brushes.Black, new PointF(155, 155));
            g.DrawString(String.Format("Engine: {0:0.00}", engineVector.Bigness), font, Brushes.Black, new PointF(155, 185));
            g.DrawString(String.Format("Inertia: {0}", inertia), font, Brushes.Black, new PointF(155, 215));
            g.DrawString(String.Format("Engine: {0:0.00} x {1:0.00} {2:0.00}", engineVector.X, engineVector.Y, engineVector.Bigness), font, Brushes.Black, new PointF(155, 245));


            // This vector will point where the ship is going.
            Real2DVector v = new Real2DVector();
            v.Change(model.ShipAngle, 10);
            Point p2 = new Point((int)(v.X + model.PositionX), (int)(v.Y + model.PositionY));
            g.DrawLine(shipPen, model.PositionPoint, p2);

            // This is the circle around the ship.
            int width = 20;
            g.DrawArc(shipPen, new Rectangle((int)model.PositionX - width / 2, (int)model.PositionY - width / 2, width, width), 0, 360);
        }
    }

}
