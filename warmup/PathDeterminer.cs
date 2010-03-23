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
        private readonly string _path;

        public PathDeterminer(string path)
        {
            _path = path;
        }

        public string FullPath
        {
            get { return Path.GetFullPath(_path); }
        }
    }
}