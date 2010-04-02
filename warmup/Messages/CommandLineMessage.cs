using AppBus;

namespace warmup.Messages
{
    public class CommandLineMessage : IEventMessage
    {
        public string[] CommandLineArguments { get; set; }
    }
}