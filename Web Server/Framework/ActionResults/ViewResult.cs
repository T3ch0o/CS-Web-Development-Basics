namespace Framework.ActionResults
{
    public class ViewResult : IViewable
    {
        public ViewResult(IRenderable view)
        {
            View = view;
        }

        public string Invoke()
        {
            return View.Render();
        }

        public IRenderable View { get; }
    }
}