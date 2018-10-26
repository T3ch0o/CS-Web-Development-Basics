namespace Framework.ActionResults
{
    public class RedirectResult : IRedirectable
    {
        public RedirectResult(string redirectUrl)
        {
            RedirectUrl = redirectUrl;
        }

        public string Invoke()
        {
            return RedirectUrl;
        }

        public string RedirectUrl { get; }
    }
}