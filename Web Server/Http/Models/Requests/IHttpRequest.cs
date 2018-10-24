namespace Http.Models.Requests
{
    using System.Collections.Generic;

    using Http.Models.Cookies;
    using Http.Models.Headers;

    public interface IHttpRequest
    {
        string Path { get; }

        string Url { get; }

        Dictionary<string, object> FormData { get; }

        Dictionary<string, object> QueryData { get; }

        HttpRequestMethod Method { get; }

        IHttpHeaderCollection Headers { get; }

        IHttpCookieCollection Cookies { get; }
    }
}