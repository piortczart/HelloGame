using HelloGame.Common;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace HelloGame.Server
{
    class Program
    {
        private TcpListener tcpListener;
        private Thread listenThread;
        MessageTransciever sender = new MessageTransciever();

        SynchronizedCollection<NetworkStream> _clients = new SynchronizedCollection<NetworkStream>();

        public void Server()
        {
            tcpListener = new TcpListener(IPAddress.Any, 3000);
            listenThread = new Thread(new ThreadStart(ListenForClients));
            listenThread.Start();
        }

        private void ListenForClients()
        {
            tcpListener.Start();

            while (true)
            {
                // Blocks until a client has connected to the server
                TcpClient client = tcpListener.AcceptTcpClient();
                Task.Run(() => { HandleClientComm(client); });
            }
        }

        private void HandleClientComm(TcpClient client)
        {
            TcpClient tcpClient = client;
            NetworkStream clientStream = tcpClient.GetStream();
            _clients.Add(clientStream);

            // Construct a binary formatter
            BinaryFormatter formatter = new BinaryFormatter();

            // Deserialize the stream into object
            while (true)
            {
                NetworkMessage message = sender.Get(clientStream);
                message.Text += " No!";
                foreach(var otherClient in _clients.Where(c => c != clientStream).ToList())
                {
                    sender.Send(message, otherClient);
                }
                
                // Send it back for tests.
                sender.Send(message, clientStream);
            }
        }

        static void Main(string[] args)
        {
            new Program().Server();
        }
    }
}
