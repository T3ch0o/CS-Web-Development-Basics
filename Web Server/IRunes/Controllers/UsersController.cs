namespace Demo.Controllers
{
    using System.Linq;

    using Demo.Attributes;
    using Demo.Database;
    using Demo.Models;
    using Demo.Services;
    using Demo.Services.Interfaces;

    using Http.Models.Cookies;
    using Http.Models.Responses;

    using WebServer.Results;

    public class UsersController : ControllerBase
    {
        private readonly IHashService _hashService = new HashService();

        private readonly IUserCookieService _userCookieService = new UserCookieService();

        private readonly DemoDbContext _dbContext = new DemoDbContext();

        [HttpGet("/users/login")]
        public IHttpResponse GetLogin()
        {
            return View("Views/Users/Login");
        }

        [HttpPost("/users/login")]
        public IHttpResponse PostLogin()
        {
            string username = (string)Request.FormData["username"];
            string password = (string)Request.FormData["password"];

            User matchingUser = _dbContext.Users.SingleOrDefault(user => user.Username == username);

            if (matchingUser == null || matchingUser.Password != _hashService.Hash(password))
            {
                return Redirect("/users/login");
            }

            RedirectResult redirect = Redirect("/");

            SignInUser(username, redirect);

            return redirect;
        }

        [HttpGet("/users/register")]
        public IHttpResponse GetRegister()
        {
            return View("Views/Users/Register");
        }

        [HttpPost("/users/register")]
        public IHttpResponse PostRegister()
        {
            string username = (string)Request.FormData["username"];
            string password = (string)Request.FormData["password"];
            string confirmPassword = (string)Request.FormData["confirmPassword"];

            if (string.IsNullOrWhiteSpace(username) ||
                username.Length < 4 ||
                _dbContext.Users.Any(user => user.Username == username) ||
                string.IsNullOrWhiteSpace(password) ||
                password.Length < 6 ||
                password != confirmPassword)
            {
                return Redirect("/users/register");
            }

            string hashedPassword = _hashService.Hash(password);

            User newUser = new User
            {
                    Username = username,
                    Password = hashedPassword,
                    Email = username
            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            IHttpResponse response = Redirect("/");

            SignInUser(username, response);

            return response;
        }

        [HttpGet("/logout")]
        public IHttpResponse Logout()
        {
            if (!IsAuthenticated)
            {
                return Redirect("/");
            }

            HttpCookie cookie = Request.Cookies.GetCookie("auth");
            cookie.Delete();

            Request.Session.ClearParameters();

            IHttpResponse response = Redirect("/");
            response.Cookies.Add(cookie);

            return response;
        }

        private void SignInUser(string username, IHttpResponse response)
        {
            Request.Session.AddParameter("username", username);
            ViewPropertyBag.Add("username", username);
            response.Cookies.Add(new HttpCookie("auth", _userCookieService.EncryptUsernameCookie(username)));
        }
    }
}