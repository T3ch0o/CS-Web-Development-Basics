namespace Http.Models.Responses
{
    using Http.Models.Cookies;
    using Http.Models.Headers;

    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; }

        IHttpHeaderCollection Headers { get; }

        IHttpCookieCollection Cookies { get; }

        byte[] BodyBytes { get; }

        byte[] FormResponseBytes();
    }
}