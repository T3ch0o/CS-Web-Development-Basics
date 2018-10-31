namespace Http.Models.Cookies
{
    using System.Collections.Generic;

    public interface IHttpCookieCollection : ICollection<HttpCookie>
    {
        bool HasCookies { get; }

        HttpCookie this[string key] { get; }

        bool TryGetValue(string key, out HttpCookie value);
    }
}