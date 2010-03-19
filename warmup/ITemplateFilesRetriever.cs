using System;

namespace warmup
{
    public interface ITemplateFilesRetriever
    {
        void RetrieveFiles(Uri sourceLocation, TargetDir target);
        bool CanRetrieve();
    }
}