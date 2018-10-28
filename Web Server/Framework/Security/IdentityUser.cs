namespace Framework.Security
{
    using System;
    using System.Collections.Generic;

    public sealed class IdentityUser : IdentityUser<string>
    {
        public IdentityUser()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class IdentityUser<TKey> : IIdentity
            where TKey : IEquatable<TKey>
    {
        public virtual TKey Id { get; protected set; }

        public virtual string Username { get; set; }

        public virtual string Password { get; set; }

        public virtual string Email { get; set; }

        public virtual bool IsValid { get; set; }

        public virtual IEnumerable<string> Roles { get; set; }
    }
}