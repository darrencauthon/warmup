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
            if (TheTypeIsAConcreteClass(type) && TheTypeIsAFileRetriever(type))
                AddTheTypeToTheRegistry(type, registry);
        }

        private static void AddTheTypeToTheRegistry(Type type, IRegistry registry)
        {
            registry.AddType(typeof (IFileRetriever), type);
        }

        private static bool TheTypeIsAFileRetriever(Type type)
        {
            return type.GetInterfaces().Contains(typeof (IFileRetriever));
        }

        private static bool TheTypeIsAConcreteClass(Type type)
        {
            return !type.IsAbstract && !type.IsInterface;
        }
    }
}