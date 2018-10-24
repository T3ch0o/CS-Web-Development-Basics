namespace Http.Models.Cookies
{
    public interface IHttpCookieCollection
    {
        void Add(HttpCookie cookie);

        bool Contains(string name);

        HttpCookie GetCookie(string name);

        bool HasCookies();
    }
}