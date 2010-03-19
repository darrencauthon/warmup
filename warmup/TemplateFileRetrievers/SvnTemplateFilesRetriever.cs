using System;
using System.Diagnostics;
using warmup.settings;

namespace warmup.TemplateFileRetrievers
{
    public class SvnTemplateFilesRetriever : ITemplateFilesRetriever
    {
        private readonly WarmupConfiguration configuration;

        public SvnTemplateFilesRetriever(IWarmupConfigurationProvider warmupConfigurationProvider)
        {
            configuration = warmupConfigurationProvider.GetWarmupConfiguration();
        }

        public bool CanExport()
        {
            return TheSourceControlTypeIsSvn();
        }

        private bool TheSourceControlTypeIsSvn()
        {
            return string.Compare(configuration.SourceControlType, "Svn", true) == 0;
        }

        public void Export(Uri sourceLocation, TargetDir target)
        {
            Console.WriteLine("svn exporting to: {0}", target.FullPath);

            var psi = new ProcessStartInfo("svn",
                                           string.Format("export {0} {1}", sourceLocation, target.FullPath));

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