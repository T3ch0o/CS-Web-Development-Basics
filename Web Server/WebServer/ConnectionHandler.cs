﻿namespace WebServer
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

                if (request == null)
                {
                    response = new HttpResponse(HttpStatusCode.BadRequest);
                }
                else
                {
                    string sessionId;

                    if (request.Cookies.TryGetValue(HttpSessionStorage.SessionCookieKey, out HttpCookie cookie))
                    {
                        sessionId = cookie.Value;
                    }
                    else
                    {
                        sessionId = Guid.NewGuid().ToString();
                    }

                    request.Session = HttpSessionStorage.GetSession(sessionId);

                    if (request.Path.Contains("/Resources") || request.Path.Contains('.'))
                    {
                        response = _resourceHandler.Handle(request);
                    }
                    else
                    {
                        response = _controllerHandler.Handle(request);
                    }

                    response.Cookies.Add(new HttpCookie(HttpSessionStorage.SessionCookieKey, sessionId, isHttpOnly: true));
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Bad Request Error\n{0}\n{1}\n", e.Message, e.StackTrace);
                response = new HttpResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine("Internal Server Error\n{0}\n{1}\n", e.Message,e.StackTrace);
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

            string request = stringBuilder.ToString();

            if (string.IsNullOrWhiteSpace(request))
            {
                return null;
            }

            return HttpRequest.Parse(request);
        }

        private async Task SendResponse(IHttpResponse response)
        {
            Memory<byte> responseData = new Memory<byte>(response.FormResponseBytes());
            await _clientSocket.SendAsync(responseData, SocketFlags.None);
        }
    }
}