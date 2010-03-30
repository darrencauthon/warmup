namespace warmup
{
    public interface ITemplateFilesRetriever
    {
        void Handle(WarmupTemplateRequest warmupTemplateRequest);
        bool CanRetrieve();
    }
}