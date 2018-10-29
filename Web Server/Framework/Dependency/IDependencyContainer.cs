namespace Framework.Dependency
{
    using System;

    public interface IDependencyContainer
    {
        void RegisterDependency<TImplementation>();

        void RegisterDependency<TService, TImplementation>() where TImplementation : TService;

        T CreateInstance<T>();

        object CreateInstance(Type type);
    }
}