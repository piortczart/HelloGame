using System.Drawing;
using System.Windows.Forms;
using HelloGame.Common.Rendering;
using HelloGame.Common.Extensions;

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

            var timer = new Timer
            {
                Interval = 14
            };
            timer.Tick += (sender, args) => { this.Invoke(Refresh); };
            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _renderer.PaintStuff(e.Graphics, new Size(Width, Height), true);
        }
    }
}