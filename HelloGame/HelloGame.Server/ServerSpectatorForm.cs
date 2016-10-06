using System.Drawing;
using System.Windows.Forms;
using HelloGame.Common.Rendering;

namespace HelloGame.Server
{
    public partial class ServerSpectatorForm : Form
    {
        private readonly Renderer _renderer;

        public ServerSpectatorForm(Renderer renderer)
        {
            InitializeComponent();
            _renderer = renderer;
            _renderer.RepaintAction = Refresh;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _renderer.PaintStuff(e.Graphics, new Size(Width, Height), true);
        }
    }
}