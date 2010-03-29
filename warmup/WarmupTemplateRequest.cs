using warmup.Bus;

namespace warmup
{
    public class WarmupTemplateRequest : IEventMessage
    {
        public bool IsValid { get; set; }

        public string TemplateName { get; set; }

        public string TokenReplaceValue { get; set; }
    }
}