namespace Http.Models.Headers
{
    public readonly struct HttpHeader
    {
        public HttpHeader(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public string Value { get; }

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }
    }
}