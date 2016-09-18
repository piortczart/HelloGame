using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HelloGame
{
    public partial class HelloGameForm : Form
    {
        public KeysInfo KeysMine = new KeysInfo();
        private List<IThing> _things = new List<IThing>();

        public HelloGameForm()
        {
            InitializeComponent();

            _things.Add(new ThingForce(KeysMine, new Point(100, 100)));

            Timer timer = new Timer();
            timer.Interval = 5;
            timer.Tick += (a, b) =>
            {
                foreach (IThing item in _things)
                {
                    item.UpdateModel();
                }
                Refresh();
            };
            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            foreach (IThing item in _things)
            {
                item.PaintStuff(e.Graphics); 
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            KeysMine.Pressed(e.KeyCode);

            Refresh();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            KeysMine.Released(e.KeyCode);

            Refresh();
        }
    }
}
