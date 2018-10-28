namespace Framework.Attributes.Property
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ValidationAttribute : Attribute
    {
        public abstract bool IsValid(object value);
    }
}