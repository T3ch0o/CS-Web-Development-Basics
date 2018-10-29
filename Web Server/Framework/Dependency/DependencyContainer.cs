namespace Framework.Dependency
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class DependencyContainer : IDependencyContainer
    {
        private readonly IDictionary<Type, Type> _dependencies = new Dictionary<Type, Type>();

        public void RegisterDependency<TImplementation>()
        {
            RegisterDependency<TImplementation, TImplementation>();
        }

        public void RegisterDependency<TService, TImplementation>() where TImplementation : TService
        {
            _dependencies[typeof(TService)] = typeof(TImplementation);
        }

        public T CreateInstance<T>()
        {
            return (T)CreateInstance(typeof(T));
        }

        public object CreateInstance(Type type)
        {
            Type instanceType = _dependencies.ContainsKey(type) ? _dependencies[type] : type;

            if (instanceType.IsAbstract)
            {
                throw new ArgumentException($"Abstract type '{instanceType.FullName}' cannot be instantiated", nameof(type));
            }

            ConstructorInfo targetConstructor = instanceType.GetConstructors(BindingFlags.Public)
                                                            .FirstOrDefault();

            if (targetConstructor == null)
            {
                throw new InvalidOperationException($"Cannot find public constructor to instantiate type {instanceType.FullName}");
            }

            ParameterInfo[] parameters = targetConstructor.GetParameters();
            object[] arguments = new object[parameters.Length];

            for (int parameterIndex = 0; parameterIndex < parameters.Length; ++parameterIndex)
            {
                arguments[parameterIndex] = CreateInstance(parameters[parameterIndex].ParameterType);
            }

            return targetConstructor.Invoke(arguments);
        }
    }
}