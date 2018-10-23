namespace WebServer.Results
{
    using System.Text;

    using Http.Models;
    using Http.Models.Responses;

    public class HtmlResult : HttpResponse
    {
        public HtmlResult(string content, HttpStatusCode statusCode) : base(statusCode)
        {
            BodyBytes = Encoding.ASCII.GetBytes(content);
            Headers.Add("Content-Type", "text/html");
        }
    }
}