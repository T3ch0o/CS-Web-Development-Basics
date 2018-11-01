namespace Http.Models.Headers
{
    using System.Collections.Generic;

    public interface IHttpHeaderCollection : ICollection<HttpHeader>
    {
        bool HasHeaders { get; }

        HttpHeader this[string key] { get; }

        void Add(string name, string value);

        bool Contains(string header);

        bool TryGetHeader(string key, out HttpHeader value);
    }
}