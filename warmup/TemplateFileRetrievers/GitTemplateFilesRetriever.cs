using System;
using System.Diagnostics;
using System.IO;
using warmup.Messages;
using warmup.settings;
using AppBus;

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
            var message = new GetTargetFilePathMessage{ TokenReplaceValue = requestMessage.TokenReplaceValue};
            bus.Send(message);
            var fullPath = message.Result.Path;

            Console.WriteLine("Hardcore git cloning action to: {0}", fullPath);

            var sourceLocationToGit = GetTheGitSourceLocation(requestMessage);
            if (string.IsNullOrEmpty(sourceLocationToGit) == false)
            {
                var psi = CreateProcessStartInfo(fullPath, sourceLocationToGit);

                //todo: better error handling
                Console.WriteLine("Running: {0} {1}", psi.FileName, psi.Arguments);

                string output, error;
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

        private bool TheSourceControlTypeIsGit()
        {
            return string.Compare(GetConfiguration().SourceControlType, "Git", true) == 0;
        }

        private WarmupConfiguration GetConfiguration()
        {
            return warmupConfigurationProvider.GetWarmupConfiguration();
        }

        private string GetTheGitSourceLocation(WarmupRequestMessage warmupRequestMessage)
        {
            var piecesOfPath = GetThePiecesOfPath(warmupRequestMessage);
            if (piecesOfPath.Length == 0)
                return string.Empty;
            return piecesOfPath[0] + ".git";
        }

        private static ProcessStartInfo CreateProcessStartInfo(string fullPath, string sourceLocationToGit)
        {
            var psi = new ProcessStartInfo("cmd",
                                           string.Format(" /c git clone {0} {1}", sourceLocationToGit, fullPath));

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            return psi;
        }

        private string[] GetThePiecesOfPath(WarmupRequestMessage warmupRequestMessage)
        {
            var separationCharacters = new[]{".git"};

            var sourceLocation = new Uri(GetConfiguration().SourceControlWarmupLocation + warmupRequestMessage.TemplateName);
            return sourceLocation.ToString().Split(separationCharacters, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}