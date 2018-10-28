namespace WebServer.Results
{
    using System.Text;

    using Http.Models;
    using Http.Models.Headers;
    using Http.Models.Responses;

    public class UnauthorizedResult : HttpResponse
    {
        private const string DefaultErrorHeading = "<h1>You do not have permission to access this functionality.</h1>";

        public UnauthorizedResult() : base(HttpStatusCode.Unauthorized)
        {
            Headers.Add(new HttpHeader("Content-Type", "text/html"));
            BodyBytes = Encoding.UTF8.GetBytes(DefaultErrorHeading);
        }
    }
}