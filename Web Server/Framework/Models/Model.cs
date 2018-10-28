namespace Framework.Models
{
    public class Model
    {
        private bool? _isValid;
        public bool? IsValid
        {
            get => _isValid;

            set
            {
                if (_isValid == null)
                {
                    _isValid = value;
                }
            }
        }
    }
}