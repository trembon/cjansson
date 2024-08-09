var manager = new PluginManager<IPluginBase>();
manager.Load("/path/to/plugin");

foreach (var plugin in manager.GetImplementations())
    plugin.PrintData();