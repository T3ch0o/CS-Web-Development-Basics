namespace Framework
{
    public class MvcContext
    {
        private MvcContext()
        {
        }

        public static MvcContext Instance { get; } = new MvcContext();

        public string AssemblyName { get; set; }

        public string ControllersNamespace { get; set; }

        public string ControllersSuffix { get; set; }

        public string ViewsFolder { get; set; }

        public string ModelsFolder { get; set; }
    }
}