using AppBus;

namespace warmup.Messages
{
    public class ReplaceTokensInFileMessage : IEventMessage
    {
        public string TokenReplaceValue { get; set; }
    }
}