namespace Framework.Utilities
{
    using System.IO;

    using Framework.Controllers;

    internal static class ControllerUtilities
    {
        internal static string GetControllerName(Controller controller)
        {
            return controller.GetType().Name.Replace(MvcContext.Instance.ControllersSuffix, string.Empty);
        }

        internal static string GetActionViewPath(string controller, string action)
        {
            return string.Concat(Path.Combine(MvcContext.Instance.ViewsFolder, controller, action), ".html");
        }
    }
}