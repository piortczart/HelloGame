using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelloGame.Common.Logging;
using System.Collections.Concurrent;
using HelloGame.Common.Extensions;

namespace HelloGame.Common.Model
{
    public class ModelManager
    {
        private readonly ILogger _logger;
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        readonly CollisionDetector _collidor = new CollisionDetector();
        private TimeSpan _lastModelUpdate = TimeSpan.Zero;
        private readonly EventPerSecond _modelUpdateCounter = new EventPerSecond();
        private readonly Thread _modelUpdateThread;
        private readonly ThingsList _things = new ThingsList();
        private readonly ConcurrentQueue<ThingBase> _deadThings = new ConcurrentQueue<ThingBase>();
        private readonly List<Action> _updateModelAction = new List<Action>();
        public EventPerSecond CollisionCalculations => _collidor.CollisoinsCounter;
        private readonly Overlay _overlay;
        public ThingsList Things { get { return _things; } }
        private readonly bool _isServer;

        public ModelManager(ILoggerFactory loggerFactory, Overlay overlay, bool isServer)
        {
            _isServer = isServer;
            _logger = loggerFactory.CreateLogger(GetType());
            _modelUpdateThread = new Thread(UpdateModel) { IsBackground = true };
            _overlay = overlay;
        }

        public void AddUpdateModelAction(Action action)
        {
            _updateModelAction.Add(action);
        }

        public void AddOrUpdateThing(ThingBase thingBase)
        {
            ThingBase alreadyExisting = _things.AddIfMissing(thingBase);
            if (alreadyExisting != null)
            {
                alreadyExisting.UpdateLocation(thingBase);
            }
        }

        /// <summary>
        /// Returns all things that have been despawned since last call of this method.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ThingBase> GetDeadThings()
        {
            return _deadThings.GetAll();
        }

        public void StartModelUpdates()
        {
            _modelUpdateThread.Start();
        }

        private void UpdateModel()
        {
            while (_modelUpdateThread.IsAlive)
            {
                if (!_isServer)
                {
                    // Overlay only needed in client.
                    _overlay.Update(this);
                }

                _modelUpdateCounter.Add();

                TimeSpan now = _stopwatch.Elapsed;
                if (_lastModelUpdate != TimeSpan.Zero)
                {
                    TimeSpan sinceLast = now - _lastModelUpdate;

                    IReadOnlyCollection<ThingBase> things = _things.GetThingsReadOnly();
                    foreach (var thing in things)
                    {
                        // Perform the model update.
                        thing.UpdateModel(sinceLast, things);

                        // Despawn the thing if it should elapse.
                        if (thing.IsTimeToElapse)
                        {
                            _things.Remove(thing);
                            _deadThings.Enqueue(thing);
                        }
                    }

                    //Parallel.ForEach(things,
                    //    new ParallelOptions
                    //    {
                    //        MaxDegreeOfParallelism = Environment.ProcessorCount
                    //    },
                    //    thing =>
                    //    {
                    //        // Perform the model update.
                    //        thing.UpdateModel(sinceLast, things);

                    //        // Despawn the thing if it should elapse.
                    //        if (thing.IsTimeToElapse)
                    //        {
                    //            _things.Remove(thing);
                    //            _deadThings.Enqueue(thing);
                    //        }
                    //    });

                    _collidor.DetectCollisions(_things.GetThingsArray());
                }
                _lastModelUpdate = now;

                foreach (var action in _updateModelAction)
                {
                    action.Invoke();
                }
            }
        }
    }
}