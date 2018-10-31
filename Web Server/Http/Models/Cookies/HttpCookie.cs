namespace Http.Models.Cookies
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    public class HttpCookie
    {
        // Based on RFC 6265 HTTP State Management Mechanism specification
        private static readonly Regex StringParameterRegex = new Regex(@"^[\x21\x23-\x2B\x2D-\x3A\x3C-\x5B\x5D-\x7E]*$", RegexOptions.Compiled);

        public HttpCookie(string name, string value, int expiryOffsetDays = 3, string path = "/", bool isHttpOnly = false, bool isSecure = false)
            : this(name, value, DateTime.UtcNow.AddDays(expiryOffsetDays), path, isHttpOnly, isSecure)
        {
        }

        public HttpCookie(string name, string value, DateTime expiresAt, string path = "/", bool isHttpOnly = false, bool isSecure = false)
        {
            if (!StringParameterRegex.IsMatch(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), name, "Cookie name contains disallowed characters");
            }

            if (!StringParameterRegex.IsMatch(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Cookie value contains disallowed characters");
            }

            if (!StringParameterRegex.IsMatch(path))
            {
                throw new ArgumentOutOfRangeException(nameof(path), path, "Cookie path contains disallowed characters");
            }

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
            Value = string.Empty;
            ExpiresAt = DateTime.UtcNow.AddDays(-1);
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