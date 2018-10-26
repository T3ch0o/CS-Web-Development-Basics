namespace Framework.Attributes
{
    using Http.Models;

    public class HttpPutAttribute : HttpMethodAttribute
    {
        private protected override HttpRequestMethod HttpRequestMethod => HttpRequestMethod.PUT;
    }
}