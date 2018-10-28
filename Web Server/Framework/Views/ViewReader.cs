namespace Framework.Views
{
    using System.IO;

    internal class ViewReader : IViewReader
    {
        public string ReadView(string viewPath)
        {
            return File.ReadAllText(viewPath);
        }

        public bool ViewExists(string viewPath)
        {
            return File.Exists(viewPath);
        }
    }
}