using AppBus;
using StructureMap;
using StructureMap.Configuration.DSL;
using warmup.settings;

namespace warmup
{
    public class ContainerBuilder
    {
        public IContainer CreateTheContainer()
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