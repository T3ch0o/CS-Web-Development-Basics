namespace Torshia
{
    using Framework.Api;
    using Framework.Dependency;

    using Torshia.Database;
    using Torshia.Services;
    using Torshia.Services.Interfaces;

    public class Startup : MvcApplication
    {
        public override void ConfigureServices(IDependencyContainer dependencyContainer)
        {
            dependencyContainer.RegisterDependency<TorshiaDbContext>();
            dependencyContainer.RegisterDependency<IHashService, HashService>();
            dependencyContainer.RegisterDependency<IUserCookieService, UserCookieService>();
        }
    }
}