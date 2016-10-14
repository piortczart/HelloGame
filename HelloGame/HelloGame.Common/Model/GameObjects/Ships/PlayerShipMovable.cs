using System;
using HelloGame.Common.MathStuff;
using System.Drawing;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public class PlayerShipMovable : PlayerShip
    {
        public KeysInfo KeysInfo { private get; set; }

        public PlayerShipMovable(ThingBaseInjections injections, GameThingCoordinator coordinator, string name,
            ClanEnum clan, int? id = null, ThingAdditionalInfo additionalInfo = null,
            ElapsingThingSettings elapsingThingSettings = null)
            : base(injections, coordinator, name, clan, id, additionalInfo, elapsingThingSettings)
        {
            // TODO: REDO THIS
            // Just for now. They will be reset for the proper ones.
            KeysInfo = new KeysInfo();
        }

        protected override void PaintStuffInternal(Graphics g)
        {
            base.PaintStuffInternal(g);

            if (Settings.ShowPlayerPhysicsDetails)
            {
                g.DrawString($"Ship angle: {Physics.Angle*57.296f:0}", Font, Brushes.Black, new PointF(155, 155));
                g.DrawString($"Engine: {Physics.SelfPropelling.Size:0.00}", Font, Brushes.Black, new PointF(155, 185));
                g.DrawString($"Inertia: {Physics.Interia}", Font, Brushes.Black, new PointF(155, 215));
                g.DrawString($"Engine: {Physics.SelfPropelling}", Font, Brushes.Black, new PointF(155, 245));
            }
        }

        private float GetUpdatedShipAngle(float shipAngle, TimeSpan timeSinceLastUpdate)
        {
            float maxAngleChange = Physics.RadPerSecond*(float) timeSinceLastUpdate.TotalSeconds;

            if (KeysInfo.IsA && KeysInfo.IsD)
            {
                return shipAngle;
            }

            if (KeysInfo.IsA)
            {
                shipAngle -= maxAngleChange;
                if (shipAngle < 0)
                {
                    shipAngle = 2*(float) Math.PI - shipAngle;
                }
            }
            else if (KeysInfo.IsD)
            {
                shipAngle += maxAngleChange;
                if (shipAngle > 2*(float) Math.PI)
                {
                    shipAngle -= 2*(float) Math.PI;
                }
            }
            return shipAngle;
        }

        protected override void Umi(TimeSpan timeSinceLastUpdate)
        {
            Physics.Angle = GetUpdatedShipAngle(Physics.Angle, timeSinceLastUpdate);

            UpdateEngineAcc(Physics.SelfPropelling, Physics.Angle, KeysInfo);

            if (KeysInfo.IsJ)
            {
                PewPew(1);
            }

            if (KeysInfo.IsSpace)
            {
                PewPew(0);
            }
        }

        private static void UpdateEngineAcc(Vector2D engineForce, float shipAngle, KeysInfo keys)
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
                engineForce.Change(shipAngle, -5f);
            }
        }
    }
}