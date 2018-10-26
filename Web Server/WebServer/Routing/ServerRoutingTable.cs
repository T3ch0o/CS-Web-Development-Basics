namespace WebServer.Routing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Http.Models;
    using Http.Models.Requests;
    using Http.Models.Responses;

    using WebServer.Api;
    using WebServer.Results;

    public class ServerRoutingTable : IHttpHandler
    {
        private readonly Dictionary<HttpRequestMethod, Dictionary<string, Func<IHttpRequest, IHttpResponse>>> _routes;

        public ServerRoutingTable()
        {
            _routes = new Dictionary<HttpRequestMethod, Dictionary<string, Func<IHttpRequest, IHttpResponse>>>();

            foreach (HttpRequestMethod httpRequestMethod in Enum.GetValues(typeof(HttpRequestMethod))
                                                                .Cast<HttpRequestMethod>())
            {
                _routes[httpRequestMethod] = new Dictionary<string, Func<IHttpRequest, IHttpResponse>>();
            }
        }

        public void RegisterOrOverwriteRoute(HttpRequestMethod httpRequestMethod, string path, Func<IHttpRequest, IHttpResponse> handler)
        {
            _routes[httpRequestMethod][path] = handler;
        }

        public IHttpResponse Handle(IHttpRequest request)
        {
            if (_routes.TryGetValue(request.Method, out var pathRoutes)) // PathRoutes is inner path dictionary
            {
                if (pathRoutes.TryGetValue(request.Path, out Func<IHttpRequest, IHttpResponse> handler))
                {
                    return handler(request);
                }
            }

            string resourcePath = string.Concat("../Resources/", request.Path);

            if (File.Exists(resourcePath))
            {
                return new InlineResourceResult(File.ReadAllBytes(resourcePath), HttpStatusCode.OK);
            }

            return new HttpResponse(HttpStatusCode.NotFound);
        }
    }
}