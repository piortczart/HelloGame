using System;
using HelloGame.Common.Extensions;
using HelloGame.Common.Model.GameObjects;
using HelloGame.Common.Model.GameObjects.Ships;
using HelloGame.Common.Physicsish;

namespace HelloGame.Common.Model
{
    [Serializable]
    public class ThingDescription
    {
        /**
         * Getters cannot be made private! Needed for deserialization!
         */

        public AlmostPhysics AlmostPhysics { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }
        public object[] ConstructParams { get; set; }

        // Needed for deserialization.
        // ReSharper disable once MemberCanBePrivate.Global
        public ThingDescription()
        {
        }

        public ThingDescription(ThingBase thingBase, bool isHisShip)
        {
            AlmostPhysics = thingBase.Physics;
            Id = thingBase.Id;
            PlayerShip player = thingBase as PlayerShip;
            if (player != null)
            {
                Type = isHisShip ? "PlayerShipMovable" : "PlayerShipAny";
                ConstructParams = new object[]
                {
                    player.Name,
                    player.ThingAdditionalInfo.SerializeJson(),
                    (int) player.Clan,
                    thingBase.ElapsingSettings.SerializeJson()
                };
                return;
            }

            var big = thingBase as BigMass;
            if (big != null)
            {
                Type = big.GetType().Name;
                ConstructParams = new object[]
                {
                    big.Physics.Size,
                    big.Color.SerializeJson(),
                    big.ThingAdditionalInfo.SerializeJson(),
                    thingBase.ElapsingSettings.SerializeJson()
                };
                return;
            }

            var ai = thingBase as AiShip;
            if (ai != null)
            {
                Type = ai.GetType().Name;
                ConstructParams = new object[]
                {
                    ai.Name,
                    ai.ThingAdditionalInfo.SerializeJson(),
                    ai.AiType,
                    ai.ShipSettingType,
                    thingBase.ElapsingSettings.SerializeJson()
                };
                return;
            }

            var lazer = thingBase as LazerBeamPew;
            if (lazer != null)
            {
                Type = lazer.GetType().Name;
                ConstructParams = new object[]
                {
                    lazer.ThingAdditionalInfo.SerializeJson(),
                    thingBase.ElapsingSettings.SerializeJson()
                };
                return;
            }

            var bomb = thingBase as Bomb;
            if (bomb != null)
            {
                Type = bomb.GetType().Name;
                ConstructParams = new object[]
                {
                    bomb.ThingAdditionalInfo.SerializeJson(),
                    thingBase.ElapsingSettings.SerializeJson()
                };
                return;
            }

            throw new NotImplementedException($"Cannot create type: {thingBase.GetType().Name}");
        }
    }
}