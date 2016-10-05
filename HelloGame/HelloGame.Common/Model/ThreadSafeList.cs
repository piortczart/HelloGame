using HelloGame.Common.Model.GameObjects.Ships;
using System.Collections.Generic;
using System.Linq;

namespace HelloGame.Common.Model
{
    /// <summary>
    /// Poor man's thread-safe list implementaion.
    /// </summary>
    public class ThingsList
    {
        private readonly List<ThingBase> _interalList = new List<ThingBase>();
        private static readonly object Lock = new object();

        public int Count
        {
            get
            {
                lock (Lock)
                {
                    return _interalList.Count;
                }
            }
        }

        public IReadOnlyCollection<ThingBase> GetThingsReadOnly()
        {
            lock (Lock)
            {
                return new List<ThingBase>(_interalList).AsReadOnly();
            }
        }

        /// <summary>
        /// Modifying the collection this method returns will have no effect on the underlying list of things.
        /// </summary>
        public ThingBase[] GetThingsArray()
        {
            lock (Lock)
            {
                var result = new ThingBase[_interalList.Count];
                _interalList.CopyTo(result);
                return result;
            }
        }

        public PlayerShipMovable GetPlayerShip()
        {
            lock (Lock)
            {
                return (PlayerShipMovable)_interalList.SingleOrDefault(t => t is PlayerShipMovable);
            }
        }

        /// <summary>
        /// Adds the thing if it's not already there.
        /// Returns null if it added the new thing or existing thing if there is something with the given id.
        /// </summary>
        public ThingBase AddIfMissing(ThingBase otherThing)
        {
            lock (Lock)
            {
                // Check if it already exists.
                ThingBase existing = _interalList.Find(t => t.Id == otherThing.Id);
                if (existing == null)
                {
                    // It is not here yet - add it.
                    _interalList.Add(otherThing);
                    // Return null to incidate the thing was added.
                    return null;
                }
                // There is already something with this id - return it.
                return existing;
            }
        }

        public ThingBase GetById(int id)
        {
            lock (Lock)
            {
                return _interalList.Find(t => t.Id == id);
            }
        }

        public bool Remove(ThingBase thing)
        {
            lock (Lock)
            {
                // This will not throw an exception if the thing is not there.
                return _interalList.Remove(thing);
            }
        }

        public IReadOnlyCollection<ThingBase> GetByIds(IList<int> stuffIds)
        {
            lock (Lock)
            {
                return new List<ThingBase>(_interalList.Where(t => stuffIds.Contains(t.Id))).AsReadOnly();
            }
        }
    }
}