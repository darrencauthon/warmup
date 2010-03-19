using System;
using warmup.settings;

namespace warmup
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var arguments = GetCommandLineArguments(args);

            var baseUri = new Uri(WarmupConfiguration.settings.SourceControlWarmupLocation + arguments.TemplateName);
            var td = new TargetDir(arguments.TokenReplaceValue);

            switch (WarmupConfiguration.settings.SourceControlType)
            {
                case SourceControlType.Subversion:
                    Console.WriteLine("svn exporting to: {0}", td.FullPath);
                    Svn.Export(baseUri, td);
                    break;
                case SourceControlType.Git:
                    Console.WriteLine("Hardcore git cloning action to: {0}", td.FullPath);
                    Git.Clone(baseUri, td);
                    break;
            }

            Console.WriteLine("replacing tokens");
            td.ReplaceTokens(arguments.TokenReplaceValue);
        }

        private static CommandLineArgumentSet GetCommandLineArguments(string[] args)
        {
            var parser = new CommandLineArgumentParser();
            var arguments = parser.GetArguments(args);
            if (arguments.IsValid == false)
                throw new ArgumentException("Command line arguments are not valid");
            return arguments;
        }
    }
}