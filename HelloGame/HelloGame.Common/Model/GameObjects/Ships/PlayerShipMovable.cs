﻿using System;
using HelloGame.Common.Logging;
using HelloGame.Common.MathStuff;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public class PlayerShipMovable : PlayerShip
    {
        public KeysInfo KeysInfo { private get; set; }

        private static readonly ThingSettings Settings = new ThingSettings
        {
            Aerodynamism = 0.1m,
            TimeToLive = TimeSpan.Zero,
            Mass = 3,
            RadPerSecond = (decimal)Math.PI
        };

        public PlayerShipMovable(ILogger logger, GameThingCoordinator gameManager, string name, decimal size = 10, int? id = null, ThingBase creator = null) 
            : base(logger, gameManager, Settings, name, size, id, creator)
        {
            // TODO: REDO THIS
            // Just for now. They will be reset for the proper ones.
            KeysInfo = new KeysInfo();
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
                    var bomb = new Bomb(Logger, this);
                    bomb.Spawn(Physics.GetPointInDirection(10), Physics.TotalForce.GetScaled(1.2m, false));

                    GameCoordinator.UpdateThing(bomb);
                }
            }

            if (KeysInfo.IsSpace)
            {
                PewPew(true);
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