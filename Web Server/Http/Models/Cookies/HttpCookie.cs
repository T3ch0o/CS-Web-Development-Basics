namespace Http.Models.Cookies
{
    using System;

    public class HttpCookie
    {
        private const int DefaultExpiryOffsetDays = 3;

        public HttpCookie(string name, string value, int expireOffsetDays = DefaultExpiryOffsetDays)
        {
            Name = name;
            Value = value;
            ExpiresAt = DateTime.UtcNow.AddDays(expireOffsetDays);
            IsNew = true;
        }

        public HttpCookie(string name, string value, DateTime expiresAt, bool isNew)
        {
            Name = name;
            Value = value;
            ExpiresAt = expiresAt;
            IsNew = isNew;
        }

        public string Name { get; }

        public string Value { get; }

        public DateTime ExpiresAt { get; }

        public bool IsNew { get; }

        public override string ToString()
        {
            return $"{Name}={Value}; Expires={ExpiresAt.ToLongTimeString()}";
        }
    }
}