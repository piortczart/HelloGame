using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Settings;
using HelloGame.Common.TimeStuffs;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public class AiShip : ShipBase
    {
        private readonly Limiter _locatePlayerLimiter;
        private Real2DVector _playerPointer = new Real2DVector();
        private readonly AiShipSettings _aiShipBaseSettings;
        public AiType AiType { get; set; }
        public ShipSettingType ShipSettingType { get; set; }

        public AiShip(ThingBaseInjections injections, GameThingCoordinator coordinator, AiType aiType,
            ShipSettingType shipSettingType, string name, ThingAdditionalInfo additionalInfo, int? id = null,
            ElapsingThingSettings elapsingThingSettings = null)
            : base(
                injections, ShipBaseSettings.GetShipTypeSettings(shipSettingType, elapsingThingSettings, typeof(AiShip)),
                name, id, additionalInfo)
        {
            _aiShipBaseSettings = AiShipSettings.AiSettings[aiType];
            _locatePlayerLimiter = new Limiter(_aiShipBaseSettings.LocatePlayerFrequency, TimeSource);
            AiType = aiType;
            ShipSettingType = shipSettingType;
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
                if (_locatePlayerLimiter.CanHappen() && !player.IsDestroyed && !player.IsTimeToElapse)
                {
                    // Locate a player's ship.

                    // Face him. (change the angle)
                    decimal x = player.Physics.Position.X - Physics.Position.X;
                    decimal y = player.Physics.Position.Y - Physics.Position.Y;
                    _playerPointer = Real2DVector.GetFromCoords(x, y);

                    Physics.Angle = _playerPointer.Angle;

                    if (Settings.IsAiHostile)
                    {
                        PewPew(0);
                    }
                }

                // Player is far away? Get closer.
                if (player.Physics.Position.DistanceTo(Physics.Position) > _aiShipBaseSettings.DistanceToPlayer)
                {
                    Physics.SelfPropelling.Change(Physics.Angle, ShipSettings.MaxEnginePower);
                }
                // Player too close? Go back.
                else
                {
                    Physics.SelfPropelling.Change(Physics.Angle, ShipSettings.MaxEnginePower);
                }
            }
        }
    }
}