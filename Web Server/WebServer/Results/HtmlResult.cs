namespace WebServer.Results
{
    using Http.Models;
    using Http.Models.Responses;

    public class HtmlResult : HttpResponse
    {
        public HtmlResult(HttpStatusCode statusCode) : base(statusCode)
        {
            Headers.Add("Content-Type", "text/html");
        }
    }
}