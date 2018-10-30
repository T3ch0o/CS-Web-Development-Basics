namespace Framework.Routers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Framework.ActionResults;
    using Framework.Attributes;
    using Framework.Attributes.Action;
    using Framework.Attributes.Property;
    using Framework.Controllers;
    using Framework.Dependency;
    using Framework.Extensions;
    using Framework.Security;
    using Framework.Views;

    using Http.Extensions;
    using Http.Models;
    using Http.Models.Requests;
    using Http.Models.Responses;

    using WebServer.Api;
    using WebServer.Results;

    public class ControllerRouter : IHttpHandler
    {
        private readonly MvcContext _mvcContext;

        private readonly ViewEngine _viewEngine;

        private readonly IDependencyContainer _dependencyContainer;

        public ControllerRouter(MvcContext mvcContext, IDependencyContainer dependencyContainer)
        {
            _mvcContext = mvcContext;
            _dependencyContainer = dependencyContainer;
            _viewEngine = new ViewEngine(mvcContext, new ViewReader());
        }

        public IHttpResponse Handle(IHttpRequest request)
        {
            string controllerName;
            string actionName;

            if (request.Path == "/")
            {
                controllerName = "Home";
                actionName = "Index";
            }
            else
            {
                string[] pathParts = request.Path.ToLower().Split('/', StringSplitOptions.RemoveEmptyEntries);

                controllerName = pathParts.First().Capitalize();
                actionName = pathParts.Last().Capitalize();
            }

            string controllerAssemblyQualifiedName = string.Concat(_mvcContext.AssemblyName, ".", _mvcContext.ControllersNamespace, ".", controllerName, _mvcContext.ControllersSuffix, ", ", _mvcContext.AssemblyName);

            Type controllerType = Type.GetType(controllerAssemblyQualifiedName);

            if (controllerType == null)
            {
                return RenderNotFound(request);
            }

            MethodInfo targetMethod = FindMethod(controllerType, actionName, request.Method);

            if (targetMethod == null)
            {
                return RenderNotFound(request);
            }
            else
            {
                Controller controller = (Controller)_dependencyContainer.CreateInstance(controllerType);

                controller.ViewEngine = _viewEngine;
                controller.Request = request;
                controller.MvcContext = _mvcContext;

                if (targetMethod.GetCustomAttributes<AuthorizeAttribute>()
                                .Any(attribute => !attribute.IsAuthorized(controller.Identity)))
                {
                    return new WebServer.Results.RedirectResult("/");
                }

                ParameterInfo[] parameters = targetMethod.GetParameters();
                object[] arguments = new object[parameters.Length];

                for (int parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
                {
                    ParameterInfo parameter = parameters[parameterIndex];

                    Type parameterType = parameter.ParameterType;

                    if (parameterType.IsPrimitiveOrString())
                    {
                        arguments[parameterIndex] = Convert.ChangeType(request.FormData[parameter.Name], parameterType);
                    }
                    else
                    {
                        arguments[parameterIndex] = InstantiateAndFill(parameterType, request.FormData);
                    }

                    controller.ModelState.IsValid = IsValidModel(arguments[parameterIndex]);
                }

                IActionResult actionResult = (IActionResult)targetMethod.Invoke(controller, arguments);

                string result = actionResult.Invoke();

                switch (actionResult)
                {
                    case IRedirectable _:
                        return new WebServer.Results.RedirectResult(result);

                    case IViewable _:
                        return new HtmlResult(result, HttpStatusCode.OK);
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

        private static object InstantiateAndFill(Type objectType, IDictionary<string, object> formData)
        {
            object instance = Activator.CreateInstance(objectType);

            foreach (PropertyInfo property in objectType.GetProperties())
            {
                if (property.PropertyType.IsPrimitiveOrString())
                {
                    property.SetValue(instance, Convert.ChangeType(formData[property.Name.ToLower()], property.PropertyType));
                }
                else
                {
                    property.SetValue(instance, InstantiateAndFill(property.PropertyType, formData));
                }
            }

            return instance;
        }

        private static bool IsValidModel(object model)
        {
            foreach (PropertyInfo property in model.GetType().GetProperties())
            {
                ValidationAttribute[] validationAttributes = property.GetCustomAttributes<ValidationAttribute>().ToArray();

                if (validationAttributes.Length > 0)
                {
                    if (validationAttributes.Any(validationAttribute => !validationAttribute.IsValid(property.GetValue(model))))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private HtmlResult RenderNotFound(IHttpRequest request)
        {
            if (request.Session.ContainsParameter("auth"))
            {
                return new HtmlResult(_viewEngine.RenderError("404 NotFound", ((IIdentity)request.Session.GetParameter("auth")).Role), HttpStatusCode.NotFound);
            }

            return new HtmlResult(_viewEngine.RenderError("404 NotFound", "Guest"), HttpStatusCode.NotFound);
        }
    }
}