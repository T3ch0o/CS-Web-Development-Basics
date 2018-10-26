namespace Demo.Attributes
{
    internal class HttpPostAttribute : RouteAttribute
    {
        internal HttpPostAttribute(string route) : base(route)
        {
        }
    }
}