using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using HelloGame.GameObjects;
using HelloGame.MathStuff;

namespace HelloGame
{
    public partial class HelloGameForm : Form
    {
        public KeysInfo KeysMine = new KeysInfo();
        public readonly Scene scene;

        public HelloGameForm()
        {
            InitializeComponent();

            scene = new Scene(this);

            var ship = new DaShip(KeysMine , scene);
            ship.Spawn(new Point(100, 100), new Real2DVector());

            scene.AddThing(ship);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            scene.PaintStuff(e.Graphics);
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
