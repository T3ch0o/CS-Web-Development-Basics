namespace Http.Models.Headers
{
    using System.Collections.Generic;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();

        public void Add(string name, string value)
        {
            _headers.Add(name, value);
        }

        public void Add(HttpHeader header)
        {
            Add(header.Name, header.Value);
        }

        public bool ContainsHeader(string name)
        {
            return _headers.ContainsKey(name);
        }

        public bool TryGetHeader(string name, out HttpHeader? header)
        {
            if (_headers.TryGetValue(name, out string value))
            {
                header = new HttpHeader(name, value);
                return true;
            }

            header = null;
            return false;
        }

        public override string ToString()
        {
            return string.Join("\r\n", _headers);
        }
    }
}