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

            for (int characterIndex = 1; characterIndex < characters.Length; ++characterIndex)
            {
                ref char character = ref characters[characterIndex];
                character = char.ToLower(character);
            }

            return new string(characters);
        }
    }
}