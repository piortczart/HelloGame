using System;
using System.Threading;
using System.Windows.Forms;
using HelloGame.Client.Properties;
using HelloGame.Common.Model;

namespace HelloGame.Client
{
    public partial class InitialSetup : UserControl
    {
        public ClientNetwork ClientNetwork { private get; set; }
        public Server.GameServer Server { private get; set; }
        public GameManager GameManager { get; set; }
        public CancellationTokenSource Cancellation { private get; set; }

        public InitialSetup()
        {
            InitializeComponent();

            tbServerName.Text = Settings.Default.ServerName;
            tbPlayerName.Text = Settings.Default.PlayerName;

            cbIsLocal.Checked = true;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbPlayerName.Text))
            {
                MessageBox.Show("Gief name!", "You a hacker?!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Settings.Default.ServerName = tbServerName.Text;
            Settings.Default.PlayerName = tbPlayerName.Text;
            Settings.Default.Save();

            int port = 12152;

            if (cbIsLocal.Checked)
            {
                try
                {
                    var cts = new CancellationTokenSource();
                    Server.Start(cts, port);
                }
                catch (Exception exception)
                {
                    lbLog.Text = "Staring server failed: " + exception.Message;
                    return;
                }
            }

            try
            {
                ClientNetwork.StartConnection(Settings.Default.ServerName, Settings.Default.PlayerName, Cancellation, port);
            }
            catch (Exception exception)
            {
                lbLog.Text = "Connecting to server failed: " + exception.Message;
            }

            lbLog.Text = "We are in!";

            ((Form)this.TopLevelControl).Close();
        }

        private void cbIsLocal_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsLocal.Checked)
            {
                tbServerName.Text = "localhost";
            }
            tbServerName.Enabled = !cbIsLocal.Checked;
        }
    }
}
