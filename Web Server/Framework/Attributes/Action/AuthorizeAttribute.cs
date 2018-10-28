namespace Framework.Attributes.Action
{
    using System;
    using System.Linq;

    using Framework.Security;

    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute
    {
        private readonly string _role;

        public AuthorizeAttribute()
        {
        }

        public AuthorizeAttribute(string role)
        {
            _role = role;
        }

        public bool IsAuthorized(IIdentity user)
        {
            if (_role == null)
            {
                return user != null;
            }

            return user.Roles.Contains(_role);
        }
    }
}