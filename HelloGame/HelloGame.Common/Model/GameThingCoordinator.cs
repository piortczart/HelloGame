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
        private Action<ThingBase> _askToSpawnAction;
        private Action<ThingBase> _updateThingAction;
        private Action<ThingBase> _shootLazerAction;

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
            return _model.Things.GetById(id.Value);
        }

        public void SetActions(Action<ThingBase> askToSpawn, Action<ThingBase> updateThing, Action<ThingBase> shootLazer)
        {
            _askToSpawnAction = askToSpawn;
            _updateThingAction = updateThing;
            _shootLazerAction = shootLazer;
        }

        public void AskServerToSpawn(ThingBase thing)
        {
            _askToSpawnAction(thing);
        }

        public void UpdateThing(ThingBase thing)
        {
            _updateThingAction(thing);
        }

        internal void ShootLazer(ShipBase shipBase)
        {
            _shootLazerAction(shipBase);
        }
    }
}