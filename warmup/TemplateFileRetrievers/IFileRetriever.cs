using warmup.Messages;

namespace warmup.TemplateFileRetrievers
{
    public interface IFileRetriever
    {
        bool CanRetrieveTheFiles();
        void RetrieveTheFiles(WarmupRequestMessage requestMessage);
    }
}