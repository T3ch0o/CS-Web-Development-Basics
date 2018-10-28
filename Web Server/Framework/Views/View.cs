namespace Framework.Views
{
    using Framework.ActionResults;

    public class View : IRenderable
    {
        private readonly string _content;

        public View(string content)
        {
            _content = content;
        }

        public string Render()
        {
            return _content;
        }
    }
}