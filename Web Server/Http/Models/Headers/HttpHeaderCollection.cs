namespace Http.Models.Headers
{
    using System.Collections.Generic;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> _headers = new Dictionary<string, HttpHeader>();

        public void Add(string name, string value)
        {
            Add(new HttpHeader(name, value));
        }

        public void Add(HttpHeader header)
        {
            _headers.Add(header.Name, header);
        }

        public bool ContainsHeader(string name)
        {
            return _headers.ContainsKey(name);
        }

        public bool TryGetHeader(string name, out HttpHeader? header)
        {
            if (_headers.TryGetValue(name, out HttpHeader value))
            {
                header = value;
                return true;
            }

            header = null;
            return false;
        }

        public override string ToString()
        {
            return string.Join("\r\n", _headers.Values);
        }
    }
}