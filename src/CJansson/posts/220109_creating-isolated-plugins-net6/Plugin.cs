using PluginBase;

namespace Plugin1
{
    public class Plugin1 : IPluginBase
    {
        public void PrintData()
        {
            System.Console.WriteLine($"hello from Plugin (Newtonsoft version: {typeof(Newtonsoft.Json.JsonSerializer).Assembly.GetName().Version})");
        }
    }
}