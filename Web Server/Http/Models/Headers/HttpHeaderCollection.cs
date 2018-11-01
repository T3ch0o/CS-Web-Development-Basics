namespace Http.Models.Headers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> _headers = new Dictionary<string, HttpHeader>();

        public int Count => _headers.Count;

        public bool IsReadOnly => false;

        public bool HasHeaders => Count > 0;

        public HttpHeader this[string key] => _headers[key];

        public void Add(string name, string value)
        {
            Add(new HttpHeader(name, value));
        }

        public bool Contains(string header)
        {
            return _headers.ContainsKey(header);
        }

        public bool TryGetHeader(string key, out HttpHeader value)
        {
            bool containsHeader = _headers.TryGetValue(key, out HttpHeader header);

            value = header;
            return containsHeader;
        }

        public IEnumerator<HttpHeader> GetEnumerator()
        {
            return _headers.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(HttpHeader item)
        {
            _headers.Add(item.Name, item);
        }

        public void Clear()
        {
            _headers.Clear();
        }

        public bool Contains(HttpHeader item)
        {
            return _headers.ContainsKey(item.Name);
        }

        public void CopyTo(HttpHeader[] array, int arrayIndex)
        {
            HttpHeader[] headers = _headers.Values.ToArray();

            if (array.Length - arrayIndex < headers.Length)
            {
                throw new ArgumentException("Array has insufficient capacity to store headers", nameof(array));
            }

            foreach (HttpHeader header in headers)
            {
                array[arrayIndex++] = header;
            }
        }

        public bool Remove(HttpHeader item)
        {
            return _headers.Remove(item.Name);
        }
    }
}