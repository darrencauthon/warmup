using AppBus;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace warmup
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var bus = CreateTheApplicationBus();

            var message = new ApplicationRanMessage { CommandLineArguments = args };

            bus.Send(message);
        }

        private static IApplicationBus CreateTheApplicationBus()
        {
            var container = CreateTheContainer();
            var bus = container.GetInstance<IApplicationBus>();
            bus.Add(typeof(IWarmupRequestFromCommandLineHandler));
            bus.Add(typeof(IWarmupTemplateRequestExecuter));
            return bus;
        }

        private static IContainer CreateTheContainer()
        {
            var registry = new Registry();

            registry.Scan(x =>
                              {
                                  x.TheCallingAssembly();
                                  x.SingleImplementationsOfInterface();
                              });

            registry.For<IApplicationBus>()
                .Singleton();

            var test = new Container(registry);

            return test;
        }
    }
}