using System.Collections.Generic;
using warmup.Messages;

namespace warmup
{
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