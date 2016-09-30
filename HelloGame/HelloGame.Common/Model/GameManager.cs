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

namespace HelloGame.Common.Model
{
    public class GameManager
    {
        private readonly IThingFactory _thingFactory;
        private readonly bool _isServer;
        private readonly ILogger _logger;
        public ModelManager ModelManager { get; }
        private readonly ConcurrentQueue<ThingBase> _thingsToSpawn = new ConcurrentQueue<ThingBase>();
        readonly GameThingCoordinator _gameCoordinator;

        public GameManager(ModelManager modelManager, GameThingCoordinator gameCoordinator, IThingFactory thingFactory, bool isServer, ILoggerFactory loggerFactory)
        {
            ModelManager = modelManager;
            _gameCoordinator = gameCoordinator;
            _gameCoordinator.SetActions(AskServerToSpawn, ModelManager.UpdateThing);
            _thingFactory = thingFactory;
            _isServer = isServer;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public void AskServerToSpawn(ThingBase thing)
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

        public void SetUpdateModelAction(Action action)
        {
            ModelManager.SetUpdateModelAction(action);
        }

        public PlayerShipAny AddPlayer(string name)
        {
            _logger.LogInfo($"Adding player: {name}");
            PlayerShipAny newShip = _thingFactory.GetPlayerShip(15, new Point(100, 100), name);
            ModelManager.UpdateThing(newShip);
            return newShip;
        }

        private void AddAiShip()
        {
            _logger.LogInfo("Adding AI ship.");
            Point location = MathX.Random.GetRandomPoint(new Rectangle(300, 300, 300, 200));
            AiShip newShip = _thingFactory.GetAiShip(15, location, "Stupid AI");
            ModelManager.UpdateThing(newShip);
        }

        private void AddBigThing()
        {
            _logger.LogInfo("Adding a big thing.");
            BigMass bigMass = _thingFactory.GetBigMass();
            ModelManager.UpdateThing(bigMass);
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

                AddAiShip();
                AddAiShip();

                for (int i = 0; i < MathX.Random.Next(1, 1); i++)
                {
                    AddBigThing();
                }
            }
        }

        public void ParseThingDescription(ThingDescription description)
        {
            ThingBase thing = _thingFactory.CreateFromDescription(description);
            thing.Physics.Update(description.AlmostPhysics, ThingBase.UpdateLocationSettings.All);
            ModelManager.UpdateThing(thing);
        }

        public void SetKeysInfo(KeysInfo keysMine)
        {
            PlayerShipMovable shipMovable = (PlayerShipMovable)ModelManager.GetThings().FirstOrDefault(t => t is PlayerShipMovable);
            if (shipMovable != null)
            {
                shipMovable.KeysInfo = keysMine;
            }
        }

        public List<ThingBase> GetMyThings()
        {
            ThingBase me = GetMe();
            return ModelManager.GetThings().Where(t => t.Creator == me).ToList();
        }

        public ThingBase GetMe()
        {
            return ModelManager.GetThings().FirstOrDefault(t => t is PlayerShipMovable);
        }

        public void ParseThingDescriptions(List<ThingDescription> stuff)
        {
            foreach (ThingDescription thing in stuff)
            {
                ParseThingDescription(thing);
            }
        }
    }
}