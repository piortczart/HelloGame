using System;
using HelloGame.Common.Model.GameObjects.Ships;

namespace HelloGame.Common.Model
{
    /// <summary>
    /// This was created because ThingBase (ship) needed GameManager to add stuff, was constructed in ThingFactory, which was needed in GameManager.
    /// Circular constructor stuff.
    /// </summary>
    public class GameThingCoordinator
    {
        private readonly ModelManager _model;

        public event Action<ThingBase, Weapon> OnClientShootRequest;

        public GameThingCoordinator(ModelManager model)
        {
            _model = model;
        }

        public ThingBase GetThingById(int? id)
        {
            if (id == null)
            {
                return null;
            }
            return _model.ThingsThreadSafe.GetById(id.Value);
        }

        public void Shoot(ShipBase shooter, Weapon weapon)
        {
            OnClientShootRequest?.Invoke(shooter, weapon);
        }
    }
}