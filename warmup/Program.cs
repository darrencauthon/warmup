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
            var arguments = GetCommandLineArguments(args);

            DownloadTheTemplateFiles(arguments);

            ReplaceTokensInTheTemplateFiles(arguments);
        }

        private static void ReplaceTokensInTheTemplateFiles(CommandLineArgumentSet arguments)
        {
            Console.WriteLine("replacing tokens");
            (new TargetDir(arguments.TokenReplaceValue)).ReplaceTokens(arguments.TokenReplaceValue);
        }

        private static void DownloadTheTemplateFiles(CommandLineArgumentSet arguments)
        {
            GetTemplateFileRetrievers()
                .ToList()
                .ForEach(retriever =>
                             {
                                 if (retriever.CanRetrieve())
                                     retriever.RetrieveFiles(arguments);
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