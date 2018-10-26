namespace Framework.ActionResults
{
    using System.IO;

    public class View : IRenderable
    {
        private readonly string _templateName;

        public View(string templateName)
        {
            _templateName = templateName;
        }

        public string Render()
        {
            return File.ReadAllText(_templateName);
        }
    }
}