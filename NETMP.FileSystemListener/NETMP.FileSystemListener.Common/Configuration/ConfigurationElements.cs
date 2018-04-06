using System.Configuration;
using NETMP.FileSystemListener.Common.Configuration;

namespace NETMP.FileSystemListener.Common.Configuration
{
    public interface IConfigurationCollectionElement
    {
        object Key { get; }
    }

    public class ObservableFolderElement : ConfigurationElement, IConfigurationCollectionElement
    {
        [ConfigurationProperty("path", IsKey = true, IsRequired = true)]
        public string Path =>  (string)base["path"];

        public object Key => Path; 
        }
    }

    public class RuleElement : ConfigurationElement, IConfigurationCollectionElement
    {
        [ConfigurationProperty("filePathTemplate", IsKey = true, IsRequired = true)]
        public string FilePathTemplate => (string)base["filePathTemplate"]; 

        [ConfigurationProperty("movingDestinationFolder", IsRequired = true)]
        public string MovingDestinationFolder => (string)base["movingDestinationFolder"];

        [ConfigurationProperty("addIndexNumber", IsRequired = false)]
        public bool AddIndexNumber => (bool)base["addIndexNumber"];

        [ConfigurationProperty("addMovingDate", IsRequired = false)]
        public bool AddMovingDate => (bool)base["addMovingDate"];

        public object Key => FilePathTemplate;
    }
