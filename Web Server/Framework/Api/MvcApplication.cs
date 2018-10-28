namespace Framework.Api
{
    using Framework.Dependency;

    public class MvcApplication : IMvcApplication
    {
        public virtual void Configure()
        {
        }

        public virtual void ConfigureServices(IDependencyContainer dependencyContainer)
        {
        }
    }
}