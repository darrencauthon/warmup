using System;
using System.Linq;
using AppBus;
using warmup.Messages;
using warmup.TemplateFileRetrievers;

namespace warmup.Behaviors
{
    public class ExecuteTheWarmupRequest : IMessageHandler<WarmupRequestMessage>
    {
        private readonly IFileRetriever[] fileRetrievers;
        private readonly ITokensInFilesReplacer tokensInFilesReplacer;

        public ExecuteTheWarmupRequest(IFileRetriever[] fileRetrievers, ITokensInFilesReplacer tokensInFilesReplacer)
        {
            this.fileRetrievers = fileRetrievers;
            this.tokensInFilesReplacer = tokensInFilesReplacer;
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
            return tokensInFilesReplacer;
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
            return fileRetrievers;
        }
    }
}