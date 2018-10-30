namespace Framework.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    using Framework.ActionResults;
    using Framework.Models;
    using Framework.Security;
    using Framework.Views;

    using Http.Models.Requests;

    public class Controller
    {
        public MvcContext MvcContext { protected get; set; }

        public IHttpRequest Request { protected get; set; }

        public IIdentity Identity => (IIdentity)Request.Session.GetParameter("auth");

        public Model ModelState { get; } = new Model();

        public IViewEngine ViewEngine { private get; set; }

        protected IDictionary<string, object> PropertyBag { get; } = new Dictionary<string, object>();

        protected IViewable View([CallerMemberName] string actionName = default)
        {
            string controllerName = GetType().Name.Replace(MvcContext.ControllersSuffix, string.Empty);

            string viewContent;

            try
            {
                viewContent = ViewEngine.RenderView(controllerName, actionName, PropertyBag);
            }
            catch (Exception e)
            {
                viewContent = ViewEngine.RenderError(e.Message, PropertyBag["role"].ToString());
            }

            return new ViewResult(new View(viewContent));
        }

        protected IRedirectable Redirect(string url)
        {
            return new RedirectResult(url);
        }

        protected void SignIn(IIdentity authorization)
        {
            Request.Session.AddParameter("auth", authorization);
        }

        protected void SignOut()
        {
            Request.Session.ClearParameters();
        }
    }
}