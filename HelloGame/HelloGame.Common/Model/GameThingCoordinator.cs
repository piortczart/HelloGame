using System;
using System.Linq;

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

        public int ThingsCount { get { return _model.GetThings().Count; } }

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
            return _model.GetThings().FirstOrDefault(t=>t.Id == id);
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