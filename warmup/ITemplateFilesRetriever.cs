using System;

namespace warmup
{
    public interface ITemplateFilesRetriever
    {
        void Export(Uri sourceLocation, TargetDir target);
        bool CanExport();
    }
}