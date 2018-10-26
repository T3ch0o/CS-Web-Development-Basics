namespace WebServer.Api
{
    using Http.Models.Requests;
    using Http.Models.Responses;

    public interface IHttpHandler
    {
        IHttpResponse Handle(IHttpRequest request);
    }
}