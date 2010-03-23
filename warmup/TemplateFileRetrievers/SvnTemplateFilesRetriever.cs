using System;
using System.Diagnostics;
using warmup.settings;

namespace warmup.TemplateFileRetrievers
{
    public class SvnTemplateFilesRetriever : ITemplateFilesRetriever
    {
        private readonly IPathDeterminer pathDeterminer;
        private readonly WarmupConfiguration configuration;

        public SvnTemplateFilesRetriever(IWarmupConfigurationProvider warmupConfigurationProvider, IPathDeterminer pathDeterminer)
        {
            this.pathDeterminer = pathDeterminer;
            configuration = warmupConfigurationProvider.GetWarmupConfiguration();
        }

        public bool CanRetrieve()
        {
            return TheSourceControlTypeIsSvn();
        }

        private bool TheSourceControlTypeIsSvn()
        {
            return string.Compare(configuration.SourceControlType, "Svn", true) == 0;
        }

        public void RetrieveFiles(WarmupTemplateRequest warmupTemplateRequest)
        {
            var sourceLocation = new Uri(configuration.SourceControlWarmupLocation + warmupTemplateRequest.TemplateName);

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