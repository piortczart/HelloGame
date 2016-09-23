using System.Drawing;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Model.GameObjects;
using HelloGame.Common.Model.GameObjects.Ships;

namespace HelloGame.Common.Model
{
    public class GameManager
    {
        public ModelManager ModelManager { get; }

        public GameManager(ModelManager modelManager)
        {
            ModelManager = modelManager;
        }

        private void AddThing(ThingBase thing)
        {
            ModelManager.AddThing(thing);
        }

        public void StartGame()
        {
            SpawnStart();
            ModelManager.StartModelUpdates();
        }

        private void SpawnStart()
        {
            var ship = new PlayerShipAny(ModelManager, 30);
            ship.Spawn(new Point(100, 100));
            AddThing(ship);

            var aiShip = new AiShip(ModelManager, 50);
            aiShip.Spawn(new Point(400, 100));
            AddThing(aiShip);

            for (int i = 0; i < MathX.Random.Next(1, 4); i++)
            {
                var mass = new BigMass(MathX.Random.Next(80, 200));
                mass.Spawn(new Point(MathX.Random.Next(100, 500), MathX.Random.Next(400, 600)));
                AddThing(mass);
            }
        }
    }
}