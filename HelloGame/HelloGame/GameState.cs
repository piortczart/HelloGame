using HelloGame.GameObjects;
using HelloGame.GameObjects.Ships;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace HelloGame
{
    public class GameState
    {
        KeysInfo _keysMine;
        DaShip ship;
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        CollisionDetector collidor = new CollisionDetector();
        private readonly List<ThingBase> _things = new List<ThingBase>();
        private TimeSpan _lastModelUpdate = TimeSpan.Zero;
        HelloGameForm _form;

        public GameState(HelloGameForm form, KeysInfo keysMine)
        {
            _keysMine = keysMine;
            _form = form;
            Restart();
        }

        public void AddThing(ThingBase thingBase)
        {
            _things.Add(thingBase);
        }

        public void PaintStuff(Graphics g)
        {
            foreach (ThingBase item in _things)
            {
                item.PaintStuff(g);
            }
        }

        private void RemoveAll()
        {
            _things.Clear();
        }

        private void StartGame()
        {
            Timer timer = new Timer { Interval = 5 };
            timer.Tick += (a, b) =>
            {
                if (ship.IsTimeToElapse)
                {
                    Restart();
                }

                TimeSpan now = _stopwatch.Elapsed;
                TimeSpan sinceLast = now - _lastModelUpdate;
                _lastModelUpdate = _stopwatch.Elapsed;
                foreach (ThingBase item in _things.ToArray())
                {
                    item.UpdateModel(sinceLast, _things);
                    if (item.IsTimeToElapse)
                    {
                        _things.Remove(item);
                    }
                }
                collidor.DetectCollisions(_things);
                _form.Refresh();
            };
            timer.Start();
        }

        private void Restart()
        {
            RemoveAll();

            ship = new PlayerShip(_keysMine, this);
            ship.Spawn(new Point(100, 100));
            AddThing(ship);

            var aiShip = new AiShip(this);
            ship.Spawn(new Point(400, 100));
            AddThing(aiShip);

            var mass = new BigMass(80);
            mass.Spawn(new Point(500, 500));
            AddThing(mass);

            StartGame();
        }
    }
}