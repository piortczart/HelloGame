using System.Windows.Forms;

namespace HelloGame
{
    public partial class HelloGameForm : Form
    {
        public KeysInfo KeysMine = new KeysInfo();
        GameState _game;

        public HelloGameForm()
        {
            InitializeComponent();
            _game = new GameState(this, KeysMine);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _game.PaintStuff(e.Graphics);
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
