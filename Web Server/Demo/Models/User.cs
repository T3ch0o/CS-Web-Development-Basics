namespace Demo.Models
{
    internal class User : IdObject
    {
        public string Username { get; }

        public string Password { get; }

        public string Email { get; }
    }
}