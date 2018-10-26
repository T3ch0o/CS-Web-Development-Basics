namespace FrameworkTest
{
    using System.Threading.Tasks;

    using Framework;
    using Framework.Routers;

    using WebServer;

    internal static class Program
    {
        private static async Task Main()
        {
            await MvcEngine.Run(new Server(8000, new ControllerRouter()));
        }
    }
}