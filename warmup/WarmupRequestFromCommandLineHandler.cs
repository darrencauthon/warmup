using AppBus;

namespace warmup
{
    public class WarmupRequestFromCommandLineHandler : IMessageHandler<ApplicationRanMessage>
    {
        private readonly IWarmupTemplateRequestParser warmupTemplateRequestParser;
        private readonly IApplicationBus bus;

        public WarmupRequestFromCommandLineHandler(IWarmupTemplateRequestParser warmupTemplateRequestParser,
                                                   IApplicationBus bus)
        {
            this.warmupTemplateRequestParser = warmupTemplateRequestParser;
            this.bus = bus;
        }

        public void Handle(ApplicationRanMessage message)
        {
            bus.Send(warmupTemplateRequestParser.GetArguments(message.CommandLineArguments));
        }
    }
}