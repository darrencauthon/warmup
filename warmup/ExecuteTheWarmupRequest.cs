using System;
using System.Linq;
using AppBus;
using warmup.Messages;
using warmup.settings;
using warmup.TemplateFileRetrievers;

namespace warmup
{
    public class ExecuteTheWarmupRequest : IMessageHandler<WarmupRequestMessage>
    {
        public void Handle(WarmupRequestMessage warmupRequestMessage)
        {
            RetrieveTheTemplateFiles(warmupRequestMessage);

            ReplaceTokensInTheTemplateFiles(warmupRequestMessage);
        }

        private static void ReplaceTokensInTheTemplateFiles(WarmupRequestMessage warmupRequestMessage)
        {
            Console.WriteLine("replacing tokens");
            (CreateTokenFileReplacer(warmupRequestMessage)).ReplaceTokens(warmupRequestMessage.TokenReplaceValue);
        }

        private static ITokensInFilesReplacer CreateTokenFileReplacer(WarmupRequestMessage warmupRequestMessage)
        {
            return new TokensInFilesReplacer();
        }

        private static void RetrieveTheTemplateFiles(WarmupRequestMessage warmupRequestMessage)
        {
            GetTemplateFileRetrievers()
                .ToList()
                .ForEach(retriever =>
                             {
                                 if (retriever.CanRetrieveTheFiles())
                                     retriever.RetrieveTheFiles(warmupRequestMessage);
                             });
        }

        private static IFileRetriever[] GetTemplateFileRetrievers()
        {
            var warmupConfigurationProvider = GetTheWarmupConfigurationProvider();
            return new IFileRetriever[]{
                                           new GitTemplateFilesRetriever(warmupConfigurationProvider),
                                           new SvnTemplateFilesRetriever(warmupConfigurationProvider),
                                       };
        }

        private static IWarmupConfigurationProvider GetTheWarmupConfigurationProvider()
        {
            return new ConfigurationFileWarmupConfigurationProvider();
        }
    }
}