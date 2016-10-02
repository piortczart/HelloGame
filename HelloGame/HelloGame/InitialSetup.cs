using System;
using System.Threading;
using System.Windows.Forms;
using HelloGame.Client.Properties;
using HelloGame.Common.Model;
using System.Threading.Tasks;

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
            cbCreateLocalServer.Checked = Settings.Default.SpawnServer;
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
            Settings.Default.SpawnServer = cbCreateLocalServer.Checked;
            Settings.Default.Save();

            if (cbCreateLocalServer.Checked)
            {
                try
                {
                    var cts = new CancellationTokenSource();
                    Task.Run(async () => { await Server.Start(cts); })
                        .ContinueWith(t => {
                            ;
                        }, TaskContinuationOptions.OnlyOnFaulted);
                }
                catch (Exception exception)
                {
                    lbLog.Text = "Staring server failed: " + exception.Message;
                    return;
                }
            }

            try
            {
                ClientNetwork.StartConnection(Settings.Default.ServerName, Settings.Default.PlayerName, Cancellation);
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
            if (cbCreateLocalServer.Checked)
            {
                tbServerName.Text = "localhost";
            }
            tbServerName.Enabled = !cbCreateLocalServer.Checked;
        }
    }
}
