namespace Http.Models.Session
{
    using System.Collections.Generic;

    public class HttpSession : IHttpSession
    {
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();

        public HttpSession(string id)
        {
            Id = id;
        }

        public string Id { get; }

        public void AddParameter<T>(string name, T parameter)
        {
            _parameters.Add(name, parameter);
        }

        public void ClearParameters()
        {
            _parameters.Clear();
        }

        public bool ContainsParameter(string name)
        {
            return _parameters.ContainsKey(name);
        }

        public object GetParameter(string name)
        {
            return _parameters[name];
        }

        public T GetParameter<T>(string name)
        {
            return (T)_parameters[name];
        }

        public bool TryGetParameter(string name, out object parameter)
        {
            bool exists = _parameters.TryGetValue(name, out object value);

            parameter = value;
            return exists;
        }

        public bool TryGetParameter<T>(string name, out T parameter)
        {
            bool exists = _parameters.TryGetValue(name, out object value);

            parameter = (T)value;
            return exists;
        }
    }
}