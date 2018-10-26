namespace Demo
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Demo.Attributes;
    using Demo.Controllers;

    using Http.Models;
    using Http.Models.Responses;

    using WebServer;
    using WebServer.Routing;

    internal static class Program
    {
        private static async Task Main()
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            foreach (Type controllerType in Assembly.GetExecutingAssembly()
                                                    .GetTypes()
                                                    .Where(type => type.Namespace == "Demo.Controllers" && type.Name != "ControllerBase"))
            {
                foreach (MethodInfo method in controllerType.GetMethods())
                {
                    foreach (RouteAttribute routeAttribute in method.GetCustomAttributes<RouteAttribute>())
                    {
                        HttpRequestMethod requestMethod;

                        switch (routeAttribute)
                        {
                            case HttpGetAttribute _:
                                requestMethod = HttpRequestMethod.GET;
                                break;

                            case HttpPostAttribute _:
                                requestMethod = HttpRequestMethod.POST;
                                break;

                            default:
                                continue;
                        }

                        serverRoutingTable.RegisterOrOverwriteRoute(requestMethod,
                                                                    routeAttribute.Route,
                                                                    request =>
                                                                    {
                                                                        ControllerBase controller = (ControllerBase)Activator.CreateInstance(controllerType);

                                                                        controller.Request = request;

                                                                        return (IHttpResponse)method.Invoke(controller, null);
                                                                    });
                    }
                }
            }

            Server server = new Server(5000, serverRoutingTable);

            await server.Run();
        }
    }
}