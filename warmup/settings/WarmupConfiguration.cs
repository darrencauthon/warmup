using System.Configuration;

namespace warmup.settings
{
    /// <summary>
    /// Configuration Handler for Warmup
    /// </summary>
    public sealed class WarmupConfiguration : ConfigurationSection
    {
        private static readonly WarmupConfiguration _settings =
            ConfigurationManager.GetSection("warmup") as WarmupConfiguration;

        /// <summary>
        /// Settings section
        /// </summary>
        public static WarmupConfiguration settings
        {
            get { return _settings; }
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