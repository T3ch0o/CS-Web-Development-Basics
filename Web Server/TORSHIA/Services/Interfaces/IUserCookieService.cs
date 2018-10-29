namespace Torshia.Services.Interfaces
{
    internal interface IUserCookieService
    {
        string EncryptUsernameCookie(string username);

        string DecryptUsernameCookie(string cookieContent);
    }
}