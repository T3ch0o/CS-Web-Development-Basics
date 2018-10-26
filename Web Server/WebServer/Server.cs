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

        private readonly IHttpHandler _httpHandler;

        private readonly TcpListener _tcpListener;

        private readonly CancellationToken _cancellationToken = new CancellationToken();

        public Server(int port, IHttpHandler httpHandler)
        {
            _httpHandler = httpHandler;
            _tcpListener = new TcpListener(IPAddress.Parse(ServerIp), port);
        }

        public async Task Run()
        {
            _tcpListener.Start();
            await ListenLoop(_cancellationToken);
        }

        private async Task ListenLoop(CancellationToken cancellationToken)
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                Socket clientSocket = await _tcpListener.AcceptSocketAsync();

                ConnectionHandler connectionHandler = new ConnectionHandler(clientSocket, _httpHandler);
                Task.Run(connectionHandler.ProcessRequestAsync, cancellationToken);
            }
        }
    }
}