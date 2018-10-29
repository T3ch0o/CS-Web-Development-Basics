namespace Torshia.Controllers
{
    using Framework.ActionResults;
    using Framework.Controllers;

    internal class HomeController : Controller
    {
        IActionResult Index()
        {
            return View();
        }
    }
}