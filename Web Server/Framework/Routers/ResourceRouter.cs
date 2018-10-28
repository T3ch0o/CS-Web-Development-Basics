namespace Framework.Routers
{
    using System.IO;

    using Http.Models;
    using Http.Models.Requests;
    using Http.Models.Responses;

    using WebServer.Api;
    using WebServer.Results;

    public class ResourceRouter : IHttpHandler
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            string resourcePath = string.Concat("../Resources/", request.Path);

            if (File.Exists(resourcePath))
            {
                return new InlineResourceResult(File.ReadAllBytes(resourcePath), HttpStatusCode.OK);
            }

            return new HttpResponse(HttpStatusCode.NotFound);
        }
    }
}