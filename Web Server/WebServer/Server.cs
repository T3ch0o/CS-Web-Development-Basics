namespace WebServer
{
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    using WebServer.Api;

    public class Server
    {
        private const string ServerIp = "127.0.0.1";

        private readonly IHttpHandler _controllerHandler;

        private readonly IHttpHandler _resourceHandler;

        private readonly TcpListener _tcpListener;

        private readonly CancellationToken _cancellationToken = new CancellationToken();

        public Server(int port, IHttpHandler controllerHandler, IHttpHandler resourceHandler)
        {
            _controllerHandler = controllerHandler;
            _resourceHandler = resourceHandler;
            _tcpListener = new TcpListener(IPAddress.Parse(ServerIp), port);
        }

        public async Task Run()
        {
            _tcpListener.Start();

            while (!_cancellationToken.IsCancellationRequested)
            {
                Socket clientSocket = await _tcpListener.AcceptSocketAsync();

                ConnectionHandler connectionHandler = new ConnectionHandler(clientSocket, _controllerHandler, _resourceHandler);
                Task.Run(connectionHandler.ProcessRequestAsync, _cancellationToken);
            }
        }
    }
}