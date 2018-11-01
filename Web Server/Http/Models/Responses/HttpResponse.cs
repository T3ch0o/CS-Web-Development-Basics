namespace Http.Models.Responses
{
    using System;
    using System.Text;

    using Http.Configuration;
    using Http.Extensions;
    using Http.Models.Cookies;
    using Http.Models.Headers;

    public class HttpResponse : IHttpResponse
    {
        public HttpResponse(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }

        public IHttpHeaderCollection Headers { get; } = new HttpHeaderCollection();

        public IHttpCookieCollection Cookies { get; } = new HttpCookieCollection();

        public byte[] BodyBytes { get; protected set; }

        public byte[] FormResponseBytes()
        {
            byte[] headBytes = Encoding.ASCII.GetBytes(ToString());

            if (BodyBytes == null)
            {
                return headBytes;
            }

            byte[] responseBytes = new byte[headBytes.Length + BodyBytes.Length];

            Buffer.BlockCopy(headBytes, 0, responseBytes, 0, headBytes.Length);
            Buffer.BlockCopy(BodyBytes, 0, responseBytes, headBytes.Length, BodyBytes.Length);

            return responseBytes;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("{0} {1}\r\n", Constants.HttpProtocolVersion, StatusCode.ToHttpFormat(), Headers);

            foreach (HttpHeader header in Headers)
            {
                stringBuilder.AppendFormat("{0}: {1}\r\n", header.Name, header.Value);
            }

            foreach (HttpCookie cookie in Cookies)
            {
                stringBuilder.AppendFormat("Set-Cookie: {0}\r\n", cookie);
            }

            stringBuilder.Append("\r\n");

            return stringBuilder.ToString();
        }
    }
}