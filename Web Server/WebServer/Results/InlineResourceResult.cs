namespace WebServer.Results
{
    using Http.Models;
    using Http.Models.Headers;
    using Http.Models.Responses;

    public class InlineResourceResult : HttpResponse
    {
        public InlineResourceResult(byte[] content, HttpStatusCode statusCode) : base(statusCode)
        {
            Headers.Add(new HttpHeader("Content-Length", content.Length.ToString()));
            Headers.Add(new HttpHeader("Content-Disposition", "inline"));
            Headers.Add("Content-Type", "text/css");

            BodyBytes = content;
        }
    }
}