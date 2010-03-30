using AppBus;

namespace warmup
{
    public interface IWarmupRequestFromCommandLineHandler
    {
        void Handle(ApplicationRanMessage message);
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

        public override void Handle(ApplicationRanMessage message)
        {
            bus.Send(warmupTemplateRequestParser.GetArguments(message.CommandLineArguments));
        }
    }
}