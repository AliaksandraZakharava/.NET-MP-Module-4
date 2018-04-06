using System.Configuration;

namespace NETMP.FileSystemListener.Common.Configuration
{
    public class FileSystemListenerConfig : ConfigurationSection
    {
        [ConfigurationProperty("culture")]
        public string Culture => (string)base["culture"];

        [ConfigurationCollection(typeof(ObservableFolderElement), AddItemName = "folder")]
        [ConfigurationProperty("observableFolders")]
        public EnumerableConfigurationElementCollection<ObservableFolderElement> ObservableFolders
            => (EnumerableConfigurationElementCollection<ObservableFolderElement>)this["observableFolders"];

        [ConfigurationCollection(typeof(RuleElement), AddItemName = "rule")]
        [ConfigurationProperty("rules")]
        public EnumerableConfigurationElementCollection<RuleElement> Rules
            => (EnumerableConfigurationElementCollection<RuleElement>)this["rules"];
    }
}
