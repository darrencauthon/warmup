using warmup.Bus;

namespace warmup
{
    public interface IWarmupRequestFromCommandLineHandler : IMessageHandler<ApplicationRanMessage>
    {
    }

    public class WarmupRequestFromCommandLineHandler : MessageHandler<ApplicationRanMessage>, IWarmupRequestFromCommandLineHandler
    {
        private readonly IWarmupTemplateRequestParser warmupTemplateRequestParser;
        private readonly IApplicationBus bus;

        public WarmupRequestFromCommandLineHandler(IWarmupTemplateRequestParser warmupTemplateRequestParser,
                                                   IApplicationBus bus)
        {
            this.warmupTemplateRequestParser = warmupTemplateRequestParser;
            this.bus = bus;
        }

        public override bool CanHandle(ApplicationRanMessage message)
        {
            return GetWarmupTemplateRequest(message).IsValid;
        }

        public override void Handle(ApplicationRanMessage message)
        {
            bus.Send(warmupTemplateRequestParser.GetArguments(message.CommandLineArguments));
        }

        private WarmupTemplateRequest GetWarmupTemplateRequest(ApplicationRanMessage message)
        {
            return warmupTemplateRequestParser.GetArguments(message.CommandLineArguments);
        }
    }
}