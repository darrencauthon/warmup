using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppBus;
using StructureMap;

namespace warmup.Setup
{
    public class ApplicationBusBuilder
    {
        private readonly ContainerBuilder containerBuilder;

        public ApplicationBusBuilder()
        {
            containerBuilder = new ContainerBuilder();
        }

        public IApplicationBus CreateTheApplicationBus()
        {
            var container = CreateTheContainer();

            var bus = container.GetInstance<IApplicationBus>();

            GetAllTypesThatImplement(typeof (IMessageHandler)).ForEach(bus.Add);

            return bus;
        }

        private IContainer CreateTheContainer()
        {
            return containerBuilder.CreateTheContainer();
        }

        private static List<Type> GetAllTypesThatImplement(Type type)
        {
            var listOfTypes = new List<Type>();

            GetAllAssemblies()
                .ForEach(assembly => GetAllTypesThatImplementTheType(assembly, type)
                                         .ForEach(listOfTypes.Add));

            return listOfTypes;
        }

        private static List<Assembly> GetAllAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().ToList();
        }

        private static List<Type> GetAllTypesThatImplementTheType(Assembly assembly, Type implementingType)
        {
            return assembly.GetTypes()
                .Where(type => type.IsAbstract == false)
                .Where(type => type.IsInterface == false)
                .Where(type => type.GetInterfaces().Contains(implementingType))
                .ToList();
        }
    }
}