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
            var targetDir = new TargetDir(arguments.TokenReplaceValue);

            switch (WarmupConfiguration.settings.SourceControlType)
            {
                case SourceControlType.Subversion:
                    Console.WriteLine("svn exporting to: {0}", targetDir.FullPath);
                    Svn.Export(baseUri, targetDir);
                    break;
                case SourceControlType.Git:
                    Console.WriteLine("Hardcore git cloning action to: {0}", targetDir.FullPath);
                    Git.Clone(baseUri, targetDir);
                    break;
            }

            Console.WriteLine("replacing tokens");
            targetDir.ReplaceTokens(arguments.TokenReplaceValue);
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