using System.Configuration;

namespace warmup.settings
{
    /// <summary>
    /// Configuration Handler for Warmup
    /// </summary>
    public sealed class WarmupConfigurationFromConfigFile : ConfigurationSection
    {
        private static readonly WarmupConfigurationFromConfigFile Settings = ConfigurationManager.GetSection("warmup") as WarmupConfigurationFromConfigFile;

        /// <summary>
        /// Settings section
        /// </summary>
        public static WarmupConfigurationFromConfigFile settings
        {
            get { return Settings; }
        }

        /// <summary>
        /// The top level location for warmup templates
        /// </summary>
        [ConfigurationProperty("sourceControlWarmupLocation", IsRequired = true)]
        public string SourceControlWarmupLocation
        {
            get { return (string)this["sourceControlWarmupLocation"]; }
        }

        /// <summary>
        /// What type of source control are we using?
        /// </summary>
        [ConfigurationProperty("sourceControlType", IsRequired = false, DefaultValue = "svn")]
        public string SourceControl
        {
            get { return (string)this["sourceControlType"]; }
        }

        public string SourceControlType
        {
            get
            {
                if (SourceControl.Contains("git"))
                    return "git";

                return "svn";
            }
        }
    }
}