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

            var warmupConfigurationProvider = GetTheWarmupConfigurationProvider();

            var baseUri = new Uri(warmupConfigurationProvider.GetWarmupConfiguration().SourceControlWarmupLocation + arguments.TemplateName);
            var targetDir = new TargetDir(arguments.TokenReplaceValue);

            var templateHandlers = new ISourceControlTemplateHandler[]{
                                                                          new Git(warmupConfigurationProvider),
                                                                          new Svn(warmupConfigurationProvider)
                                                                      };
            templateHandlers.ToList()
                .ForEach(handler =>
                             {
                                 if (handler.CanExport())
                                     handler.Export(baseUri, targetDir);
                             });
            return targetDir;
        }

        private static IWarmupConfigurationProvider GetTheWarmupConfigurationProvider()
        {
            return new ConfigurationFileWarmupConfigurationProvider();
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