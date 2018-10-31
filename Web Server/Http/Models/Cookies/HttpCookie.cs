namespace Http.Models.Cookies
{
    using System;
    using System.Text;

    public class HttpCookie
    {
        public HttpCookie(string name, string value, int expiryOffsetDays = 3, string path = "/", bool isHttpOnly = false, bool isSecure = false)
            : this(name, value, DateTime.UtcNow.AddDays(expiryOffsetDays), path, isHttpOnly, isSecure)
        {
        }

        public HttpCookie(string name, string value, DateTime expiresAt, string path = "/", bool isHttpOnly = false, bool isSecure = false)
        {
            Name = name;
            Value = value;
            ExpiresAt = expiresAt;
            Path = path;
            IsHttpOnly = isHttpOnly;
            IsSecure = isSecure;
        }

        public string Name { get; }

        public string Value { get; set; }

        public DateTime ExpiresAt { get; set; }

        public string Path { get; set; }

        public bool IsHttpOnly { get; set; }

        public bool IsSecure { get; set; }

        public void Delete()
        {
            ExpiresAt = DateTime.UtcNow.AddDays(-1);
            Value = string.Empty;
        }

        public override string ToString()
        {
            string startingValue = $"{Name}={Value};Expires={ExpiresAt:R};Path={Path};";

            StringBuilder stringBuilder = new StringBuilder(startingValue);

            if (IsHttpOnly)
            {
                stringBuilder.Append("HttpOnly;");
            }

            if (IsSecure)
            {
                stringBuilder.Append("Secure;");
            }

            // Remove trailing semi-colon
            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            return stringBuilder.ToString();
        }
    }
}