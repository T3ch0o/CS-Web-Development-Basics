namespace Torshia
{
    using System.Threading.Tasks;

    using Framework;

    internal static class Program
    {
        private static async Task Main()
        {
            await WebHost.Start(new Startup());
        }
    }
}