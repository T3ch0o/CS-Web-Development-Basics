namespace FrameworkTest.Controllers
{
    using Framework.ActionResults;
    using Framework.Attributes.Action;
    using Framework.Controllers;
    using Framework.Security;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            SignIn(new IdentityUser
            {
                    Username = "Pesho",
                    Password = "123"
            });

            return View();
        }

        [Authorize]
        public IActionResult Authorized()
        {
            return View();
        }
    }
}