namespace Http.Models.Headers
{
    public interface IHttpHeaderCollection
    {
        void Add(string name, string value);

        void Add(HttpHeader header);

        bool ContainsHeader(string name);

        bool TryGetHeader(string name, out HttpHeader? header);
    }
}