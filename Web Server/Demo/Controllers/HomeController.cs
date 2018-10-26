namespace Demo.Controllers
{
    using Demo.Attributes;

    using Http.Models.Responses;

    public class HomeController : ControllerBase
    {
        [HttpGet("/")]
        [HttpGet("/home/index")]
        public IHttpResponse Index()
        {
            if (IsAuthenticated)
            {
                ViewPropertyBag["username"] = Request.Session.GetParameter<string>("username");
                return View("Views/Home/IndexLoggedIn");
            }

            return View("Views/Home/Index");
        }
    }
}