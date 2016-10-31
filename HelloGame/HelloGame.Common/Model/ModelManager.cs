using System;
using System.Collections.Generic;
using System.Threading;
using HelloGame.Common.Logging;
using System.Collections.Concurrent;
using HelloGame.Common.Extensions;
using HelloGame.Common.Model.GameObjects.Ships;
using HelloGame.Common.Settings;
using HelloGame.Common.TimeStuffs;

namespace HelloGame.Common.Model
{
    public class ModelManager
    {
        private readonly ILogger _logger;
        readonly CollisionDetector _collidor;
        private readonly EventPerSecond _modelUpdateCounter;
        private readonly Thread _modelUpdateThread;
        private readonly ThingsThreadSafeList _thingsThreadSafe = new ThingsThreadSafeList();
        private readonly ConcurrentQueue<ThingBase> _elapsedThings = new ConcurrentQueue<ThingBase>();
        private readonly List<Action> _updateModelAction = new List<Action>();
        public EventPerSecond CollisionCalculations => _collidor.CollisoinsCounter;
        private readonly Overlay _overlay;
        public ThingsThreadSafeList ThingsThreadSafe => _thingsThreadSafe;
        private readonly TimeSource _timeSource;
        private readonly bool _isServer;
        private readonly TimeCounter _modelUpdateTimeCounter;
        public readonly ThingsToRespawnThreadSafe ThingsToRespawn;

        public ModelManager(ILoggerFactory loggerFactory, TimeSource timeSource, Overlay overlay, bool isServer,
            GeneralSettings settings)
        {
            ThingsToRespawn = new ThingsToRespawnThreadSafe(timeSource);
            _timeSource = timeSource;
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

        public void AddThing(ThingBase thing)
        {
            if (!_thingsThreadSafe.AddNewThing(thing))
            {
                throw new Exception("An exiting thing was asked to be added!");
            }
        }

        public bool UpdateThing(ThingBase sourceThing, ThingBase.UpdateLocationSettings settings)
        {
            ThingBase existing = _thingsThreadSafe.GetById(sourceThing.Id);
            if (existing == null)
            {
                return false;
            }
            existing.UpdateState(sourceThing, settings);
            return true;
        }

        public void AddOrUpdateThing(ThingBase sourceThing, ThingBase.UpdateLocationSettings settings)
        {
            ThingBase alreadyExisting = _thingsThreadSafe.AddIfMissing(sourceThing);
            alreadyExisting?.UpdateState(sourceThing, settings);
        }

        /// <summary>
        /// Returns all things that have been despawned since last call of this method.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ThingBase> ConsumeElapsedThings()
        {
            return _elapsedThings.GetAll();
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
                _overlay.UpdateDuringModelUpdate(this);
            }

            _modelUpdateCounter.Add();

            IReadOnlyCollection<ThingBase> things = _thingsThreadSafe.GetThingsReadOnly();
            foreach (ThingBase thing in things)
            {
                // Perform the model update.
                thing.UpdateModel(timePassed, things);

                // Despawn the thing if it should elapse.
                if (thing.IsTimeToElapse)
                {
                    _thingsThreadSafe.Remove(thing);
                    _elapsedThings.Enqueue(thing);
                    // Only the server has the afterlife with resurrect/respawn options.
                    if (_isServer)
                    {
                        Afterlife(thing);
                    }
                }
            }
            _collidor.DetectCollisions(_thingsThreadSafe.GetThingsArray());

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

        /// <summary>
        /// What to do with a dead thing?
        /// </summary>
        private void Afterlife(ThingBase deadThing)
        {
            // Any ship will get ressurected (the object itself will be recreated though).
            var ship = deadThing as ShipBase;
            if (ship != null)
            {
                TimeSpan whenToRespawn = _timeSource.ElapsedSinceStart.Add(ship.ShipSettings.RespawnTime);
                ThingsToRespawn.Add(new ThingToRespawn(whenToRespawn, deadThing));
            }
        }

        public ParseThingResult HandleThingInfo(ThingBase thing, ParseThingSource source)
        {
            switch (source)
            {
                case ParseThingSource.ToClient:

                    if (thing is PlayerShipMovable)
                    {
                        // Do not update the player's angle. That is what he always controls.
                        // The player's position should not be updated because server does not have the latest engine info (has it with a big delay)
                        // This migh tbe the first time we are asked to spawn our ship?...
                        AddOrUpdateThing(thing, ThingBase.UpdateLocationSettings.ExcludePositionAndAngle);
                    }
                    else
                    {
                        // We were sent a new thing or an update of something else. Update or add totally.
                        AddOrUpdateThing(thing, ThingBase.UpdateLocationSettings.All);
                    }
                    return ParseThingResult.Unknown;
                case ParseThingSource.ToServer_PlayerPosition:
                    if (!UpdateThing(thing, ThingBase.UpdateLocationSettings.AngleAndEngineAndPosition))
                    {
                        // Player thinks he is alive, but he is dead.
                        return ParseThingResult.UpdateFailedThingMissing;
                    }
                    return ParseThingResult.UpdateSuccess;
                case ParseThingSource.ToServer_SpawnRequest:
                    AddThing(thing);
                    return ParseThingResult.Unknown;
                default:
                    throw new ArgumentOutOfRangeException(nameof(source), source, null);
            }
        }
    }
}