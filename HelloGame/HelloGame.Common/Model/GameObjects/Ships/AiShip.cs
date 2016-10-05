using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using HelloGame.Common.MathStuff;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public class AiShip : ShipBase
    {
        private readonly Limiter _locatePlayerLimiter;
        private Real2DVector _playerPointer = new Real2DVector();
        private readonly AiShipBaseSettings _aiShipBaseSettings;

        public AiShip(ThingBaseInjections injections, GameThingCoordinator coordinator, string name, int? id = null, ThingBase creator = null)
            : base(injections, coordinator, injections.GeneralSettings.AiShipBaseSettings, name, id, creator)
        {
            _aiShipBaseSettings = injections.GeneralSettings.AiShipBaseSettings;
            _locatePlayerLimiter = new Limiter(_aiShipBaseSettings.LocatePlayerFrequency, TimeSource);
        }

        protected override void PaintStuffInternal(Graphics g)
        {
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, IEnumerable<ThingBase> otherThings)
        {
            if (IsDestroyed)
            {
                return;
            }

            ThingBase player = otherThings.FirstOrDefault(s => s is PlayerShip);
            if (player != null)
            {
                if (_locatePlayerLimiter.CanHappen())
                {
                    // Locate a player's ship.

                    // Face him. (change the angle)
                    decimal x = player.Physics.Position.X - Physics.Position.X;
                    decimal y = player.Physics.Position.Y - Physics.Position.Y;
                    _playerPointer = Real2DVector.GetFromCoords(x, y);

                    Physics.Angle = _playerPointer.Angle;

                    if (GeneralSettings.IsAiHostile)
                    {
                        PewPew();
                    }
                }

                // Player is far away? Get closer.
                if (player.Physics.Position.DistanceTo(Physics.Position) > _aiShipBaseSettings.DistanceToPlayer)
                {
                    Physics.SelfPropelling.Change(Physics.Angle, _aiShipBaseSettings.MaxEnginePower);
                }
                // Player too close? Go back.
                else
                {
                    Physics.SelfPropelling.Change(Physics.Angle, _aiShipBaseSettings.MaxEnginePower);
                }
            }
        }
    }
}
