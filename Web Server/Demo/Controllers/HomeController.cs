namespace Demo.Controllers
{
    using Http.Models;
    using Http.Models.Responses;

    using WebServer.Results;

    public class HomeController
    {
        public IHttpResponse Index()
        {
            return new HtmlResult("<h1>Hello World!</h1>", HttpStatusCode.OK);
        }
    }
}