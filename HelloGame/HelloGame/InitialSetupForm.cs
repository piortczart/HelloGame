using System;
using System.Windows.Forms;
using HelloGame.Common.Model;

namespace HelloGame
{
    public partial class InitialSetupForm : Form
    {
        public InitialSetupForm(ClientNetwork clientNetwork, Server.GameServer server, GameManager gameManager)
        {
            InitializeComponent();

            initialSetup1.Server = server;
            initialSetup1.ClientNetwork = clientNetwork;
            initialSetup1.GameManager = gameManager;
        }

        private void initialSetup1_Load(object sender, EventArgs e)
        {
        }
    }
}
