using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace warmup
{
    public class AssemblyLoader
    {
        private readonly string nameOfDll;

        public AssemblyLoader(string nameOfDll)
        {
            this.nameOfDll = nameOfDll;
        }

        public void LoadAllAssembliesInTheCurrentFolder()
        {
            var currentAssemblies = GetLoadedAssemblies();

            var assemblyFiles = GetAssembliesInCurrentFolder(nameOfDll);

            LoadAssembliesThatHaveNotBeenPreviouslyLoaded(assemblyFiles, currentAssemblies);
        }

        private static void LoadAssembliesThatHaveNotBeenPreviouslyLoaded(IEnumerable<string> assemblyFiles, ICollection<string> currentAssemblies)
        {
            foreach (var file in assemblyFiles)
            {
                var assemblyName = Path.GetFileNameWithoutExtension(file);
                if (TheAssemblyHasNotBeenLoaded(currentAssemblies, assemblyName))
                    AttemptToLoadAssembly(file);
            }
        }

        private static bool TheAssemblyHasNotBeenLoaded(ICollection<string> currentAssemblies, string assemblyName)
        {
            return currentAssemblies.Contains(assemblyName) == false;
        }

        private static void AttemptToLoadAssembly(string file)
        {
            try
            {
                Assembly.LoadFrom(file);
            }
            catch
            {
            }
        }

        private static IList<string> GetLoadedAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            return assemblies.Select(x => x.GetName().Name).ToList();
        }

        private static IList<string> GetAssembliesInCurrentFolder(string nameOfDll)
        {
            var binDir = Path.GetFullPath(nameOfDll);
            binDir = binDir.Substring(0, binDir.Length - nameOfDll.Length);
            return Directory.GetFiles(binDir, "*.dll").ToList();
        }
    }
}