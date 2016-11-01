using System;
using System.Threading;
using System.Windows.Forms;
using HelloGame.Common.Model;

namespace HelloGame.Client
{
    public partial class InitialSetupForm : Form
    {
        public InitialSetupForm(ClientNetwork clientNetwork, GameManager gameManager,
            CancellationTokenSource cancellation)
        {
            InitializeComponent();

            initialSetup1.ClientNetwork = clientNetwork;
            initialSetup1.GameManager = gameManager;
            initialSetup1.Cancellation = cancellation;
        }

        private void initialSetup1_Load(object sender, EventArgs e)
        {
        }
    }
}