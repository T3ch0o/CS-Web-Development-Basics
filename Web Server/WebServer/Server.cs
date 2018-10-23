namespace WebServer
{
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    using WebServer.Routing;

    public class Server
    {
        private const string ServerIp = "127.0.0.1";

        private readonly ServerRoutingTable _serverRoutingTable;

        private readonly TcpListener _tcpListener;

        private readonly CancellationToken _cancellationToken = new CancellationToken();

        public Server(int port, ServerRoutingTable serverRoutingTable)
        {
            _serverRoutingTable = serverRoutingTable;
            _tcpListener = new TcpListener(IPAddress.Parse(ServerIp), port);
        }

        public void Run()
        {
            _tcpListener.Start();
            ListenLoop(_cancellationToken).GetAwaiter().GetResult();
        }

        public async Task ListenLoop(CancellationToken cancellationToken)
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                Socket clientSocket = await _tcpListener.AcceptSocketAsync();

                ConnectionHandler connectionHandler = new ConnectionHandler(clientSocket, _serverRoutingTable);
                Task.Run(connectionHandler.ProcessRequestAsync, cancellationToken);
            }
        }
    }
}