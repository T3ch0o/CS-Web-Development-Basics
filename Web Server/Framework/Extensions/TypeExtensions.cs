namespace Framework.Extensions
{
    using System;

    internal static class TypeExtensions
    {
        internal static bool IsPrimitiveOrString(this Type type)
        {
            return type.IsPrimitive || type == typeof(string);
        }
    }
}