using System;
using System.Linq;
using warmup.settings;
using warmup.TemplateFileRetrievers;

namespace warmup
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var commandLineArgumentSet = GetCommandLineArguments(args);

            DownloadTheTemplateFiles(commandLineArgumentSet);

            ReplaceTokensInTheTemplateFiles(commandLineArgumentSet);
        }

        private static void ReplaceTokensInTheTemplateFiles(CommandLineArgumentSet commandLineArgumentSet)
        {
            Console.WriteLine("replacing tokens");
            (new TargetDir(commandLineArgumentSet.TokenReplaceValue)).ReplaceTokens(commandLineArgumentSet.TokenReplaceValue);
        }

        private static void DownloadTheTemplateFiles(CommandLineArgumentSet commandLineArgumentSet)
        {
            GetTemplateFileRetrievers()
                .ToList()
                .ForEach(retriever =>
                             {
                                 if (retriever.CanRetrieve())
                                     retriever.RetrieveFiles(commandLineArgumentSet);
                             });
        }

        private static ITemplateFilesRetriever[] GetTemplateFileRetrievers()
        {
            var warmupConfigurationProvider = GetTheWarmupConfigurationProvider();
            return new ITemplateFilesRetriever[]{
                                                    new GitTemplateFilesRetriever(warmupConfigurationProvider),
                                                    new SvnTemplateFilesRetriever(warmupConfigurationProvider)
                                                };
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