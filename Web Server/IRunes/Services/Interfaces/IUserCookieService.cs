namespace Demo.Services.Interfaces
{
    internal interface IUserCookieService
    {
        string EncryptUsernameCookie(string username);

        string DecryptUsernameCookie(string cookieContent);
    }
}