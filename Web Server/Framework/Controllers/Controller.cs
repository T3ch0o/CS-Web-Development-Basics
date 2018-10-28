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
        private IViewEngine _viewEngine;

        private MvcContext _mvcContext;
        public MvcContext MvcContext
        {
            protected get => _mvcContext;

            set
            {
                _mvcContext = value;
                _viewEngine = new ViewEngine(value, new ViewReader());
            }
        }

        public IHttpRequest Request { protected get; set; }

        public IIdentity Identity => (IIdentity)Request.Session.GetParameter("auth");

        public Model ModelState { get; } = new Model();

        protected IDictionary<string, object> PropertyBag { get; } = new Dictionary<string, object>();

        protected IViewable View([CallerMemberName] string actionName = default)
        {
            string controllerName = GetType().Name.Replace(MvcContext.ControllersSuffix, string.Empty);

            string viewContent;

            try
            {
                viewContent = _viewEngine.RenderView(controllerName, actionName, PropertyBag);
            }
            catch (Exception e)
            {
                viewContent = _viewEngine.RenderError(e.Message);
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