using AppBus;
using warmup.Messages;

namespace warmup
{
    public class ProcessCommandLineWarmupRequest : IMessageHandler<CommandLineMessage>
    {
        private readonly IWarmupTemplateRequestParser warmupTemplateRequestParser;
        private readonly IApplicationBus bus;

        public ProcessCommandLineWarmupRequest(IWarmupTemplateRequestParser warmupTemplateRequestParser,
                                               IApplicationBus bus)
        {
            this.warmupTemplateRequestParser = warmupTemplateRequestParser;
            this.bus = bus;
        }

        public void Handle(CommandLineMessage message)
        {
            var arguments = warmupTemplateRequestParser.GetRequest(message.CommandLineArguments);
            if (arguments.IsValid)
                bus.Send(arguments);
        }
    }
}