namespace Http.Extensions
{
    internal static class StringExtensions
    {
        internal static string Capitalize(this string value)
        {
            if (value.Length == 0)
            {
                return value;
            }

            char[] characters = value.ToCharArray();

            {
                ref char firstCharacter = ref characters[0];
                firstCharacter = char.ToUpper(firstCharacter);
            }

            return new string(characters);
        }
    }
}