namespace Framework
{
    using System.Reflection;
    using System.Threading.Tasks;

    using Framework.Api;
    using Framework.Dependency;
    using Framework.Routers;

    using WebServer;

    public static class WebHost
    {
        private const int Port = 8000;

        public static async Task Start(IMvcApplication mvcApplication)
        {
            MvcContext mvcContext = new MvcContext
            {
                AssemblyName = Assembly.GetEntryAssembly().GetName().Name,
                ControllersNamespace = "Controllers",
                ControllersSuffix = "Controller",
                ViewsFolder = "Views",
                ModelsFolder = "Models"
            };

            await Start(mvcApplication, mvcContext);
        }

        public static async Task Start(IMvcApplication mvcApplication, MvcContext mvcContext)
        {
            IDependencyContainer container = new DependencyContainer();
            mvcApplication.ConfigureServices(container);

            ControllerRouter controllerRouter = new ControllerRouter(mvcContext);
            mvcApplication.Configure();

            Server server = new Server(Port, controllerRouter);

            await server.Run();
        }
    }
}