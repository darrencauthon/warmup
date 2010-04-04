using System;
using System.Linq;
using AppBus;
using warmup.Messages;
using warmup.TemplateFileRetrievers;

namespace warmup.Behaviors
{
    public class ExecuteTheWarmupRequestBehavior : IMessageHandler<WarmupRequestMessage>
    {
        private readonly IFileRetriever[] fileRetrievers;
        private readonly IApplicationBus applicationBus;

        public ExecuteTheWarmupRequestBehavior(IFileRetriever[] fileRetrievers, IApplicationBus applicationBus)
        {
            this.fileRetrievers = fileRetrievers;
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
            applicationBus.Send(new ReplaceTokensInFileMessage{ TokenReplaceValue = warmupRequestMessage.TokenReplaceValue});
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