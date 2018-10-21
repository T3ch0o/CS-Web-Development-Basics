namespace Http.Extensions
{
    using Http.Models;

    public static class HttpStatusCodeExtensions
    {
        public static string ToHttpFormat(this HttpStatusCode httpStatusCode)
        {
            return string.Concat((int)httpStatusCode, " ", httpStatusCode);
        }
    }
}