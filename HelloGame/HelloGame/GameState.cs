using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using HelloGame.GameObjects;
using HelloGame.GameObjects.Ships;
using HelloGame.MathStuff;

namespace HelloGame
{
    public class GameState
    {
        readonly KeysInfo _keysMine;
        DaShip _ship;
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        readonly CollisionDetector _collidor = new CollisionDetector();
        private readonly List<ThingBase> _things = new List<ThingBase>();
        private TimeSpan _lastModelUpdate = TimeSpan.Zero;
        readonly HelloGameForm _form;
        Thread _t;

        public CounterInTime ModelUpdateCounter = new CounterInTime(TimeSpan.FromSeconds(1));

        public GameState(HelloGameForm form, KeysInfo keysMine)
        {
            _keysMine = keysMine;
            _form = form;
            Restart();
            StartGame();
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
            _t = new Thread(UpdateModel);
            _t.IsBackground = false;
            _t.Start();
        }

        private void UpdateModel()
        {
            while (_t.IsAlive)
            {
                ModelUpdateCounter.Add();

                if (_ship.IsTimeToElapse)
                {
                    Restart();
                }

                TimeSpan now = _stopwatch.Elapsed;
                if (_lastModelUpdate != TimeSpan.Zero)
                {
                    TimeSpan sinceLast = now - _lastModelUpdate;

                    var nonModifiable = new List<ThingBase>(_things);
                    Parallel.ForEach(nonModifiable, item =>
                    {
                        item.UpdateModel(sinceLast, nonModifiable);
                        if (item.IsTimeToElapse)
                        {
                            if (_things.Contains(item))
                            {
                                _things.Remove(item);
                            }
                        }
                    });
                    _collidor.DetectCollisions(_things);
                }
                _lastModelUpdate = now;

                if (!_form.IsDisposed) { _form.Invoke(() => { _form.Refresh(); }); }
            }
        }

        private void Restart()
        {
            RemoveAll();

            _ship = new PlayerShip(_keysMine, this);
            _ship.Spawn(new Point(100, 100));
            AddThing(_ship);

            var aiShip = new AiShip(this);
            aiShip.Spawn(new Point(400, 100));
            AddThing(aiShip);

            for(int i=0; i<MathX.Random.Next(1,4); i++)
            {
                var mass = new BigMass(MathX.Random.Next(80, 200));
                mass.Spawn(new Point(MathX.Random.Next(100,500), MathX.Random.Next(400,600)));
                AddThing(mass);
            }
        }
    }
}