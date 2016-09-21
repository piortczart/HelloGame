using HelloGame.GameObjects;
using HelloGame.GameObjects.Ships;
using HelloGame.MathStuff;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelloGame
{
    public static class X
    {
        public static void Invoke(this Control control, Action action)
        {
            try
            {
                control.Invoke(action);
            }
            catch
            {
                // Ignore
            }
        }
    }

    public class CounterInTime
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        TimeSpan _time;

        private int lastCounter = 0;
        private int coutner = 0;

        private int lastPiece = 0;

        private int currentPiece => (int)Math.Floor(stopwatch.Elapsed.TotalMilliseconds / _time.TotalMilliseconds);

        public CounterInTime(TimeSpan time)
        {
            _time = time;
        }

        public void Add()
        {
            if (currentPiece != lastPiece)
            {
                lastPiece = currentPiece;
                lastCounter = coutner;
                coutner = 0;
            }

            coutner += 1;
        }

        public decimal GetPerTime()
        {
            return (decimal)(lastCounter / _time.TotalSeconds);
        }
    }



    public class GameState
    {
        KeysInfo _keysMine;
        DaShip _ship;
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        CollisionDetector _collidor = new CollisionDetector();
        private readonly List<ThingBase> _things = new List<ThingBase>();
        private TimeSpan _lastModelUpdate = TimeSpan.Zero;
        HelloGameForm _form;
        Thread t;

        public CounterInTime modelUpdateCounter = new CounterInTime(TimeSpan.FromSeconds(1));

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
            t = new Thread(new ThreadStart(UpdateModel));
            t.IsBackground = false;
            t.Start();
        }

        private void UpdateModel()
        {
            while (t.IsAlive)
            {
                modelUpdateCounter.Add();

                if (_ship.IsTimeToElapse)
                {
                    Restart();
                }

                TimeSpan now = _stopwatch.Elapsed;
                if (_lastModelUpdate != TimeSpan.Zero)
                {
                    TimeSpan sinceLast = now - _lastModelUpdate;

                    var nonModifiable = new List<ThingBase>(_things);
                    Parallel.ForEach(nonModifiable, (item) =>
                    {
                        item.UpdateModel(sinceLast, nonModifiable);
                        if (item.IsTimeToElapse)
                        {
                            _things.Remove(item);
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