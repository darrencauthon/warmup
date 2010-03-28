using warmup.Bus;

namespace warmup
{
    public class WarmupRequestFromCommandLineHandler : MessageHandler<ApplicationRanMessage>
    {
        private readonly IWarmupTemplateRequestParser warmupTemplateRequestParser;
        private readonly IWarmupTemplateRequestExecuter warmupTemplateRequestExecuter;

        public WarmupRequestFromCommandLineHandler(IWarmupTemplateRequestParser warmupTemplateRequestParser,
                                                   IWarmupTemplateRequestExecuter warmupTemplateRequestExecuter)
        {
            this.warmupTemplateRequestParser = warmupTemplateRequestParser;
            this.warmupTemplateRequestExecuter = warmupTemplateRequestExecuter;
        }

        public override bool CanHandle(ApplicationRanMessage message)
        {
            return GetWarmupTemplateRequest(message).IsValid;
        }

        public override void Handle(ApplicationRanMessage message)
        {
            warmupTemplateRequestExecuter.Execute(GetWarmupTemplateRequest(message));
        }

        private WarmupTemplateRequest GetWarmupTemplateRequest(ApplicationRanMessage message)
        {
            return warmupTemplateRequestParser.GetArguments(message.CommandLineArguments);
        }
    }
}