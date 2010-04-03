using System;
using System.Diagnostics;
using AppBus;
using warmup.Messages;
using warmup.settings;

namespace warmup.Behaviors
{
    public class RetrieveFilesFromTheSvnRepository : IMessageHandler<RetrieveFilesFromSvnRepositoryMessage>
    {
        private readonly IApplicationBus applicationBus;
        private readonly IWarmupConfigurationProvider warmupConfigurationProvider;

        public RetrieveFilesFromTheSvnRepository(IApplicationBus applicationBus,
                                                 IWarmupConfigurationProvider warmupConfigurationProvider)
        {
            this.applicationBus = applicationBus;
            this.warmupConfigurationProvider = warmupConfigurationProvider;
        }

        public void Handle(RetrieveFilesFromSvnRepositoryMessage requestMessage)
        {
            var sourceLocation = new Uri(GetConfiguration().SourceControlWarmupLocation + requestMessage.TemplateName);

            var psi = CreateProcessStartInfo(sourceLocation, requestMessage);

            //todo: better error handling
            Console.WriteLine("Running: {0} {1}", psi.FileName, psi.Arguments);
            string output, error = "";
            using (var p = Process.Start(psi))
            {
                output = p.StandardOutput.ReadToEnd();
                error = p.StandardError.ReadToEnd();
            }

            Console.WriteLine(output);
            Console.WriteLine(error);
        }

        private WarmupConfiguration GetConfiguration()
        {
            return warmupConfigurationProvider.GetWarmupConfiguration();
        }

        private ProcessStartInfo CreateProcessStartInfo(Uri sourceLocation, RetrieveFilesFromSvnRepositoryMessage warmupRequestMessage)
        {
            var processStartInfo = new ProcessStartInfo("svn", string.Format("export {0} {1}", sourceLocation, GetThePath(warmupRequestMessage)));

            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;

            return processStartInfo;
        }

        private string GetThePath(RetrieveFilesFromSvnRepositoryMessage requestMessage)
        {
            var message = new GetTargetFilePathMessage{TokenReplaceValue = requestMessage.TokenReplaceValue};
            applicationBus.Send(message);
            return message.Result.Path;
        }
    }
}