using AppBus;
using warmup.Messages;

namespace warmup
{
    public class ProcessCommandLineWarmupRequest : IMessageHandler<CommandLineMessage>
    {
        private readonly IWarmupRequestMessageParser warmupRequestMessageParser;
        private readonly IApplicationBus bus;

        public ProcessCommandLineWarmupRequest(IWarmupRequestMessageParser warmupRequestMessageParser,
                                               IApplicationBus bus)
        {
            this.warmupRequestMessageParser = warmupRequestMessageParser;
            this.bus = bus;
        }

        public void Handle(CommandLineMessage message)
        {
            var arguments = warmupRequestMessageParser.GetRequest(message.CommandLineArguments);
            if (arguments.IsValid)
                bus.Send(arguments);
        }
    }
}