namespace WebServer.Results
{
    using System.Text;

    using Http.Models;
    using Http.Models.Responses;

    public class TextResult : HttpResponse
    {
        public TextResult(string content, HttpStatusCode statusCode) : base(statusCode)
        {
            BodyBytes = Encoding.ASCII.GetBytes(content);
            Headers.Add("Content-Type", "text/plain");
        }
    }
}