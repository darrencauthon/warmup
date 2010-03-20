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

            RetrieveTheTemplateFiles(commandLineArgumentSet);

            ReplaceTokensInTheTemplateFiles(commandLineArgumentSet);
        }

        private static void ReplaceTokensInTheTemplateFiles(WarmupTemplateRequest warmupTemplateRequest)
        {
            Console.WriteLine("replacing tokens");
            (new TargetDir(warmupTemplateRequest.TokenReplaceValue)).ReplaceTokens(warmupTemplateRequest.TokenReplaceValue);
        }

        private static void RetrieveTheTemplateFiles(WarmupTemplateRequest warmupTemplateRequest)
        {
            GetTemplateFileRetrievers()
                .ToList()
                .ForEach(retriever =>
                             {
                                 if (retriever.CanRetrieve())
                                     retriever.RetrieveFiles(warmupTemplateRequest);
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

        private static WarmupTemplateRequest GetCommandLineArguments(string[] args)
        {
            var parser = new WarmupTemplateRequestParser();
            var arguments = parser.GetArguments(args);
            if (arguments.IsValid == false)
                throw new ArgumentException("Command line arguments are not valid");
            return arguments;
        }
    }
}