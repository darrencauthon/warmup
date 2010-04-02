using AppBus;

namespace warmup.Messages
{
    public class WarmupRequestMessage : IEventMessage
    {
        public bool IsValid { get; set; }

        public string TemplateName { get; set; }

        public string TokenReplaceValue { get; set; }
    }
}