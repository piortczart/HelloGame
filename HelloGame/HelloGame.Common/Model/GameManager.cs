using System;
using System.Drawing;
using System.Linq;
using HelloGame.Common.Logging;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Model.GameObjects;
using HelloGame.Common.Model.GameObjects.Ships;

namespace HelloGame.Common.Model
{
    public class GameManager
    {
        private readonly ThingFactory _thingFactory;
        private readonly bool _isServer;
        private readonly ILogger _logger;
        public ModelManager ModelManager { get; }

        public GameManager(ModelManager modelManager, ThingFactory thingFactory, bool isServer, ILoggerFactory loggerFactory)
        {
            _thingFactory = thingFactory;
            _isServer = isServer;
            _logger = loggerFactory.CreateLogger(GetType());
            ModelManager = modelManager;
        }

        public void SetUpdateModelAction(Action action)
        {
            ModelManager.SetUpdateModelAction(action);
        }

        public PlayerShipAny AddPlayer(string name)
        {
            _logger.LogInfo($"Adding player: {name}");
            PlayerShipAny newShip = _thingFactory.GetPlayerShip(15, new Point(100, 100), name);
            ModelManager.AddThing(newShip);
            return newShip;
        }

        public void AddAiShip()
        {
            _logger.LogInfo("Adding AI ship.");
            AiShip newShip = _thingFactory.GetAiShip(15, new Point(400, 400), "Stupid AI");
            ModelManager.AddThing(newShip);
        }

        public void AddBigThing()
        {
            _logger.LogInfo("Adding a big thing.");
            BigMass bigMass = _thingFactory.GetBigMass();
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

                AddAiShip();

                //for (int i = 0; i < MathX.Random.Next(1, 4); i++)
                //{
                    AddBigThing();
                //}
            }
        }

        public void CreateFromDescription(ThingDescription description)
        {
            ThingBase thing = _thingFactory.CreateFromDescription(description);
            ModelManager.AddThing(thing);
        }

        public void SetKeysInfo(KeysInfo keysMine)
        {
            PlayerShipMovable shipMovable = (PlayerShipMovable)ModelManager.GetThings().FirstOrDefault(t => t is PlayerShipMovable);
            if (shipMovable != null)
            {
                shipMovable.KeysInfo = keysMine;
            }
        }
    }
}