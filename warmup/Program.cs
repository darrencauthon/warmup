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

            bus.Send(new ApplicationRanMessage{CommandLineArguments = args});
        }

        private static ApplicationBus CreateTheApplicationBus()
        {
            var container = CreateTheContainer();
            return new ApplicationBus(new MessageHandlerFactory(container)){
                                                                               typeof (WarmupRequestFromCommandLineHandler)
                                                                           };
        }

        private static IContainer CreateTheContainer()
        {
            var registry = new Registry();

            registry.Scan(x =>
                              {
                                  x.TheCallingAssembly();
                                  x.RegisterConcreteTypesAgainstTheFirstInterface();
                              });

            var test = new Container(registry);

            return test;
        }
    }
}