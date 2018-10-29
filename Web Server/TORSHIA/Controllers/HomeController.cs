namespace Torshia.Controllers
{
    using Framework.ActionResults;
    using Framework.Controllers;

    internal class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}