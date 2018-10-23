namespace WebServer.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Http.Models;
    using Http.Models.Requests;
    using Http.Models.Responses;

    public class ServerRoutingTable
    {
        public ServerRoutingTable()
        {
            Routes = new Dictionary<HttpRequestMethod, Dictionary<string, Func<IHttpRequest, IHttpResponse>>>();

            foreach (HttpRequestMethod httpRequestMethod in Enum.GetValues(typeof(HttpRequestMethod))
                                                                .Cast<HttpRequestMethod>())
            {
                Routes[httpRequestMethod] = new Dictionary<string, Func<IHttpRequest, IHttpResponse>>();
            }
        }

        public Dictionary<HttpRequestMethod, Dictionary<string, Func<IHttpRequest, IHttpResponse>>> Routes { get; }
    }
}