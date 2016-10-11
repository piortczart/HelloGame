using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using HelloGame.Common.Extensions;
using HelloGame.Common.Logging;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Model.GameEvents;
using HelloGame.Common.Model.GameObjects;
using HelloGame.Common.Model.GameObjects.Ships;
using HelloGame.Common.Physicsish;
using HelloGame.Common.Settings;
using HelloGame.Common.TimeStuffs;

namespace HelloGame.Common.Model
{
    public class GameManager
    {
        private readonly ThingFactory _thingFactory;
        private readonly bool _isServer;
        private readonly ILogger _logger;
        public ModelManager ModelManager { get; }
        private readonly ConcurrentQueue<ThingBase> _thingsToSpawn = new ConcurrentQueue<ThingBase>();
        private readonly GeneralSettings _settings;
        private readonly GameEventBusSameThread _eventBus;
        private readonly TimeSource _timeSource;

        /// <summary>
        /// Others mightbe interested in knowing when the client wants to spawn somehting (like the network classes which will immediatelly send info to the server).
        /// </summary>
        public event Action<ThingBase> OnAskedServerToSpawn;

        public GameManager(GeneralSettings settings, ModelManager modelManager, GameThingCoordinator gameCoordinator,
            ThingFactory thingFactory, bool isServer, ILoggerFactory loggerFactory, GameEventBusSameThread eventBus,
            TimeSource timeSource)
        {
            ModelManager = modelManager;
            modelManager.AddUpdateModelAction(ModelUpdated);
            gameCoordinator.OnClientShootRequest += ShootAttempt;
            _thingFactory = thingFactory;
            _isServer = isServer;
            _eventBus = eventBus;
            _timeSource = timeSource;
            _logger = loggerFactory.CreateLogger(GetType());
            _settings = settings;
        }

        private void AskServerToSpawn(ThingBase thing)
        {
            _thingsToSpawn.Enqueue(thing);

            // Inform that the event has happened.
            OnAskedServerToSpawn?.Invoke(thing);
        }

        /// <summary>
        /// Gets the and dequeues the things which are waiting to be spawned.
        /// </summary>
        /// <returns></returns>
        public List<ThingBase> GetAndDequeueThingsToSpawn()
        {
            var result = new List<ThingBase>();
            ThingBase thing;
            while (_thingsToSpawn.TryDequeue(out thing))
            {
                result.Add(thing);
            }
            return result;
        }

        /// <summary>
        /// Will be called by the model each time it's updated.
        /// </summary>
        private void ModelUpdated()
        {
        }

        private Point FindEmptyArea(Rectangle retangle, int minDistance)
        {
            Position pos;
            int i = 0;
            do
            {
                pos = MathX.Random.GetRandomPosition(retangle);
                if (ModelManager.ThingsThreadSafe.GetThingsReadOnly().All(t => t.DistanceTo(pos) >= minDistance))
                {
                    break;
                }
            } while (i++ < 10);
            return new Point((int) pos.X, (int) pos.Y);
        }

        private Point GetRandomEmptyLocation(int distanceToStuff = 50)
        {
            return FindEmptyArea(new Rectangle(distanceToStuff, distanceToStuff,
                _settings.GameSize.Width - distanceToStuff, _settings.GameSize.Height - distanceToStuff),
                distanceToStuff);
        }

        private void ResurrectPlayerRandom(PlayerShipOther deadShip, string name, ClanEnum clan)
        {
            if (!_isServer)
            {
                throw new Exception("Only server can add players.");
            }

            PlayerShipOther newShip = AddPlayerRandom(name, clan, deadShip.Score);
            newShip.Score = deadShip.Score;
            _eventBus.ThePlayerSpawned(deadShip, newShip);
        }


        public PlayerShipOther AddPlayerRandom(string name, ClanEnum clan, int score = 0)
        {
            if (!_isServer)
            {
                throw new Exception("Only server can add players.");
            }

            _logger.LogInfo($"Adding player: {name}");
            Point trueLocation = GetRandomEmptyLocation();
            PlayerShipOther newShip = _thingFactory.GetPlayerShip(trueLocation, name, clan);
            ModelManager.AddThing(newShip);
            return newShip;
        }

        private void ShootAttempt(ThingBase source, Weapon weapon)
        {
            if (source is PlayerShipMovable)
            {
                if (_isServer)
                {
                    throw new Exception("PlayerShipMovable can't exist on the server.");
                }

                ThingBase projectile;
                // Player is shooting. On the client side. This only asks server to spawn a lazer.
                switch (weapon.WeaponType)
                {
                    case WeaponType.Lazer:
                        projectile = _thingFactory.GetLazerBeam(-1, ThingAdditionalInfo.GetNew(source));
                        break;
                    case WeaponType.Bomb:
                        projectile = _thingFactory.GetBomb(-1, ThingAdditionalInfo.GetNew(source));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                // THe proejctile is null when server did not find the shooter as an object
                if (projectile != null)
                {
                    AskServerToSpawn(projectile);
                }
            }
            else
            {
                // AI is shooting. Only server can spawn.
                if (_isServer)
                {
                    switch (weapon.WeaponType)
                    {
                        case WeaponType.Lazer:
                            LazerBeamPew lazer = _thingFactory.GetLazerBeam(null, ThingAdditionalInfo.GetNew(source));
                            ModelManager.AddThing(lazer);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            weapon.LastShotTime = _timeSource.ElapsedSinceStart;
        }

        public AiShip AddAiShipRandom(string name = null)
        {
            if (!_isServer)
            {
                throw new Exception("Only server can add AI ships.");
            }
            if (_settings.SpawnAi)
            {
                _logger.LogInfo("Adding AI ship.");
                Point location = GetRandomEmptyLocation();
                AiShip newShip = _thingFactory.GetRandomAiShip(location, name ?? AiShipSettings.GetRandomAiShipName());
                ModelManager.AddThing(newShip);
                return newShip;
            }
            return null;
        }

        public void AddBigThingRandom()
        {
            _logger.LogInfo("Adding a big thing.");
            int size = MathX.Random.Next(30, 170);
            Point location = GetRandomEmptyLocation(size + 50);
            BigMass bigMass = _thingFactory.GetBigMass(size, location, null);
            ModelManager.AddThing(bigMass);
        }

        public void StartGame()
        {
            SpawnStart();
            ModelManager.StartModelUpdates();
        }

        private void SpawnStart()
        {
            if (_isServer)
            {
                for (int i = 0; i < _settings.AiShipCount; i++)
                {
                    AddAiShipRandom();
                }

                for (int i = 0; i < _settings.PlanetsCount; i++)
                {
                    AddBigThingRandom();
                }
            }
        }

        /// <summary>
        /// Called when the server sends the "Stuff Died" message.
        /// </summary>
        public void StuffDied(List<int> stuffIds)
        {
            var toDespawn = ModelManager.ThingsThreadSafe.GetByIds(stuffIds);
            _logger.LogInfo(
                $"Asked to despawn items: {toDespawn.Count} ({string.Join(",", toDespawn.Select(t => t.GetType().Name))})");
            foreach (ThingBase thingToRemove in toDespawn)
            {
                thingToRemove.Despawn();
            }
        }

        public ParseThingResult ParseThingDescription(ThingDescription description, ParseThingSource source)
        {
            // Create a full "thing" from the description.
            // It will then be either added as a new thing, or an existing thing will be updated based on it.
            ThingBase thing = _thingFactory.CreateFromDescription(description);
            if (thing != null)
            {
                return ModelManager.HandleThingInfo(thing, source);
            }
            return ParseThingResult.Unknown;
        }

        public void SetKeysInfo(KeysInfo keysMine)
        {
            PlayerShipMovable shipMovable = ModelManager.ThingsThreadSafe.GetPlayerShip();
            if (shipMovable != null)
            {
                shipMovable.KeysInfo = keysMine;
            }
        }

        public ThingBase GetMe()
        {
            return ModelManager.ThingsThreadSafe.GetPlayerShip();
        }

        public void ParseThingDescriptions(List<ThingDescription> stuff, ParseThingSource source)
        {
            foreach (ThingDescription thing in stuff)
            {
                ParseThingDescription(thing, source);
            }
        }

        public void Resurrect(ThingBase thing)
        {
            if (!_isServer)
            {
                throw new ArgumentException("Can only be called by the server.");
            }

            var ai = thing as AiShip;
            if (ai != null)
            {
                ResurrectAiRandom(ai);
                return;
            }

            var ship = thing as PlayerShipOther;
            if (ship != null)
            {
                ResurrectPlayerRandom(ship, ship.Name, ship.Clan);
            }
        }

        private void ResurrectAiRandom(AiShip ai)
        {
            if (!_isServer)
            {
                throw new Exception("Only server can add players.");
            }

            AddAiShipRandom(ai.Name);
        }
    }

    public enum ParseThingResult
    {
        Unknown,
        UpdateSuccess,
        UpdateFailedThingMissing
    }
}