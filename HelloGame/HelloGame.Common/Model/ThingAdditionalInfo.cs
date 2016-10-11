using System;
using HelloGame.Common.Extensions;
using Newtonsoft.Json;

namespace HelloGame.Common.Model
{
    public class ThingAdditionalInfo
    {
        public bool IsDestroyed { get; set; }
        public int? CreatorId { get; set; }
        public int? Score { get; set; }

        /// <summary>
        /// Created to track BigMass collisions.
        /// TODO: track this for a player too, independently from the score?
        /// </summary>
        public int? DeathsCaused { get; set; }

        public string WeaponsSerialized { get; set; }

        public Weapons GetWeapons()
        {
            if (String.IsNullOrEmpty(WeaponsSerialized))
            {
                return null;
            }
            return WeaponsSerialized.DeSerializeJson<Weapons>();
        }

        public ThingBase Creator { get; private set; }

        public static ThingAdditionalInfo GetNew(ThingBase creator, Weapons weapons = null)
        {
            var result = new ThingAdditionalInfo {Creator = creator, WeaponsSerialized = weapons?.SerializeJson()};
            // 0 score, not destoryed, possibly a creator.
            if (creator != null)
            {
                result.CreatorId = creator.Id;
            }
            return result;
        }

        public void SetCreator(ThingBase creator)
        {
            Creator = creator;
        }

        public void SetCreator(GameThingCoordinator coordinator)
        {
            Creator = coordinator.GetThingById(CreatorId);
        }

        public void SetCreator(ThingsThreadSafeList thingsThreadSafe)
        {
            Creator = CreatorId.HasValue ? thingsThreadSafe.GetById(CreatorId.Value) : null;
        }
    }
}