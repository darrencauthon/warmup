namespace warmup.TemplateFileRetrievers
{
    public interface IFileRetriever
    {
        bool CanRetrieveTheFiles();
        void RetrieveTheFiles(WarmupTemplateRequest request);
    }
}