namespace Framework.Attributes.Action
{
    using System;
    using System.Linq;

    using Framework.Security;

    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute
    {
        private readonly string[] _roles;

        public AuthorizeAttribute()
        {
        }

        public AuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public bool IsAuthorized(IIdentity user)
        {
            if (_roles.Length == 0)
            {
                return user != null;
            }

            if (user != null)
            {
                return _roles.Contains(user.Role);
            }

            return false;
        }
    }
}