using System.Reflection;
using System.Runtime.Loader;

namespace ConsoleApp
{
    public class PluginContext : AssemblyLoadContext
    {
        private Dictionary<string, Assembly> sharedAssemblies;

        public Guid Id { get; }

        public PluginContext(params Type[] sharedTypes) : base(true)
        {
            Id = Guid.NewGuid();

            sharedAssemblies = new();

            foreach (Type sharedType in sharedTypes)
                sharedAssemblies[Path.GetFileName(sharedType.Assembly.Location)] = sharedType.Assembly;
        }

        public void InitializeFromFolder(string path)
        {
            foreach (string dll in Directory.EnumerateFiles(path, "*.dll"))
            {
                if (sharedAssemblies.ContainsKey(Path.GetFileName(dll)))
                    continue;

                string absoluteDllPath = Path.GetFullPath(dll);
                LoadFromAssemblyPath(absoluteDllPath);
            }
        }

        public void InitializeFromBytes(byte[] assembly)
        {
            using(var ms = new MemoryStream(assembly))
                LoadFromStream(ms);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            string filename = $"{assemblyName.Name}.dll";
            if (sharedAssemblies.ContainsKey(filename))
                return sharedAssemblies[filename];

            return Assembly.Load(assemblyName);
        }
    }
}
