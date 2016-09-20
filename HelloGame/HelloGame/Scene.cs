using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using HelloGame.GameObjects;

namespace HelloGame
{
    public class Scene
    {
        private readonly List<ThingBase> _things = new List<ThingBase>();

        public Scene(HelloGameForm form)
        {
            Timer timer = new Timer {Interval = 5};
            timer.Tick += (a, b) =>
            {
                foreach (ThingBase item in _things.ToArray())
                {
                    item.UpdateModel();
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
