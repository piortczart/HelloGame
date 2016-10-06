using System;
using System.Collections.Generic;
using System.Threading;
using HelloGame.Common.Logging;
using System.Collections.Concurrent;
using HelloGame.Common.Extensions;
using HelloGame.Common.Settings;

namespace HelloGame.Common.Model
{
    public class ModelManager
    {
        private readonly ILogger _logger;
        readonly CollisionDetector _collidor;
        private readonly EventPerSecond _modelUpdateCounter;
        private readonly Thread _modelUpdateThread;
        private readonly ThingsList _things = new ThingsList();
        private readonly ConcurrentQueue<ThingBase> _deadThings = new ConcurrentQueue<ThingBase>();
        private readonly List<Action> _updateModelAction = new List<Action>();
        public EventPerSecond CollisionCalculations => _collidor.CollisoinsCounter;
        private readonly Overlay _overlay;
        public ThingsList Things => _things;
        private readonly bool _isServer;
        private readonly TimeCounter _modelUpdateTimeCounter;

        public ModelManager(ILoggerFactory loggerFactory, TimeSource timeSource, Overlay overlay, bool isServer,
            GeneralSettings settings)
        {
            _isServer = isServer;
            _logger = loggerFactory.CreateLogger(GetType());
            _modelUpdateThread = new Thread(UpdateModel) {IsBackground = true};
            _overlay = overlay;
            _collidor = new CollisionDetector(timeSource, settings);
            _modelUpdateCounter = new EventPerSecond(timeSource);
            _modelUpdateTimeCounter = new TimeCounter(timeSource);
        }

        public void AddUpdateModelAction(Action action)
        {
            _updateModelAction.Add(action);
        }

        public void AddOrUpdateThing(ThingBase thingBase)
        {
            ThingBase alreadyExisting = _things.AddIfMissing(thingBase);
            alreadyExisting?.UpdateLocation(thingBase);
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
                SingleModelUpdate();
            }
        }

        public void SingleModelUpdate()
        {
            TimeSpan timePassed = _modelUpdateTimeCounter.GetTimeSinceLastCall();

            if (!_isServer)
            {
                // Overlay only needed in client.
                _overlay.Update(this);
            }

            _modelUpdateCounter.Add();

            IReadOnlyCollection<ThingBase> things = _things.GetThingsReadOnly();
            foreach (ThingBase thing in things)
            {
                if (!thing.IsDestroyed)
                {
                    // Perform the model update.
                    thing.UpdateModel(timePassed, things);

                    // Despawn the thing if it should elapse.
                    if (thing.IsTimeToElapse)
                    {
                        _things.Remove(thing);
                        _deadThings.Enqueue(thing);
                    }
                }
            }
            _collidor.DetectCollisions(_things.GetThingsArray());

            foreach (var action in _updateModelAction)
            {
                action.Invoke();
            }

            int toSleep = 10 - (int) timePassed.TotalMilliseconds;
            if (toSleep > 0)
            {
                Thread.Sleep(toSleep);
            }
        }
    }
}