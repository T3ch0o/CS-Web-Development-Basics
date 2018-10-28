namespace Framework.Api
{
    using Framework.Dependency;

    public interface IMvcApplication
    {
        void Configure();

        void ConfigureServices(IDependencyContainer dependencyContainer);
    }
}