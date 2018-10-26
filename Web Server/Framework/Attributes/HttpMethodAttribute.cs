namespace Framework.Attributes
{
    using System;

    using Http.Models;

    [AttributeUsage(AttributeTargets.Method)]
    public abstract class HttpMethodAttribute : Attribute
    {
        private protected abstract HttpRequestMethod HttpRequestMethod { get; }

        public bool Accepts(HttpRequestMethod method)
        {
            return method == HttpRequestMethod;
        }
    }
}