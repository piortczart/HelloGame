using System.Drawing;
using System.Windows.Forms;
using HelloGame.Common;

namespace HelloGame
{
    public partial class HelloGameForm : Form
    {
        private readonly Renderer _renderer;
        private readonly KeysInfo _keysMine = new KeysInfo();
        private readonly EventPerSecond _paintCounter = new EventPerSecond();
        private readonly Font _font = new Font("Courier", 12, GraphicsUnit.Pixel);
        public readonly ClientNetwork Network = new ClientNetwork();

        public HelloGameForm(Renderer renderer)
        {
            _renderer = renderer;
            InitializeComponent();

            var ini = new InitialSetupForm(Network);
            ini.ShowDialog(this);
        }

        public void StartLocalServer()
        {
            var server = new global::HelloGame.Server.Server();
            server.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _renderer.PaintStuff(e.Graphics);

            _paintCounter.Add();

            e.Graphics.DrawString($"paints/s: {_paintCounter.GetPerSecond()}", _font, Brushes.Black, new PointF(10, 15));
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            _keysMine.Pressed(e.KeyCode);
            Refresh();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            _keysMine.Released(e.KeyCode);
            Refresh();
        }
    }
}
