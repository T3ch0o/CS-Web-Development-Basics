namespace Http.Models.Responses
{
    using System;
    using System.Text;

    using Http.Configuration;
    using Http.Extensions;
    using Http.Models.Headers;

    public class HttpResponse : IHttpResponse
    {
        public HttpResponse(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }

        public IHttpHeaderCollection Headers { get; } = new HttpHeaderCollection();

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
            return $"{Constants.HttpProtocolVersion} {StatusCode.ToHttpFormat()}\r\n{Headers}\r\n\r\n";
        }
    }
}