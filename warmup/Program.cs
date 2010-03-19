using System;
using System.Linq;
using warmup.settings;

namespace warmup
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var arguments = GetCommandLineArguments(args);

            var targetDir = PullDownTheTemplate(arguments);

            ReplaceTokensInTheTemplate(arguments, targetDir);
        }

        private static void ReplaceTokensInTheTemplate(CommandLineArgumentSet arguments, TargetDir targetDir)
        {
            Console.WriteLine("replacing tokens");
            targetDir.ReplaceTokens(arguments.TokenReplaceValue);
        }

        private static TargetDir PullDownTheTemplate(CommandLineArgumentSet arguments)
        {
            var baseUri = new Uri(WarmupConfiguration.settings.SourceControlWarmupLocation + arguments.TemplateName);
            var targetDir = new TargetDir(arguments.TokenReplaceValue);

            var templateHandlers = new ISourceControlTemplateHandler[]{
                                                                          new Git(WarmupConfiguration.settings),
                                                                          new Svn(WarmupConfiguration.settings)
                                                                      };
            templateHandlers.ToList()
                .ForEach(handler =>
                             {
                                 if (handler.CanExport())
                                     handler.Export(baseUri, targetDir);
                             });
            return targetDir;
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