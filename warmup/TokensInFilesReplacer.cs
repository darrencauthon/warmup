using System;
using System.IO;
using System.Linq;
using AppBus;
using warmup.Messages;

namespace warmup
{
    public interface ITokensInFilesReplacer
    {
        void ReplaceTokens(string name);
    }

    public class TokensInFilesReplacer : ITokensInFilesReplacer
    {
        private readonly IApplicationBus applicationBus;

        public TokensInFilesReplacer(IApplicationBus applicationBus)
        {
            this.applicationBus = applicationBus;
        }

        public void ReplaceTokens(string name)
        {
            var startingPoint = new DirectoryInfo(GetThePath(name));

            //move all directories
            MoveAllDirectories(startingPoint, name);

            startingPoint = new DirectoryInfo(startingPoint.FullName.Replace("__NAME__", name));

            //move all files
            MoveAllFiles(startingPoint, name);

            //replace file content
            ReplaceTokensInTheFiles(startingPoint, name);
        }

        private string GetThePath(string name)
        {
            var message = new GetTargetFilePathMessage{TokenReplaceValue = name};
            applicationBus.Send(message);
            return message.Result.Path;
        }

        private static void ReplaceTokensInTheFiles(DirectoryInfo point, string name)
        {
            foreach (var info in point.GetFiles("*.*", SearchOption.AllDirectories))
            {
                //don't do this on exe's or dll's
                if (new[]{".exe", ".dll", ".pdb", ".jpg", ".png", ".gif", ".mst", ".msi", ".msm", ".gitignore", ".idx", ".pack"}.Contains(info.Extension)) continue;
                //skip the .git directory
                if (new[]{"\\.git\\"}.Contains(info.FullName)) continue;

                //process contents
                var contents = File.ReadAllText(info.FullName);
                contents = contents.Replace("__NAME__", name);
                AttemptToSaveTheContentsOfTheFile(info, contents);
            }
        }

        private static void AttemptToSaveTheContentsOfTheFile(FileSystemInfo info, string contents)
        {
            try
            {
                File.WriteAllText(info.FullName, contents);
            }
            catch (Exception)
            {
                // move on if it fails :(
            }
        }

        private static void MoveAllFiles(DirectoryInfo point, string name)
        {
            foreach (var file in point.GetFiles("*.*", SearchOption.AllDirectories))
            {
                var moveTo = file.FullName.Replace("__NAME__", name);
                try
                {
                    file.MoveTo(moveTo);
                }
                catch (Exception)
                {
                    Console.WriteLine("Trying to move '{0}' to '{1}'", file.FullName, moveTo);
                    throw;
                }
            }
        }

        private static void MoveAllDirectories(DirectoryInfo dir, string name)
        {
            var workingDirectory = dir;
            if (workingDirectory.Name.Contains("__NAME__"))
            {
                var newFolderName = dir.Name.Replace("__NAME__", name);
                if (dir.Parent != null)
                {
                    var moveTo = Path.Combine(dir.Parent.FullName, newFolderName);

                    try
                    {
                        workingDirectory.MoveTo(moveTo);
                        workingDirectory = new DirectoryInfo(moveTo);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Trying to move '{0}' to '{1}'", workingDirectory.FullName, moveTo);
                        throw;
                    }
                }
            }

            foreach (var info in workingDirectory.GetDirectories())
            {
                MoveAllDirectories(info, name);
            }
        }
    }
}