using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using HelloGame.GameObjects;

namespace HelloGame
{
    public class Scene
    {
        private readonly List<ThingBase> _things = new List<ThingBase>();
        private TimeSpan _lastModelUpdate = TimeSpan.Zero;
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public Scene(HelloGameForm form)
        {
            Timer timer = new Timer {Interval = 5};
            timer.Tick += (a, b) =>
            {
                TimeSpan now = _stopwatch.Elapsed;
                TimeSpan sinceLast = now - _lastModelUpdate;
                _lastModelUpdate = _stopwatch.Elapsed;
                foreach (ThingBase item in _things.ToArray())
                {
                    item.UpdateModel(sinceLast);
                    if (item.IsTimeToDie)
                    {
                        _things.Remove(item);
                    }
                }
                form.Refresh();
            };
            timer.Start();
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
    }
}
