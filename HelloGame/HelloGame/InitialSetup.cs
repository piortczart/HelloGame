using System;
using System.Threading;
using System.Windows.Forms;
using HelloGame.Client.Properties;
using HelloGame.Common.Model;
using System.Threading.Tasks;
using HelloGame.Common.Model.GameObjects.Ships;
using System.Linq;

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
            cbCreateServer.Checked = Settings.Default.SpawnServer;

            cbClan.DataSource = Enum.GetValues(typeof(ClanEnum)).Cast<ClanEnum>();
            cbClan.SelectedItem = (ClanEnum) Settings.Default.ClanId;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbPlayerName.Text))
            {
                MessageBox.Show("Gief name!", "You a hacker?!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ClanEnum clan = (ClanEnum) cbClan.SelectedItem;

            Settings.Default.ServerName = tbServerName.Text;
            Settings.Default.PlayerName = tbPlayerName.Text;
            Settings.Default.SpawnServer = cbCreateServer.Checked;
            Settings.Default.ClanId = (int) clan;
            Settings.Default.Save();

            if (cbCreateServer.Checked)
            {
                try
                {
                    var cts = new CancellationTokenSource();
                    Task.Run(async () => { await Server.Start(cts); })
                        .ContinueWith(t =>
                        {
                            ;
                        }, TaskContinuationOptions.OnlyOnFaulted);
                }
                catch (Exception exception)
                {
                    lbLog.Text = "Staring server failed: " + exception.Message;
                    return;
                }
            }

            string name = Settings.Default.PlayerName;

            if (cbLocalOnly.Checked)
            {
                // ?
                GameManager.StartGame();
                GameManager.AddPlayer(name, clan);
            }
            else
            {
                try
                {
                    ClientNetwork.StartConnection(Settings.Default.ServerName, name, clan, Cancellation);
                }
                catch (Exception exception)
                {
                    lbLog.Text = "Connecting to server failed: " + exception.Message;
                }
                lbLog.Text = "We are in!";
            }

            ((Form) this.TopLevelControl).Close();
        }

        private void cbIsLocal_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCreateServer.Checked)
            {
                tbServerName.Text = "localhost";
            }
            tbServerName.Enabled = !cbCreateServer.Checked;
        }
    }
}