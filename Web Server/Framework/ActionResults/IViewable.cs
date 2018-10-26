namespace Framework.ActionResults
{
    public interface IViewable : IActionResult
    {
        IRenderable View { get; }
    }
}