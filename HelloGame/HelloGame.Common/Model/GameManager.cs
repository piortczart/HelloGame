using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using HelloGame.Common.Extensions;
using HelloGame.Common.Logging;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Model.GameObjects;
using HelloGame.Common.Model.GameObjects.Ships;
using HelloGame.Common.Physicsish;
using HelloGame.Common.Settings;

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

        public GameManager(GeneralSettings settings, ModelManager modelManager, GameThingCoordinator gameCoordinator,
            ThingFactory thingFactory, bool isServer, ILoggerFactory loggerFactory)
        {
            ModelManager = modelManager;
            modelManager.AddUpdateModelAction(ModelUpdated);
            gameCoordinator.SetActions(ModelManager.AddThing, ShootLazer);
            _thingFactory = thingFactory;
            _isServer = isServer;
            _logger = loggerFactory.CreateLogger(GetType());
            _settings = settings;
        }

        private void AskServerToSpawn(ThingBase thing)
        {
            _thingsToSpawn.Enqueue(thing);
        }

        public List<ThingBase> GetThingsToSpawn()
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
            // Only the server can spawn new ships.
            if (_isServer)
            {
                if (!ModelManager.ThingsThreadSafe.GetThingsReadOnly().Any(t => t is AiShip))
                {
                    AddAiShip();
                }
            }
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

        private Point GetNewPlayerLocation()
        {
            return FindEmptyArea(new Rectangle(10, 10, 100, 800), 50);
        }

        public PlayerShipOther AddPlayer(string name, ClanEnum clan)
        {
            if (!_isServer)
            {
                throw new Exception("Only server can add players.");
            }

            _logger.LogInfo($"Adding player: {name}");
            Point location = GetNewPlayerLocation();
            // Try to find a relatively empty area.
            PlayerShipOther newShip = _thingFactory.GetPlayerShip(location, name, clan);
            ModelManager.AddThing(newShip);
            return newShip;
        }

        private void ShootLazer(ThingBase source)
        {
            if (source is PlayerShipMovable)
            {
                // Player is shooting. On the client.
                LazerBeamPew lazer = _thingFactory.GetLazerBeam(-1,
                    source.Physics.GetPointInDirection(source.Settingz.Size/2), source);
                AskServerToSpawn(lazer);
            }
            else
            {
                // AI is shooting. Only server can spawn.
                if (_isServer)
                {
                    LazerBeamPew lazer = _thingFactory.GetLazerBeam(null,
                        source.Physics.GetPointInDirection(source.Settingz.Size/2), source);
                    ModelManager.AddThing(lazer);
                }
            }
        }

        private void AddAiShip()
        {
            if (!_isServer)
            {
                throw new Exception("Only server can add AI ships.");
            }
            if (_settings.SpawnAi)
            {
                _logger.LogInfo("Adding AI ship.");
                Point location = FindEmptyArea(new Rectangle(100, 300, 300, 800), 50);
                AiShip newShip = _thingFactory.GetRandomAiShip(location, null);
                ModelManager.AddThing(newShip);
            }
        }

        public void AddBigThing()
        {
            _logger.LogInfo("Adding a big thing.");
            int size = MathX.Random.Next(30, 170);
            Point location = FindEmptyArea(new Rectangle(100, 50, 900, 900), size + 50);
            BigMass bigMass = _thingFactory.GetBigMass(size, location);
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
                // The server can spawn items without giving them ids.
                //AddThing(_thingFactory.GetPlayerShip(25, new Point(100, 100)));

                //AddAiShip();
                AddAiShip();

                for (int i = 0; i < MathX.Random.Next(5, 10); i++)
                {
                    AddBigThing();
                }
            }
        }

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

        public void ParseThingDescription(ThingDescription description, ParseThingSource source)
        {
            // Create a full "thing" from the description.
            // It will then be either added as a new thing, or an existing thing will be updated based on it.
            ThingBase thing = _thingFactory.CreateFromDescription(description);
            if (thing != null)
            {
                ModelManager.HandleNewThing(thing, source);
            }
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
    }
}