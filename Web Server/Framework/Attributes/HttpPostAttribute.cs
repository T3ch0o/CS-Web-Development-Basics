namespace Framework.Attributes
{
    using Http.Models;

    public class HttpPostAttribute : HttpMethodAttribute
    {
        private protected override HttpRequestMethod HttpRequestMethod => HttpRequestMethod.POST;
    }
}