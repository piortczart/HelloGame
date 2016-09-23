namespace HelloGame
{
    public class ClientState
    {
        private static ClientState _instance;
        public static ClientState Instance => _instance ?? (_instance = new ClientState());
        public ClientNetwork Network => new ClientNetwork();
        public Server.Server Server => new Server.Server();
    }
}