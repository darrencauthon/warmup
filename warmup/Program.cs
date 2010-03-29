using StructureMap;
using StructureMap.Configuration.DSL;
using warmup.Bus;

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
            bus.Add<ApplicationRanMessage>(typeof(IWarmupRequestFromCommandLineHandler));
            bus.Add<WarmupTemplateRequest>(typeof(IWarmupTemplateRequestExecuter));

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