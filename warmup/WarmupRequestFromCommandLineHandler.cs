using AppBus;

namespace warmup
{
    public interface IWarmupRequestFromCommandLineHandler : IMessageHandler<ApplicationRanMessage>
    {
    }

    public class WarmupRequestFromCommandLineHandler : IWarmupRequestFromCommandLineHandler
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