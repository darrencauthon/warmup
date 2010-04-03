using AppBus;
using StructureMap;
using StructureMap.Configuration.DSL;
using warmup.Behaviors;
using warmup.Messages;
using warmup.settings;
using warmup.TemplateFileRetrievers;
using System;

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

            bus.Add(typeof (ProcessCommandLineWarmupRequest));
            bus.Add(typeof (ExecuteTheWarmupRequest));
            bus.Add(typeof (DetermineThePathToPutTheFiles));
            bus.Add(typeof (RetrieveFilesFromTheGitRepository));

            return bus;
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

            registry.For<IFileRetriever>()
                .Use<GitTemplateFilesRetriever>()
                .Named(Guid.NewGuid().ToString());
            registry.For<IFileRetriever>()
                .Use<SvnTemplateFilesRetriever>()
                .Named(Guid.NewGuid().ToString());
            
        }

        private static void LoadApplicationBusImplementations(Registry registry)
        {
            registry.For<IApplicationBus>()
                .Singleton().Use<ApplicationBus>();

            registry.For<IMessageHandlerFactory>()
                .Use<StructureMapMessageHandlerFactory>();
        }

        private static void RegisterAllInterfaceToClassNameMatchesInCurrentAssembly(Registry registry)
        {
            registry.Scan(x =>
                              {
                                  x.TheCallingAssembly();
                                  x.WithDefaultConventions();
                              });
        }
    }
}