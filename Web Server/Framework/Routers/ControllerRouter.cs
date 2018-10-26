namespace Framework.Routers
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Framework.ActionResults;
    using Framework.Attributes;
    using Framework.Controllers;

    using Http.Extensions;
    using Http.Models;
    using Http.Models.Requests;
    using Http.Models.Responses;

    using WebServer.Api;
    using WebServer.Results;

    public class ControllerRouter : IHttpHandler
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            string[] pathParts = request.Path.ToLower().Split('/', StringSplitOptions.RemoveEmptyEntries);

            string controllerName = pathParts.First().Capitalize();
            string actionName = pathParts.Last().Capitalize();

            MvcContext mvcContext = MvcContext.Instance;

            string controllerAssemblyQualifiedName = string.Concat(mvcContext.AssemblyName, ".", mvcContext.ControllersNamespace, ".", controllerName, mvcContext.ControllersSuffix, ", ", mvcContext.AssemblyName);

            Type controllerType = Type.GetType(controllerAssemblyQualifiedName);

            MethodInfo targetMethod = FindMethod(controllerType, actionName, request.Method);

            if (targetMethod != null)
            {
                Controller controller = (Controller)Activator.CreateInstance(controllerType);

                controller.Request = request;

                IActionResult actionResult = (IActionResult)targetMethod.Invoke(controller, null);

                switch (actionResult)
                {
                    case IRedirectable redirectable:
                        return new WebServer.Results.RedirectResult(redirectable.RedirectUrl);

                    case IViewable viewable:
                        return new HtmlResult(viewable.View.Render(), HttpStatusCode.OK);
                }
            }

            return new HttpResponse(HttpStatusCode.NotFound);
        }

        private static MethodInfo FindMethod(Type controllerType, string actionName, HttpRequestMethod requestMethod)
        {
            MethodInfo[] methods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                 .Where(method => method.Name == actionName)
                                                 .ToArray();

            foreach (MethodInfo method in methods)
            {
                HttpMethodAttribute httpMethodAttribute = method.GetCustomAttribute<HttpMethodAttribute>();

                if (httpMethodAttribute == null)
                {
                    if (requestMethod == HttpRequestMethod.GET)
                    {
                        return method;
                    }
                }
                else if (httpMethodAttribute.Accepts(requestMethod))
                {
                    return method;
                }
            }

            return null;
        }
    }
}