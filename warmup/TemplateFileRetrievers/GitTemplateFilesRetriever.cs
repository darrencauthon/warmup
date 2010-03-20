using System;
using System.Diagnostics;
using warmup.settings;

namespace warmup.TemplateFileRetrievers
{
    public class GitTemplateFilesRetriever : ITemplateFilesRetriever
    {
        private readonly WarmupConfiguration configuration;

        public GitTemplateFilesRetriever(IWarmupConfigurationProvider warmupConfigurationProvider)
        {
            configuration = warmupConfigurationProvider.GetWarmupConfiguration();
        }

        public bool CanRetrieve()
        {
            return TheSourceControlTypeIsGit();
        }

        private bool TheSourceControlTypeIsGit()
        {
            return string.Compare(configuration.SourceControlType, "Git", true) == 0;
        }

        public void RetrieveFiles(WarmupTemplateRequest warmupTemplateRequest)
        {
            var fullPath = new TargetDir(warmupTemplateRequest.TokenReplaceValue).FullPath;
            Console.WriteLine("Hardcore git cloning action to: {0}", fullPath);

            var sourceLocationToGit = GetTheGitSourceLocation(warmupTemplateRequest);
            if (string.IsNullOrEmpty(sourceLocationToGit) == false)
            {
                var psi = CreateProcessStartInfo(fullPath, sourceLocationToGit);

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

                //string git_directory = Path.Combine(target.FullPath, ".git");
                //if (Directory.Exists(git_directory))
                //{
                //    Console.WriteLine("Deleting {0} directory", git_directory);
                //    Directory.Delete(git_directory, true);
                //}
            }
        }

        private string GetTheGitSourceLocation(WarmupTemplateRequest warmupTemplateRequest)
        {
            var piecesOfPath = GetThePiecesOfPath(warmupTemplateRequest);
            if (piecesOfPath.Length > 0)
                return string.Empty;
            return piecesOfPath[0] + ".git";
        }

        private ProcessStartInfo CreateProcessStartInfo(string fullPath, string sourceLocationToGit)
        {
            var psi = new ProcessStartInfo("cmd",
                                           string.Format(" /c git clone {0} {1}", sourceLocationToGit, fullPath));

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            return psi;
        }

        private string[] GetThePiecesOfPath(WarmupTemplateRequest warmupTemplateRequest)
        {
            var separationCharacters = new[]{".git"};

            var sourceLocation = new Uri(configuration.SourceControlWarmupLocation + warmupTemplateRequest.TemplateName);
            return sourceLocation.ToString().Split(separationCharacters, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}