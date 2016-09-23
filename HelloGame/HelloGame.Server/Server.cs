using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HelloGame.Common;
using HelloGame.Common.Model;
using HelloGame.Common.Network;

namespace HelloGame.Server
{
    public class Server
    {
        private TcpListener _tcpListener;
        private readonly MessageTransciever _sender = new MessageTransciever();
        private readonly SynchronizedCollection<NetworkStream> _clients = new SynchronizedCollection<NetworkStream>();

        private readonly GameManager _gameManager;
        private Thread _listenThread;
        private Timer _propagateTimer;

        public Server()
        {
            _gameManager = new GameManager(new ModelManager());
        }

        public void Start(int port = 49182)
        {
            _tcpListener = new TcpListener(IPAddress.Any, port);
            _tcpListener.Start();

            _gameManager.StartGame();

            _listenThread = new Thread(Process);
            _listenThread.Start();

            _propagateTimer = new Timer(state => { Propagate(); }, null, 1000, 2000);
        }

        private void Propagate()
        {
            string message = _gameManager.ModelManager.GetThings().Select(t => t.Physics).SerializeJson();
            SendToAll(message);
        }

        private void Process()
        {
            while (true)
            {
                // Blocks until a client has connected to the server
                TcpClient client = _tcpListener.AcceptTcpClient();
                Task.Run(() => { HandleClientComm(client); });
            }
        }

        public void SendToAll(string message)
        {
            foreach (var networkStream in _clients)
            {
                _sender.Send(new NetworkMessage { Text = message }, networkStream);
            }
        }

        private void HandleClientComm(TcpClient client)
        {
            TcpClient tcpClient = client;
            NetworkStream clientStream = tcpClient.GetStream();
            _clients.Add(clientStream);

            // Deserialize the stream into object
            while (true)
            {
                NetworkMessage message = _sender.Get(clientStream);
                message.Text += " No!";
                foreach (var otherClient in _clients.Where(c => c != clientStream).ToList())
                {
                    try
                    {
                        _sender.Send(message, otherClient);

                    }
                    catch
                    {
                        // Ignore
                    }
                }

                // Send it back for tests.
                _sender.Send(message, clientStream);
            }
        }

    }
}