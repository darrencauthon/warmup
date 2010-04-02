using AppBus;

namespace warmup
{
    public class CommandLineMessage : IEventMessage
    {
        public string[] CommandLineArguments { get; set; }
    }
}