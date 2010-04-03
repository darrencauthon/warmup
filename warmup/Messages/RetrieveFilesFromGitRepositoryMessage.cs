using AppBus;

namespace warmup.Messages
{
    public class RetrieveFilesFromGitRepositoryMessage : IEventMessage
    {
        public string TemplateName { get; set; }

        public string TokenReplaceValue { get; set; }
    }
}