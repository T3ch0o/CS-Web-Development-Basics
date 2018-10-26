namespace Demo.Attributes
{
    internal class HttpGetAttribute : RouteAttribute
    {
        internal HttpGetAttribute(string route) : base(route)
        {
        }
    }
}