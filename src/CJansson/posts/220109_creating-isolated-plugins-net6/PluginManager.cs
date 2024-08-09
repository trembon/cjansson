using System.Collections.Concurrent;

namespace ConsoleApp
{
    public class PluginManager<TPluginType>
    {
        private Type pluginType;
        private ConcurrentDictionary<Guid, PluginContext> plugins;

        public IEnumerable<Guid> Plugins { get => plugins.Keys.ToArray(); }

        public PluginManager()
        {
            pluginType = typeof(TPluginType);
            plugins = new();
        }

        public Guid Load(string folder)
        {
            var plugin = new PluginContext(pluginType);
            if (plugins.TryAdd(plugin.Id, plugin))
            {
                plugin.InitializeFromFolder(folder);
                return plugin.Id;
            }

            return Guid.Empty;
        }

        public Guid Load(byte[] assembly)
        {
            var plugin = new PluginContext(pluginType);
            if (plugins.TryAdd(plugin.Id, plugin))
            {
                plugin.InitializeFromBytes(assembly);
                return plugin.Id;
            }

            return Guid.Empty;
        }

        public void Unload(Guid id)
        {
            if (plugins.TryRemove(id, out var plugin))
                plugin.Unload();
        }

        public IEnumerable<TPluginType> GetImplementations()
        {
            return plugins
                .Values
                .SelectMany(x => x.Assemblies)
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(TPluginType).IsAssignableFrom(t))
                .Select(t => Activator.CreateInstance(t))
                .Cast<TPluginType>()
                .ToArray();
        }
    }
}
