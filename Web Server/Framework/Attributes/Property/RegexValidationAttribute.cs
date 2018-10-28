namespace Framework.Attributes.Property
{
    using System.Text.RegularExpressions;

    public class RegexValidationAttribute : ValidationAttribute
    {
        private readonly Regex _regex;

        public RegexValidationAttribute(Regex regex)
        {
            _regex = regex;
        }

        public override bool IsValid(object value)
        {
            return _regex.IsMatch(value.ToString());
        }
    }
}