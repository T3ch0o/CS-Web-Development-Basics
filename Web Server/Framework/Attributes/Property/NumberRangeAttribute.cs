namespace Framework.Attributes.Property
{
    public class NumberRangeAttribute : ValidationAttribute
    {
        private readonly double _min;

        private readonly double _max;

        public NumberRangeAttribute(double min = double.MinValue, double max = double.MaxValue)
        {
            _min = min;
            _max = max;
        }

        public override bool IsValid(object value)
        {
            double number = (double)value;

            return _min <= number && number <= _max;
        }
    }
}