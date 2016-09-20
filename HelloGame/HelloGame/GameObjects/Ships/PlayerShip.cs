using HelloGame.MathStuff;
using System;
using System.Collections.Generic;

namespace HelloGame.GameObjects
{
    public class PlayerShip : DaShip
    {
        private KeysInfo _keysInfo { get; set; }

        public PlayerShip(KeysInfo keysInfo, GameState scene) : base(scene)
        {
            _keysInfo = keysInfo;
        }

        private decimal GetUpdatedShipAngle(decimal shipAngle, TimeSpan timeSinceLastUpdate)
        {
            decimal maxAngleChange = Physics.RadPerSecond * (decimal)timeSinceLastUpdate.TotalSeconds;

            if (_keysInfo.IsA && _keysInfo.IsD)
            {
                return shipAngle;
            }

            if (_keysInfo.IsA)
            {
                shipAngle -= maxAngleChange;
                if (shipAngle < 0)
                {
                    shipAngle = 2 * (decimal)Math.PI - shipAngle;
                }
            }
            else if (_keysInfo.IsD)
            {
                shipAngle += maxAngleChange;
                if (shipAngle > 2 * (decimal)Math.PI)
                {
                    shipAngle -= 2 * (decimal)Math.PI;
                }
            }
            return shipAngle;
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, List<ThingBase> otherThings)
        {
            if (IsDestroyed)
            {
                return;
            }

            Physics.Angle = GetUpdatedShipAngle(Physics.Angle, timeSinceLastUpdate);

            UpdateEngineAcc(Physics.SelfPropelling, Physics.Angle, _keysInfo);

            if (_keysInfo.IsJ)
            {
                if (_bombLimiter.CanHappen())
                {
                    var bomb = new Bomb(this);
                    bomb.Spawn(Physics.GetPointInDirection(10), Physics.TotalForce.GetScaled(1.2m, false));

                    _scene.AddThing(bomb);
                }
            }

            if (_keysInfo.IsSpace)
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
                engineForce.Change(shipAngle, -1.5m);
            }
        }
    }

}
