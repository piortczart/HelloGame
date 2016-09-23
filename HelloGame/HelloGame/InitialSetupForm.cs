using System;
using System.Windows.Forms;

namespace HelloGame
{
    public partial class InitialSetupForm : Form
    {
        public InitialSetupForm(ClientNetwork clientNetwork)
        {
            InitializeComponent();

            initialSetup1.ClientNetwork = clientNetwork;
        }

        private void initialSetup1_Load(object sender, EventArgs e)
        {

        }
    }
}
