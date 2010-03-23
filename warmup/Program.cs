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
            var warmupTemplateRequest = GetWarmupTemplateRequest(args);

            RetrieveTheTemplateFiles(warmupTemplateRequest);

            ReplaceTokensInTheTemplateFiles(warmupTemplateRequest);
        }

        private static void ReplaceTokensInTheTemplateFiles(WarmupTemplateRequest warmupTemplateRequest)
        {
            Console.WriteLine("replacing tokens");
            (CreateTokenFileReplacer(warmupTemplateRequest)).ReplaceTokens(warmupTemplateRequest.TokenReplaceValue);
        }

        private static ITokensInFilesReplacer CreateTokenFileReplacer(WarmupTemplateRequest warmupTemplateRequest)
        {
            return new PathDeterminer(warmupTemplateRequest.TokenReplaceValue);
        }

        private static IPathDeterminer CreatePathDeterminer(WarmupTemplateRequest warmupTemplateRequest)
        {
            return new PathDeterminer(warmupTemplateRequest.TokenReplaceValue);
        }

        private static void RetrieveTheTemplateFiles(WarmupTemplateRequest warmupTemplateRequest)
        {
            GetTemplateFileRetrievers(CreatePathDeterminer(warmupTemplateRequest))
                .ToList()
                .ForEach(retriever =>
                             {
                                 if (retriever.CanRetrieve())
                                     retriever.RetrieveFiles(warmupTemplateRequest);
                             });
        }

        private static ITemplateFilesRetriever[] GetTemplateFileRetrievers(IPathDeterminer pathDeterminer)
        {
            var warmupConfigurationProvider = GetTheWarmupConfigurationProvider();
            return new ITemplateFilesRetriever[]{
                                                    new GitTemplateFilesRetriever(warmupConfigurationProvider, pathDeterminer),
                                                    new SvnTemplateFilesRetriever(warmupConfigurationProvider, pathDeterminer)
                                                };
        }

        private static IWarmupConfigurationProvider GetTheWarmupConfigurationProvider()
        {
            return new ConfigurationFileWarmupConfigurationProvider();
        }

        private static WarmupTemplateRequest GetWarmupTemplateRequest(string[] args)
        {
            var parser = new WarmupTemplateRequestParser();
            var arguments = parser.GetArguments(args);
            if (arguments.IsValid == false)
                throw new ArgumentException("Command line arguments are not valid");
            return arguments;
        }
    }
}