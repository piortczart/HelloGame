using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HelloGame
{
    public class Scene
    {
        private List<AThing> _things = new List<AThing>();

        public Scene(HelloGameForm form)
        {
            Timer timer = new Timer();
            timer.Interval = 5;
            timer.Tick += (a, b) =>
            {
                foreach (AThing item in _things.ToArray())
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

        public void AddThing(AThing thing)
        {
            _things.Add(thing);
        }

        public void PaintStuff(Graphics g)
        {
            foreach (AThing item in _things)
            {
                item.PaintStuff(g);
            }
        }
    }
}
