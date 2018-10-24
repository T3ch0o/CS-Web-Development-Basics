namespace Http.Models.Session
{
    using System.Collections.Concurrent;

    public static class HttpSessionStorage
    {
        public const string SessionCookieKey = "SIS_ID";

        private static readonly ConcurrentDictionary<string, IHttpSession> Sessions = new ConcurrentDictionary<string, IHttpSession>();

        public static IHttpSession GetSession(string id)
        {
            return Sessions.GetOrAdd(id, key => new HttpSession(key));
        }
    }
}