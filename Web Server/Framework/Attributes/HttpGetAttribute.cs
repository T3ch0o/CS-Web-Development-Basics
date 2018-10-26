namespace Framework.Attributes
{
    using Http.Models;

    public class HttpGetAttribute : HttpMethodAttribute
    {
        private protected override HttpRequestMethod HttpRequestMethod => HttpRequestMethod.GET;
    }
}