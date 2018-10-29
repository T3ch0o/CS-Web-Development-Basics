namespace Http.Extensions
{
    using Http.Models;

    internal static class HttpStatusCodeExtensions
    {
        internal static string ToHttpFormat(this HttpStatusCode httpStatusCode)
        {
            return string.Concat((int)httpStatusCode, " ", httpStatusCode);
        }
    }
}