using System.Collections.Generic;

namespace warmup
{
    public class CommandLineArgumentParser
    {
        public CommandLineArgumentSet GetArguments(string[] args)
        {
            return new CommandLineArgumentSet{
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

    public class CommandLineArgumentSet
    {
        public bool IsValid { get; set; }

        public string TemplateName { get; set; }

        public string TokenReplaceValue { get; set; }
    }
}