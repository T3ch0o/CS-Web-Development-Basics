namespace Framework.Attributes
{
    using Http.Models;

    public class HttpDeleteAttribute : HttpMethodAttribute
    {
        private protected override HttpRequestMethod HttpRequestMethod => HttpRequestMethod.DELETE;
    }
}