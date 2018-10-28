namespace Framework.Views
{
    public interface IViewReader
    {
        string ReadView(string viewPath);

        bool ViewExists(string viewPath);
    }
}