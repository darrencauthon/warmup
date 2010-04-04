using System.Collections.Generic;
using AppBus;
using warmup.Messages;

namespace warmup.Behaviors
{
    public class ProcessCommandLineWarmupRequestBehavior : IMessageHandler<CommandLineMessage>
    {
        private readonly IWarmupRequestMessageParser warmupRequestMessageParser;
        private readonly IApplicationBus bus;

        public ProcessCommandLineWarmupRequestBehavior(IWarmupRequestMessageParser warmupRequestMessageParser,
                                                       IApplicationBus bus)
        {
            this.warmupRequestMessageParser = warmupRequestMessageParser;
            this.bus = bus;
        }

        public void Handle(CommandLineMessage message)
        {
            var arguments = warmupRequestMessageParser.GetRequest(message.CommandLineArguments);
            if (arguments.IsValid)
                bus.Send(arguments);
        }
    }

    public interface IWarmupRequestMessageParser
    {
        WarmupRequestMessage GetRequest(string[] args);
    }

    public class WarmupRequestMessageParser : IWarmupRequestMessageParser
    {
        public WarmupRequestMessage GetRequest(string[] args)
        {
            return new WarmupRequestMessage{
                                               IsValid = DetermineIfArgsAreValid(args),
                                               TemplateName = PullTemplateNameFromArgs(args),
                                               TokenReplaceValue = PullTokenReplaceValueFromArgs(args),
                                           };
        }

        private static string PullTokenReplaceValueFromArgs(string[] args)
        {
            return args.Length < 2 ? string.Empty : args[1];
        }

        private static string PullTemplateNameFromArgs(string[] args)
        {
            return args.Length == 0 ? string.Empty : args[0];
        }

        private static bool DetermineIfArgsAreValid(ICollection<string> args)
        {
            return args.Count == 2;
        }
    }
}