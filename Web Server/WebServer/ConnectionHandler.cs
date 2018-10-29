namespace WebServer
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    using Http.Models;
    using Http.Models.Cookies;
    using Http.Models.Requests;
    using Http.Models.Responses;
    using Http.Models.Session;

    using WebServer.Api;

    public class ConnectionHandler
    {
        private const int ChunkSize = 4096;

        private readonly Socket _clientSocket;

        private readonly IHttpHandler _controllerHandler;

        private readonly IHttpHandler _resourceHandler;

        public ConnectionHandler(Socket clientSocket, IHttpHandler controllerHandler, IHttpHandler resourceHandler )
        {
            _clientSocket = clientSocket;
            _controllerHandler = controllerHandler;
            _resourceHandler = resourceHandler;
        }

        public async Task ProcessRequestAsync()
        {
            IHttpResponse response;

            try
            {
                IHttpRequest request = await ReadRequest();

                string sessionId = SetRequestSession(request);

                if (request.Path.Contains("/Resources"))
                {
                    response = _resourceHandler.Handle(request);
                }
                else
                {
                    response = _controllerHandler.Handle(request);
                }

                SetResponseSession(response, sessionId);
            }
            catch (ArgumentException e)
            {
                response = new HttpResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                response = new HttpResponse(HttpStatusCode.InternalServerError);
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

        private string SetRequestSession(IHttpRequest httpRequest)
        {
            string sessionId;

            if (httpRequest.Cookies.Contains(HttpSessionStorage.SessionCookieKey))
            {
                HttpCookie cookie = httpRequest.Cookies.GetCookie(HttpSessionStorage.SessionCookieKey);
                sessionId = cookie.Value;
                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            }
            else
            {
                sessionId = Guid.NewGuid().ToString();
                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            }

            return sessionId;
        }

        private void SetResponseSession(IHttpResponse response, string sessionId)
        {
            response.Cookies.Add(new HttpCookie(HttpSessionStorage.SessionCookieKey, $"{sessionId};HttpOnly=true"));
        }
    }
}