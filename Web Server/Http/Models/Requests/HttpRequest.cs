namespace Http.Models.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Http.Configuration;
    using Http.Models.Cookies;
    using Http.Models.Headers;

    public class HttpRequest : IHttpRequest
    {
        private HttpRequest(string path, string url, Dictionary<string, object> formData, Dictionary<string, object> queryData, HttpRequestMethod method, IHttpHeaderCollection headers, IHttpCookieCollection cookies)
        {
            Path = path;
            Url = url;
            FormData = formData;
            QueryData = queryData;
            Method = method;
            Headers = headers;
            Cookies = cookies;
        }

        public static HttpRequest Parse(string request)
        {
            string[] requestLines = request.Split("\r\n");

            string[] firstLineComponents = requestLines[0].Split(' ');

            if (firstLineComponents.Length != 3)
            {
                throw new ArgumentException("HTTP request line is invalid", nameof(request));
            }

            if (!Enum.TryParse(firstLineComponents[0], out HttpRequestMethod method))
            {
                throw new ArgumentException("HTTP request method is invalid", nameof(request));
            }

            string url = firstLineComponents[1];

            string[] pathComponents = url.Split('?');

            if (pathComponents.Length > 2)
            {
                throw new ArgumentException("HTTP query is invalid", nameof(request));
            }

            string path = pathComponents[0];

            Dictionary<string, object> queryData = null;

            KeyValuePair<string, object> ParseQueryString(string part)
            {
                string[] keyValuePair = part.Split('=');

                if (keyValuePair.Length != 2)
                {
                    throw new ArgumentException("Query string key/value pair does not contain two part", nameof(part));
                }

                return new KeyValuePair<string, object>(keyValuePair[0], keyValuePair[1]);
            }

            if (pathComponents.Length == 2)
            {
                string query = pathComponents[1];

                if (query.Contains('#'))
                {
                    query = query.Split('#', 1).Single();
                }

                queryData = new Dictionary<string, object>(query.Split('&').Select(ParseQueryString));
            }

            string protocol = firstLineComponents[2];

            if (protocol != Constants.HttpProtocolVersion)
            {
                throw new ArgumentException("HTTP protocol version is invalid", nameof(request));
            }

            HttpHeaderCollection headers = new HttpHeaderCollection();

            int lastLineIndex = 0;

            for (int lineIndex = 1; lineIndex < requestLines.Length; ++lineIndex)
            {
                lastLineIndex = lineIndex;

                string line = requestLines[lineIndex];

                if (line == string.Empty)
                {
                    break;
                }

                string[] keyValuePair = line.Split(": ", 2);

                if (keyValuePair.Length != 2)
                {
                    throw new ArgumentException("A header is invalid", nameof(request));
                }

                headers.Add(keyValuePair[0], keyValuePair[1]);
            }

            if (!headers.ContainsHeader("Host"))
            {
                throw new ArgumentException("Host header not found", nameof(request));
            }

            Dictionary<string, object> formData = null;

            {
                int formDataLineIndex = lastLineIndex + 1;

                if (requestLines.Length > formDataLineIndex + 1)
                {
                    string formDataLine = requestLines[formDataLineIndex];

                    formData = new Dictionary<string, object>(formDataLine.Split('&').Select(ParseQueryString));
                }
            }

            HttpCookieCollection httpCookieCollection = new HttpCookieCollection();

            if (headers.TryGetHeader("Cookie", out HttpHeader? header))
            {
                foreach (string[] parts in header.Value.Value.Split("; ").Select(cookie => cookie.Split('=')))
                {
                    httpCookieCollection.Add(new HttpCookie(parts[0], parts[1]));
                }
            }

            return new HttpRequest(path, url, formData, queryData, method, headers, httpCookieCollection);
        }

        public string Path { get; }

        public string Url { get; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public HttpRequestMethod Method { get; }

        public IHttpHeaderCollection Headers { get; }

        public IHttpCookieCollection Cookies { get; }
    }
}