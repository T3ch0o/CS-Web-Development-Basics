namespace Demo.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;

    using Http.Models;
    using Http.Models.Requests;

    using WebServer.Results;

    public class ControllerBase
    {
        internal IHttpRequest Request { private protected get; set; }

        private protected bool IsAuthenticated => Request.Session.ContainsParameter("username");

        private protected Dictionary<string, object> ViewPropertyBag { get; } = new Dictionary<string, object>();

        private protected RedirectResult Redirect(string route)
        {
            return new RedirectResult(route);
        }

        private protected HtmlResult View(string relativePath, string extension = ".html")
        {
            string html = File.ReadAllText(string.Concat(relativePath, extension));

            html = Regex.Replace(html, "{{(.+)}}", match => ViewPropertyBag[match.Groups[1].Value].ToString());

            return new HtmlResult(html, HttpStatusCode.OK);
        }
    }
}