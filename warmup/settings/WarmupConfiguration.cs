namespace warmup.settings
{
    public class WarmupConfiguration
    {
        public WarmupConfiguration(string sourceControlWarmupLocation, string sourceControlType)
        {
            SourceControlWarmupLocation = sourceControlWarmupLocation;
            SourceControlType = sourceControlType;
        }

        public string SourceControlWarmupLocation { get; private set; }

        public string SourceControlType { get; private set; }
    }
}