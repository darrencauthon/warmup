using System;
using System.Diagnostics;

namespace warmup
{
    public class Git : ISourceControlTemplateHandler
    {
        public void Export(Uri sourceLocation, TargetDir target)
        {
            var separationCharacters = new[]{".git"};
            var piecesOfPath = sourceLocation.ToString().Split(separationCharacters, StringSplitOptions.RemoveEmptyEntries);
            if (piecesOfPath != null && piecesOfPath.Length > 0)
            {
                var sourceLocationToGit = piecesOfPath[0] + ".git";

                var psi = new ProcessStartInfo("cmd",
                                               string.Format(" /c git clone {0} {1}", sourceLocationToGit, target.FullPath));

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

                //string git_directory = Path.Combine(target.FullPath, ".git");
                //if (Directory.Exists(git_directory))
                //{
                //    Console.WriteLine("Deleting {0} directory", git_directory);
                //    Directory.Delete(git_directory, true);
                //}
            }
        }
    }
}