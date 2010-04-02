using System;
using System.Diagnostics;
using warmup.Messages;
using warmup.settings;

namespace warmup.TemplateFileRetrievers
{
    public class SvnTemplateFilesRetriever : IFileRetriever
    {
        private readonly IWarmupConfigurationProvider warmupConfigurationProvider;
        private readonly IPathDeterminer pathDeterminer;

        public SvnTemplateFilesRetriever(IWarmupConfigurationProvider warmupConfigurationProvider, IPathDeterminer pathDeterminer)
        {
            this.warmupConfigurationProvider = warmupConfigurationProvider;
            this.pathDeterminer = pathDeterminer;
        }

        public bool CanRetrieveTheFiles()
        {
            return TheSourceControlTypeIsSvn();
        }

        public void RetrieveTheFiles(WarmupRequestMessage requestMessage)
        {
            var sourceLocation = new Uri(GetConfiguration().SourceControlWarmupLocation + requestMessage.TemplateName);

            Console.WriteLine("svn exporting to: {0}", pathDeterminer.FullPath);

            var psi = CreateProcessStartInfo(sourceLocation);

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

        private ProcessStartInfo CreateProcessStartInfo(Uri sourceLocation)
        {
            var processStartInfo = new ProcessStartInfo("svn", string.Format("export {0} {1}", sourceLocation, pathDeterminer.FullPath));

            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;

            return processStartInfo;
        }
    }
}