using System;

namespace warmup
{
    public interface ITemplateFilesRetriever
    {
        void RetrieveFiles(CommandLineArgumentSet commandLineArgumentSet);
        bool CanRetrieve();
    }
}