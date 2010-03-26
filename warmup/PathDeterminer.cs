using System.Diagnostics;
using System.IO;

namespace warmup
{
    public interface IPathDeterminer
    {
        string FullPath { get; }
    }

    [DebuggerDisplay("{FullPath}")]
    public class PathDeterminer : IPathDeterminer
    {
        private readonly string path;

        public PathDeterminer(string path)
        {
            this.path = path;
        }

        public string FullPath
        {
            get { return Path.GetFullPath(path); }
        }
    }
}