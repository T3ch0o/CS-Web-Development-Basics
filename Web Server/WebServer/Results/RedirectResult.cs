namespace WebServer.Results
{
    using Http.Models;
    using Http.Models.Responses;

    public class RedirectResult : HttpResponse
    {
        public RedirectResult(string location) : base(HttpStatusCode.SeeOther)
        {
            Headers.Add("Location", location);
        }
    }
}