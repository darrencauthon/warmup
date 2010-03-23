using System;
using System.Diagnostics;
using warmup.settings;

namespace warmup.TemplateFileRetrievers
{
    public class SvnTemplateFilesRetriever : ITemplateFilesRetriever
    {
        private readonly IWarmupConfigurationProvider warmupConfigurationProvider;
        private readonly IPathDeterminer pathDeterminer;

        public SvnTemplateFilesRetriever(IWarmupConfigurationProvider warmupConfigurationProvider, IPathDeterminer pathDeterminer)
        {
            this.warmupConfigurationProvider = warmupConfigurationProvider;
            this.pathDeterminer = pathDeterminer;
        }

        public bool CanRetrieve()
        {
            return TheSourceControlTypeIsSvn();
        }

        private bool TheSourceControlTypeIsSvn()
        {
            return string.Compare(GetConfiguration().SourceControlType, "Svn", true) == 0;
        }

        private WarmupConfiguration GetConfiguration()
        {
            return warmupConfigurationProvider.GetWarmupConfiguration();
        }

        public void RetrieveFiles(WarmupTemplateRequest warmupTemplateRequest)
        {
            var sourceLocation = new Uri(GetConfiguration().SourceControlWarmupLocation + warmupTemplateRequest.TemplateName);

            Console.WriteLine("svn exporting to: {0}", pathDeterminer.FullPath);

            var psi = new ProcessStartInfo("svn", string.Format("export {0} {1}", sourceLocation, pathDeterminer.FullPath));

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

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
    }
}