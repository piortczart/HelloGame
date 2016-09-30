using System;

namespace HelloGame.Common.Model
{
    /// <summary>
    /// This was created because ThingBase (ship) needed GameManager to add stuff, was constructed in ThingFactory, which was needed in GameManager.
    /// Circular constructor stuff.
    /// </summary>
    public class GameThingCoordinator
    {
        private ModelManager _model;

        Action<ThingBase> _askToSpawnAction;
        Action<ThingBase> _updateThingAction;

        public GameThingCoordinator(ModelManager model)
        {
            _model = model;
        }

        public void SetActions(Action<ThingBase> askToSpawn, Action<ThingBase> updateThing)
        {
            _askToSpawnAction = askToSpawn;
            _updateThingAction = updateThing;
        }

        public void AskServerToSpawn(ThingBase thing) {
            _askToSpawnAction(thing);
        }

        public void UpdateThing(ThingBase thing) {
            _updateThingAction(thing);
        }
    }
}