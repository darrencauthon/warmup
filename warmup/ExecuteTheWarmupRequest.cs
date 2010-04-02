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
            return new TokensInFilesReplacer(new PathDeterminer(warmupRequestMessage.TokenReplaceValue));
        }

        private static IPathDeterminer CreatePathDeterminer(WarmupRequestMessage warmupRequestMessage)
        {
            return new PathDeterminer(warmupRequestMessage.TokenReplaceValue);
        }

        private static void RetrieveTheTemplateFiles(WarmupRequestMessage warmupRequestMessage)
        {
            GetTemplateFileRetrievers(CreatePathDeterminer(warmupRequestMessage))
                .ToList()
                .ForEach(retriever =>
                             {
                                 if (retriever.CanRetrieveTheFiles())
                                     retriever.RetrieveTheFiles(warmupRequestMessage);
                             });
        }

        private static IFileRetriever[] GetTemplateFileRetrievers(IPathDeterminer pathDeterminer)
        {
            var warmupConfigurationProvider = GetTheWarmupConfigurationProvider();
            return new IFileRetriever[]{
                                           new GitTemplateFilesRetriever(warmupConfigurationProvider, pathDeterminer),
                                           new SvnTemplateFilesRetriever(warmupConfigurationProvider, pathDeterminer),
                                       };
        }

        private static IWarmupConfigurationProvider GetTheWarmupConfigurationProvider()
        {
            return new ConfigurationFileWarmupConfigurationProvider();
        }
    }
}