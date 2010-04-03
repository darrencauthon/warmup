using AppBus;

namespace warmup.Messages
{
    public class RetrieveFilesFromSvnRepositoryMessage : IEventMessage
    {
        public string TemplateName { get; set; }

        public string TokenReplaceValue { get; set; }
    }
}