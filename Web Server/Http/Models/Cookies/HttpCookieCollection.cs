namespace Http.Models.Cookies
{
    using System.Collections.Generic;

    public class HttpCookieCollection : IHttpCookieCollection
    {
        private readonly Dictionary<string, HttpCookie> _cookies = new Dictionary<string, HttpCookie>();

        public void Add(HttpCookie cookie)
        {
            _cookies[cookie.Name] = cookie;
        }

        public bool Contains(string name)
        {
            return _cookies.ContainsKey(name);
        }

        public HttpCookie GetCookie(string name)
        {
            return _cookies[name];
        }

        public bool HasCookies()
        {
            return _cookies.Count > 0;
        }

        public override string ToString()
        {
            return string.Join("; ", _cookies.Values);
        }
    }
}