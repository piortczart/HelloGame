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
        //private Action<ThingBase> _askToSpawnAction;
        private Action<ThingBase> _addThingAction;
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
            return _model.ThingsThreadSafe.GetById(id.Value);
        }

        public void SetActions(Action<ThingBase> addThing, Action<ThingBase> shootLazer)
        {
            //_askToSpawnAction = askToSpawn;
            _addThingAction = addThing;
            _shootLazerAction = shootLazer;
        }

        public void AddThing(ThingBase thing)
        {
            _addThingAction(thing);
        }

        //public void AskServerToSpawn(ThingBase thing)
        //{
        //    _askToSpawnAction(thing);
        //}

        //public void UpdateThing(ThingBase thing, ThingBase.UpdateLocationSettings settings)
        //{
        //    _updateThingAction(thing, settings);
        //}

        internal void ShootLazer(ShipBase shipBase)
        {
            _shootLazerAction(shipBase);
        }
    }
}