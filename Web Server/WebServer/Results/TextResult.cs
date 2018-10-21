namespace WebServer.Results
{
    using Http.Models;
    using Http.Models.Responses;

    public class TextResult : HttpResponse
    {
        public TextResult(HttpStatusCode statusCode) : base(statusCode)
        {
            Headers.Add("Content-Type", "text/plain");
        }
    }
}