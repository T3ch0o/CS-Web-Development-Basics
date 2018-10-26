namespace Framework.Controllers
{
    using System.Runtime.CompilerServices;

    using Framework.ActionResults;
    using Framework.Utilities;

    using Http.Models.Requests;

    public class Controller
    {
        public IHttpRequest Request { protected get; set; }

        protected IViewable View([CallerMemberName] string caller = default)
        {
            string controllerName = ControllerUtilities.GetControllerName(this);

            string viewPath = ControllerUtilities.GetActionViewPath(controllerName, caller);

            return new ViewResult(new View(viewPath));
        }

        protected IRedirectable Redirect(string url)
        {
            return new RedirectResult(url);
        }
    }
}