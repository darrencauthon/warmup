using System;
using System.Linq;
using AppBus;
using warmup.settings;
using warmup.TemplateFileRetrievers;

namespace warmup
{
    public class WarmupTemplateRequestExecuter : IMessageHandler<WarmupTemplateRequest>
    {
        public void Handle(WarmupTemplateRequest warmupTemplateRequest)
        {
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
            return new TokensInFilesReplacer(new PathDeterminer(warmupTemplateRequest.TokenReplaceValue));
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
                                 if (retriever.CanRetrieveTheFiles())
                                     retriever.RetrieveTheFiles(warmupTemplateRequest);
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