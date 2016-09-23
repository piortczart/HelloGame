using System.Windows.Forms;
using HelloGame.Properties;

namespace HelloGame
{
    public partial class InitialSetup : UserControl
    {
        public ClientNetwork ClientNetwork { get; set; }

        public InitialSetup()
        {
            InitializeComponent();

            tbServerName.Text = Settings.Default.ServerName;
            tbPlayerName.Text = Settings.Default.PlayerName;
        }

        private void btnPlay_Click(object sender, System.EventArgs e)
        {
            Settings.Default.ServerName = tbServerName.Text;
            Settings.Default.PlayerName = tbPlayerName.Text;

            int port = 12152;

            if (cbIsLocal.Checked)
            {
                try
                {
                    ClientState.Instance.Server.Start(port);

                }
                catch (System.Exception exception)
                {
                    lbLog.Text = "Staring server failed: " + exception.Message;
                }
            }

            try
            {
                ClientNetwork.StartConnection(Settings.Default.ServerName, Settings.Default.PlayerName, port);
            }
            catch (System.Exception exception)
            {
                lbLog.Text = "Connecting to server failed: " + exception.Message;
            }

            lbLog.Text = "We are in!";
        }

        private void cbIsLocal_CheckedChanged(object sender, System.EventArgs e)
        {
            if (cbIsLocal.Checked)
            {
                tbServerName.Text = "localhost";
            }
            tbServerName.Enabled = !cbIsLocal.Checked;
        }
    }
}
