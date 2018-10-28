namespace Framework.Dependency
{
    using System;

    public interface IDependencyContainer
    {
        void RegisterDependency<TImplementation>();

        void RegisterDependency<TService, TImplementation>() where TService : TImplementation;

        T CreateInstance<T>();

        object CreateInstance(Type type);
    }
}