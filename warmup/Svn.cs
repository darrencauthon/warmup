using System;
using System.Diagnostics;
using warmup.settings;

namespace warmup
{
    public class Svn : ISourceControlTemplateHandler
    {
        private readonly WarmupConfiguration configuration;

        public Svn(WarmupConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public bool CanExport()
        {
            return configuration.SourceControlType == SourceControlType.Subversion;
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