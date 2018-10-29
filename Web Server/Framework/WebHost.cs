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
        private const int Port = 80;

        public static async Task Start<TStartup>() where TStartup : IMvcApplication, new()
        {
            MvcContext mvcContext = new MvcContext
            {
                AssemblyName = Assembly.GetEntryAssembly().GetName().Name,
                ControllersNamespace = "Controllers",
                ControllersSuffix = "Controller",
                ViewsFolder = "Views",
                ModelsFolder = "Models"
            };

            await Start<TStartup>(mvcContext);
        }

        public static async Task Start<TStartup>(MvcContext mvcContext) where TStartup : IMvcApplication, new()
        {
            TStartup mvcApplication = new TStartup();

            mvcApplication.Configure();

            DependencyContainer container = new DependencyContainer();
            mvcApplication.ConfigureServices(container);

            ControllerRouter controllerRouter = new ControllerRouter(mvcContext);
            ResourceRouter resourceRouter = new ResourceRouter();

            Server server = new Server(Port, controllerRouter, resourceRouter);

            await server.Run();
        }
    }
}