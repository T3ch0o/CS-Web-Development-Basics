namespace FrameworkTest
{
    using Framework.Api;
    using Framework.Dependency;

    public class Startup : MvcApplication
    {
        public override void ConfigureServices(IDependencyContainer dependencyContainer)
        {
            //dependencyContainer.RegisterDependency<IHomeService, HomeService>();
        }
    }
}