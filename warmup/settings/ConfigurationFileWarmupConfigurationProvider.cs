namespace warmup.settings
{
    public class ConfigurationFileWarmupConfigurationProvider : IWarmupConfigurationProvider
    {
        public WarmupConfiguration GetWarmupConfiguration()
        {
            var settings = WarmupConfigurationFromConfigFile.settings;
            return new WarmupConfiguration(settings.SourceControlWarmupLocation, settings.SourceControlType);
        }
    }
}