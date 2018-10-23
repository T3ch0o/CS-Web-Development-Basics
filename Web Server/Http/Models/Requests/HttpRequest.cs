namespace Http.Models.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Http.Configuration;
    using Http.Models.Headers;

    public class HttpRequest : IHttpRequest
    {
        private HttpRequest(string path, string url, Dictionary<string, object> formData, Dictionary<string, object> queryData, HttpRequestMethod method, IHttpHeaderCollection headers)
        {
            Path = path;
            Url = url;
            FormData = formData;
            QueryData = queryData;
            Method = method;
            Headers = headers;
        }

        public static HttpRequest Parse(string request)
        {
            string[] requestLines = request.Split(new[] { "\r\n" }, StringSplitOptions.None);

            string[] firstLineComponents = requestLines[0]
                    .Split(' ');

            if (firstLineComponents.Length != 3)
            {
                throw new ArgumentException("HTTP request line is invalid", nameof(request));
            }

            if (!Enum.TryParse(firstLineComponents[0], out HttpRequestMethod method))
            {
                throw new ArgumentException("HTTP request method is invalid", nameof(request));
            }

            string url = firstLineComponents[1];

            Uri uri = new Uri(url);

            string path = uri.AbsolutePath;

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

            {
                string query = uri.Query;

                if (query != string.Empty)
                {
                    queryData = new Dictionary<string, object>(query.Split('&').Select(ParseQueryString));
                }
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

                if (line == "\r\n")
                {
                    break;
                }

                string[] keyValuePair = line.Split(new[] { ": " }, StringSplitOptions.None);

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

                if (requestLines.Length > formDataLineIndex)
                {
                    string formDataLine = requestLines[formDataLineIndex];

                    formData = new Dictionary<string, object>(formDataLine.Split('&').Select(ParseQueryString));
                }
            }

            return new HttpRequest(path, url, formData, queryData, method, headers);
        }

        public string Path { get; }

        public string Url { get; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public HttpRequestMethod Method { get; }

        public IHttpHeaderCollection Headers { get; }
    }
}