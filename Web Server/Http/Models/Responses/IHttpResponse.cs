namespace Http.Models.Responses
{
    using Http.Models.Headers;

    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; }

        IHttpHeaderCollection Headers { get; }

        byte[] BodyBytes { get; }

        byte[] FormResponseBytes();
    }
}