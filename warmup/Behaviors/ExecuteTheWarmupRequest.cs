using System;
using System.Linq;
using AppBus;
using warmup.Messages;
using warmup.settings;
using warmup.TemplateFileRetrievers;

namespace warmup.Behaviors
{
    public class ExecuteTheWarmupRequest : IMessageHandler<WarmupRequestMessage>
    {
        private readonly IApplicationBus applicationBus;

        public ExecuteTheWarmupRequest(IApplicationBus applicationBus)
        {
            this.applicationBus = applicationBus;
        }

        public void Handle(WarmupRequestMessage warmupRequestMessage)
        {
            RetrieveTheTemplateFiles(warmupRequestMessage);

            ReplaceTokensInTheTemplateFiles(warmupRequestMessage);
        }

        private void ReplaceTokensInTheTemplateFiles(WarmupRequestMessage warmupRequestMessage)
        {
            Console.WriteLine("replacing tokens");
            (CreateTokenFileReplacer()).ReplaceTokens(warmupRequestMessage.TokenReplaceValue);
        }

        private ITokensInFilesReplacer CreateTokenFileReplacer()
        {
            return new TokensInFilesReplacer(applicationBus);
        }

        private void RetrieveTheTemplateFiles(WarmupRequestMessage warmupRequestMessage)
        {
            GetTemplateFileRetrievers()
                .ToList()
                .ForEach(retriever =>
                             {
                                 if (retriever.CanRetrieveTheFiles())
                                     retriever.RetrieveTheFiles(warmupRequestMessage);
                             });
        }

        private IFileRetriever[] GetTemplateFileRetrievers()
        {
            var warmupConfigurationProvider = GetTheWarmupConfigurationProvider();
            return new IFileRetriever[]{
                                           new GitTemplateFilesRetriever(warmupConfigurationProvider, applicationBus),
                                           new SvnTemplateFilesRetriever(warmupConfigurationProvider, applicationBus),
                                       };
        }

        private static IWarmupConfigurationProvider GetTheWarmupConfigurationProvider()
        {
            return new ConfigurationFileWarmupConfigurationProvider();
        }
    }
}