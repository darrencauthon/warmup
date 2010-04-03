using AppBus;
using StructureMap;
using StructureMap.Configuration.DSL;
using warmup.Behaviors;
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

            bus.Add(typeof (ProcessCommandLineWarmupRequest));
            bus.Add(typeof (ExecuteTheWarmupRequest));
            bus.Add(typeof (DetermineThePathToPutTheFiles));
            bus.Add(typeof (RetrieveFilesFromTheGitRepository));

            return bus;
        }

        private static IContainer CreateTheContainer()
        {
            var registry = new Registry();

            registry.Scan(x =>
                              {
                                  x.TheCallingAssembly();
                                  x.WithDefaultConventions();
                              });
            registry.Scan(x =>
                              {
                                  x.AssemblyContainingType(typeof (IApplicationBus));
                                  x.WithDefaultConventions();
                              });

            registry.For<IApplicationBus>()
                .Singleton();

            registry.For<IMessageHandlerFactory>()
                .Use<StructureMapMessageHandlerFactory>();

            registry.For<IWarmupConfigurationProvider>()
                .Use<ConfigurationFileWarmupConfigurationProvider>();

            var test = new Container(registry);

            return test;
        }
    }
}