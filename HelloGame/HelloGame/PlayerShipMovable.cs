using System;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Model;
using HelloGame.Common.Model.GameObjects;
using HelloGame.Common.Model.GameObjects.Ships;

namespace HelloGame
{
    public class PlayerShipMovable : PlayerShip
    {
        public KeysInfo KeysInfo { private get; set; }

        public PlayerShipMovable(ModelManager modelManager, decimal size = 10) : base(modelManager, size)
        {
        }

        private decimal GetUpdatedShipAngle(decimal shipAngle, TimeSpan timeSinceLastUpdate)
        {
            decimal maxAngleChange = Physics.RadPerSecond * (decimal)timeSinceLastUpdate.TotalSeconds;

            if (KeysInfo.IsA && KeysInfo.IsD)
            {
                return shipAngle;
            }

            if (KeysInfo.IsA)
            {
                shipAngle -= maxAngleChange;
                if (shipAngle < 0)
                {
                    shipAngle = 2 * (decimal)Math.PI - shipAngle;
                }
            }
            else if (KeysInfo.IsD)
            {
                shipAngle += maxAngleChange;
                if (shipAngle > 2 * (decimal)Math.PI)
                {
                    shipAngle -= 2 * (decimal)Math.PI;
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
                if (BombLimiter.CanHappen())
                {
                    var bomb = new Bomb(this);
                    bomb.Spawn(Physics.GetPointInDirection(10), Physics.TotalForce.GetScaled(1.2m, false));

                    ModelManager.AddThing(bomb);
                }
            }

            if (KeysInfo.IsSpace)
            {
                PewPew();
            }
        }


        private static void UpdateEngineAcc(Real2DVector engineForce, decimal shipAngle, KeysInfo keys)
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
                engineForce.Change(shipAngle, -0.5m);
            }
        }

    }
}