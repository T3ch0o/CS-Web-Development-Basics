namespace Demo.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal abstract class RouteAttribute : Attribute
    {
        private protected RouteAttribute(string route)
        {
            Route = route;
        }

        public string Route { get; }
    }
}