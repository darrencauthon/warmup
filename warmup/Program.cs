using AppBus;
using warmup.Messages;

namespace warmup
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            LoadAllAssemblies();

            PassTheCommandLineArgumentsToTheApplicationBus(args);
        }

        private static void PassTheCommandLineArgumentsToTheApplicationBus(string[] args)
        {
            var bus = CreateTheApplicationBus();

            var message = new CommandLineMessage{CommandLineArguments = args};

            bus.Send(message);
        }

        private static IApplicationBus CreateTheApplicationBus()
        {
            var applicationBusBuilder = new ApplicationBusBuilder();
            return applicationBusBuilder.CreateTheApplicationBus();
        }

        private static void LoadAllAssemblies()
        {
            var assemblyLoader = (new AssemblyLoader("warmup.dll"));
            assemblyLoader.LoadAllAssembliesInTheCurrentFolder();
        }
    }
}