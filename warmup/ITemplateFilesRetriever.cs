namespace warmup
{
    public interface ITemplateFilesRetriever
    {
        void RetrieveFiles(WarmupTemplateRequest warmupTemplateRequest);
        bool CanRetrieve();
    }
}