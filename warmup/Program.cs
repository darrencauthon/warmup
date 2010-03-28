using StructureMap;
using StructureMap.Configuration.DSL;
using warmup.Bus;

namespace warmup
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var container = CreateTheContainer();
            var bus = new ApplicationBus(new MessageHandlerFactory(container)){
                                                                                  typeof (AttemptToExecuteWarmupMessageHandler)
                                                                              };

            bus.Send(new ApplicationRanMessage{CommandLineArguments = args});
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