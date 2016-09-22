using System;
using System.Drawing;
using System.Windows.Forms;

namespace HelloGame
{
    public partial class HelloGameForm : Form
    {
        public KeysInfo KeysMine = new KeysInfo();
        readonly GameState _game;
        public EventPerSecond PaintCounter = new EventPerSecond();
        protected Font font = new Font("Courier", 12, GraphicsUnit.Pixel);

        public HelloGameForm()
        {
            InitializeComponent();
            _game = new GameState(this, KeysMine);

            var n = new ClientNetwork();
            n.Connect();
            n.SendMyInfo();
            n.Receive();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _game.PaintStuff(e.Graphics);

            PaintCounter.Add();

            e.Graphics.DrawString($"paints/s: {PaintCounter.GetPerSecond()}", font, Brushes.Black, new PointF(10, 15));
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
