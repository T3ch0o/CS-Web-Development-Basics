namespace Framework
{
    using System.Reflection;
    using System.Threading.Tasks;

    using WebServer;

    public static class MvcEngine
    {
        public static async Task Run(Server server)
        {
            MvcContext mvcContext = MvcContext.Instance;

            mvcContext.AssemblyName = Assembly.GetEntryAssembly().GetName().Name;
            mvcContext.ControllersNamespace = "Controllers";
            mvcContext.ControllersSuffix = "Controller";
            mvcContext.ViewsFolder = "Views";
            mvcContext.ModelsFolder = "Models";

            await server.Run();
        }
    }
}