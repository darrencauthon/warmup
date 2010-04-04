using System;
using System.Collections.Generic;
using System.Linq;
using AppBus;
using StructureMap;
using StructureMap.Configuration.DSL;
using warmup.Messages;
using warmup.settings;

namespace warmup
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var bus = CreateTheApplicationBus();

            var message = new CommandLineMessage{CommandLineArguments = args};

            bus.Send(message);
        }

        private static IApplicationBus CreateTheApplicationBus()
        {
            var container = CreateTheContainer();
            var bus = container.GetInstance<IApplicationBus>();

            GetAllTypesThatImplement(typeof (IMessageHandler)).ForEach(bus.Add);

            return bus;
        }

        private static List<Type> GetAllTypesThatImplement(Type implementingType)
        {
            var list = new List<Type>();
            AppDomain.CurrentDomain.GetAssemblies().ToList()
                .ForEach(assembly => assembly.GetTypes()
                                         .Where(type => type.IsAbstract == false)
                                         .Where(type => type.IsInterface == false)
                                         .Where(type => type.GetInterfaces().Contains(implementingType))
                                         .ToList().ForEach(list.Add));
            return list;
        }

        private static IContainer CreateTheContainer()
        {
            var registry = new Registry();

            RegisterAllInterfaceToClassNameMatchesInCurrentAssembly(registry);

            LoadApplicationBusImplementations(registry);

            SetSystemToUseTheConfigurationFile(registry);

            return new Container(registry);
        }

        private static void SetSystemToUseTheConfigurationFile(Registry registry)
        {
            registry.For<IWarmupConfigurationProvider>()
                .Use<ConfigurationFileWarmupConfigurationProvider>();
        }

        private static void LoadApplicationBusImplementations(Registry registry)
        {
            registry.For<IApplicationBus>()
                .Singleton().Use<ApplicationBus>();

            registry.For<IMessageHandlerFactory>()
                .Use<StructureMapMessageHandlerFactory>();
        }

        private static void RegisterAllInterfaceToClassNameMatchesInCurrentAssembly(IRegistry registry)
        {
            registry.Scan(x =>
                              {
                                  x.TheCallingAssembly();
                                  x.WithDefaultConventions();
                              });
            registry.Scan(x =>
                              {
                                  x.AssembliesFromApplicationBaseDirectory();
                                  x.With(new FileRetrieverConvention());
                              });
        }
    }
}