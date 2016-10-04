using System.Threading;
using HelloGame.Client;
using HelloGame.Common.Logging;
using HelloGame.Common.Model;

namespace HelloGame.GraphicalModelTest
{
    public partial class Form1 : HelloGameForm
    {
        public Form1(Renderer renderer, InitialSetupForm setupForm, GameManager gameManager, ILoggerFactory loggerFactory, CancellationTokenSource cancellation) 
            : base(renderer, setupForm, gameManager, loggerFactory, cancellation, false)
        {
            gameManager.AddPlayer("dupa");
        }
    }
}
