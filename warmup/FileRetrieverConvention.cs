using System;
using System.Linq;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using warmup.TemplateFileRetrievers;

namespace warmup
{
    public class FileRetrieverConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract || !type.IsClass || !type.GetInterfaces().Contains(typeof (IFileRetriever)))
                return;
            registry.AddType(typeof (IFileRetriever), type);
        }
    }
}