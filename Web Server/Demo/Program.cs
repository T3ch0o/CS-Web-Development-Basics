namespace Demo
{
    using System.Threading.Tasks;

    using Demo.Controllers;

    using Http.Models;

    using WebServer;
    using WebServer.Routing;

    internal static class Program
    {
        private static async Task Main()
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.RegisterOrOverwriteRoute(HttpRequestMethod.GET, "/", request => new HomeController().Index());

            Server server = new Server(5000, serverRoutingTable);

            await server.Run();
        }
    }
}