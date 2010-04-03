using System;
using AppBus;
using warmup.Messages;
using warmup.settings;

namespace warmup.TemplateFileRetrievers
{
    public class GitTemplateFilesRetriever : IFileRetriever
    {
        private readonly IWarmupConfigurationProvider warmupConfigurationProvider;
        private readonly IApplicationBus bus;

        public GitTemplateFilesRetriever(IWarmupConfigurationProvider warmupConfigurationProvider, IApplicationBus bus)
        {
            this.warmupConfigurationProvider = warmupConfigurationProvider;
            this.bus = bus;
        }

        public bool CanRetrieveTheFiles()
        {
            return TheSourceControlTypeIsGit();
        }

        public void RetrieveTheFiles(WarmupRequestMessage requestMessage)
        {
            if (CanRetrieveTheFiles() == false)
                throw new InvalidOperationException("System cannot retrieve the files using git");

            PutAMessageOnTheBusToRetrieveTheFiles(requestMessage);
        }

        private void PutAMessageOnTheBusToRetrieveTheFiles(WarmupRequestMessage requestMessage)
        {
            bus.Send(new RetrieveFilesFromGitRepositoryMessage{
                                                                  TemplateName = requestMessage.TemplateName,
                                                                  TokenReplaceValue = requestMessage.TokenReplaceValue
                                                              });
        }

        private bool TheSourceControlTypeIsGit()
        {
            return string.Compare(GetConfiguration().SourceControlType, "Git", true) == 0;
        }

        private WarmupConfiguration GetConfiguration()
        {
            return warmupConfigurationProvider.GetWarmupConfiguration();
        }
    }
}