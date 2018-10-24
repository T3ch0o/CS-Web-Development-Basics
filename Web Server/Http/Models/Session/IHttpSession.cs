namespace Http.Models.Session
{
    public interface IHttpSession
    {
        string Id { get; }

        void AddParameter<T>(string name, T parameter);

        void ClearParameters();

        bool ContainsParameter(string name);

        object GetParameter(string name);

        T GetParameter<T>(string name);

        bool TryGetParameter(string name, out object parameter);

        bool TryGetParameter<T>(string name, out T parameter);
    }
}