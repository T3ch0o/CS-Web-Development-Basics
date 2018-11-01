namespace Http.Models.Cookies
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class HttpCookieCollection : IHttpCookieCollection
    {
        private readonly Dictionary<string, HttpCookie> _cookies = new Dictionary<string, HttpCookie>();

        public int Count => _cookies.Count;

        public bool IsReadOnly => false;

        public bool HasCookies => Count > 0;

        public HttpCookie this[string key] => _cookies[key];

        public bool TryGetValue(string key, out HttpCookie value)
        {
            bool containsCookie = _cookies.TryGetValue(key, out HttpCookie cookie);

            value = cookie;
            return containsCookie;
        }

        public IEnumerator<HttpCookie> GetEnumerator()
        {
            return _cookies.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(HttpCookie item)
        {
            _cookies.Add(item.Name, item);
        }

        public void Clear()
        {
            _cookies.Clear();
        }

        public bool Contains(string name)
        {
            return _cookies.ContainsKey(name);
        }

        public bool Contains(HttpCookie item)
        {
            return _cookies.ContainsKey(item.Name);
        }

        public void CopyTo(HttpCookie[] array, int arrayIndex)
        {
            HttpCookie[] cookies = _cookies.Values.ToArray();

            if (array.Length - arrayIndex < cookies.Length)
            {
                throw new ArgumentException("Array has insufficient capacity to store cookies", nameof(array));
            }

            foreach (HttpCookie cookie in cookies)
            {
                array[arrayIndex++] = cookie;
            }
        }

        public bool Remove(HttpCookie item)
        {
            return _cookies.Remove(item.Name);
        }
    }
}