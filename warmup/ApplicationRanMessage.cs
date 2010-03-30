using AppBus;

namespace warmup
{
    public class ApplicationRanMessage : IEventMessage
    {
        public string[] CommandLineArguments { get; set; }
    }
}