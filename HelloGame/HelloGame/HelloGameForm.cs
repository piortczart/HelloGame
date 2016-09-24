using System.Drawing;
using System.Windows.Forms;
using HelloGame.Common;
using HelloGame.Common.Model;

namespace HelloGame
{
    public partial class HelloGameForm : Form
    {
        private readonly Renderer _renderer;
        private readonly GameManager _gameManager;
        private readonly KeysInfo _keysMine = new KeysInfo();
        private readonly EventPerSecond _paintCounter = new EventPerSecond();
        private readonly Font _font = new Font("Courier", 12, GraphicsUnit.Pixel);

        public HelloGameForm(Renderer renderer, InitialSetupForm setupForm, GameManager gameManager)
        {
            _renderer = renderer;
            _gameManager = gameManager;
            _renderer.RepaintAction = Refresh;

            InitializeComponent();

            setupForm.ShowDialog(this);

            gameManager.StartGame();

            gameManager.SetUpdateModelAction(()=> { this.Invoke(Refresh); });
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

            _gameManager.SetKeysInfo(_keysMine);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            _keysMine.Released(e.KeyCode);
            Refresh();
        }
    }
}
