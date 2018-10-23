namespace WebServer
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    using Http.Models;
    using Http.Models.Requests;
    using Http.Models.Responses;

    using WebServer.Routing;

    public class ConnectionHandler
    {
        private const int ChunkSize = 4096;

        private readonly Socket _clientSocket;

        private readonly ServerRoutingTable _serverRoutingTable;

        public ConnectionHandler(Socket clientSocket, ServerRoutingTable serverRoutingTable)
        {
            _clientSocket = clientSocket;
            _serverRoutingTable = serverRoutingTable;
        }

        public async Task ProcessRequestAsync()
        {
            Task<IHttpRequest> requestTask = ReadRequest();

            IHttpRequest request = await requestTask;

            IHttpResponse response;

            if (requestTask.IsFaulted)
            {
                if (requestTask.Exception.InnerException is ArgumentException)
                {
                    response = new HttpResponse(HttpStatusCode.BadRequest);
                }
                else
                {
                    response = new HttpResponse(HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                response = _serverRoutingTable.HandleRequest(request);
            }

            await SendResponse(response);

            _clientSocket.Dispose();
        }

        private async Task<IHttpRequest> ReadRequest()
        {
            Memory<byte> readBuffer = new Memory<byte>(new byte[ChunkSize]);
            StringBuilder stringBuilder = new StringBuilder();

            while (true)
            {
                int bytesRead = await _clientSocket.ReceiveAsync(readBuffer, SocketFlags.None);

                stringBuilder.Append(Encoding.ASCII.GetString(readBuffer.Span.Slice(0, bytesRead)));

                if (bytesRead < ChunkSize)
                {
                    break;
                }
            }

            return HttpRequest.Parse(stringBuilder.ToString());
        }

        private async Task SendResponse(IHttpResponse response)
        {
            Memory<byte> responseData = new Memory<byte>(response.FormResponseBytes());
            await _clientSocket.SendAsync(responseData, SocketFlags.None);
        }
    }
}