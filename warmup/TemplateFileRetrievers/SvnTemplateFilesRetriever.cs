using System;
using System.Diagnostics;
using System.IO;
using warmup.Messages;
using warmup.settings;

namespace warmup.TemplateFileRetrievers
{
    public class SvnTemplateFilesRetriever : IFileRetriever
    {
        private readonly IWarmupConfigurationProvider warmupConfigurationProvider;

        public SvnTemplateFilesRetriever(IWarmupConfigurationProvider warmupConfigurationProvider)
        {
            this.warmupConfigurationProvider = warmupConfigurationProvider;
        }

        public bool CanRetrieveTheFiles()
        {
            return TheSourceControlTypeIsSvn();
        }

        public void RetrieveTheFiles(WarmupRequestMessage requestMessage)
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

        private bool TheSourceControlTypeIsSvn()
        {
            return string.Compare(GetConfiguration().SourceControlType, "Svn", true) == 0;
        }

        private WarmupConfiguration GetConfiguration()
        {
            return warmupConfigurationProvider.GetWarmupConfiguration();
        }

        private ProcessStartInfo CreateProcessStartInfo(Uri sourceLocation, WarmupRequestMessage message)
        {
            var processStartInfo = new ProcessStartInfo("svn", string.Format("export {0} {1}", sourceLocation, Path.GetFullPath(message.TokenReplaceValue)));

            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;

            return processStartInfo;
        }
    }
}