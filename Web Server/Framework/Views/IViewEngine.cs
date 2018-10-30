namespace Framework.Views
{
    using System.Collections.Generic;

    public interface IViewEngine
    {
        string RenderView(string controller, string action, IDictionary<string, object> propertyBag);

        string RenderError(string error, string role);
    }
}