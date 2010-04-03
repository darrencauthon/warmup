using System;
using AppBus;
using warmup.Messages;
using warmup.settings;

namespace warmup.TemplateFileRetrievers
{
    public class SvnTemplateFilesRetriever : IFileRetriever
    {
        private readonly IWarmupConfigurationProvider warmupConfigurationProvider;
        private readonly IApplicationBus applicationBus;

        public SvnTemplateFilesRetriever(IWarmupConfigurationProvider warmupConfigurationProvider, IApplicationBus applicationBus)
        {
            this.warmupConfigurationProvider = warmupConfigurationProvider;
            this.applicationBus = applicationBus;
        }

        public bool CanRetrieveTheFiles()
        {
            return TheSourceControlTypeIsSvn();
        }

        public void RetrieveTheFiles(WarmupRequestMessage requestMessage)
        {
            if (CanRetrieveTheFiles() == false)
                throw new InvalidOperationException("System cannot retrieve the files using svn");

            applicationBus.Send(new RetrieveFilesFromSvnRepositoryMessage{
                                                                             TemplateName = requestMessage.TemplateName,
                                                                             TokenReplaceValue = requestMessage.TokenReplaceValue,
                                                                         });
        }

        private bool TheSourceControlTypeIsSvn()
        {
            return string.Compare(GetConfiguration().SourceControlType, "Svn", true) == 0;
        }

        private WarmupConfiguration GetConfiguration()
        {
            return warmupConfigurationProvider.GetWarmupConfiguration();
        }
    }
}