using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Policy;

namespace CoAppPackageMaker.ViewModels
{
    class PackageViewModel : ExtraPropertiesViewModelBase
    {
        private string _name;
        private string _version;
        private string _architecture;
        private string _displayName;
        private string _location;
        private string _feed;
        private string _publisher;

        public PackageViewModel()
        {
        }

        public PackageViewModel(PackageReader reader)
        {
            string package = "package";
            Name = reader.GetRulesPropertyValueByName(package, "name");
            DisplayName = reader.GetRulesPropertyValueByName(package, "display-name");
            Architecture = reader.GetRulesPropertyValueByName(package, "arch");
            Feed = reader.GetRulesPropertyValueByName(package, "feed");
            Location = reader.GetRulesPropertyValueByName(package, "location");
            Publisher = reader.GetRulesPropertyValueByName(package, "publisher");
            Version = reader.GetRulesPropertyValueByName(package, "version");
        }



        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Version
        {
            get { return _version; }
            set
            {
                _version = value;
                OnPropertyChanged("Version");
            }
        }

        public string Architecture
        {
            get { return _architecture; }
            set
            {
                _architecture = value;
                OnPropertyChanged("Architecture");
            }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                OnPropertyChanged("DisplayName");
            }
        }

        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                OnPropertyChanged("Location");
            }
        }
        
        public string Feed
        {
            get { return _feed; }
            set
            {
                _feed = value;
                OnPropertyChanged("Feed");
            }
        }

        public string Publisher
        {
            get { return _publisher; }
            set
            {
                _publisher = value;
                OnPropertyChanged("Publisher");
            }
        }

        //tips, visualisation in extra class

    }

    public class PackageViewModelFactory : IFactory
    {
        public object CreateInstance(PackageReader reader)
        {
            string package = "package";
            PackageViewModel model=new PackageViewModel(reader)
                                       {

                                           Name = reader.GetRulesPropertyValueByName(package, "name"),
                                           DisplayName = reader.GetRulesPropertyValueByName(package, "display-name"),
                                           Architecture = reader.GetRulesPropertyValueByName(package, "arch"),
                                           Feed = reader.GetRulesPropertyValueByName(package, "feed"),
                                           Location = reader.GetRulesPropertyValueByName(package, "location"),
                                           Publisher = reader.GetRulesPropertyValueByName(package, "publisher"),
                                           Version = reader.GetRulesPropertyValueByName(package, "version")

                                       };
            return model;

        }
    }

    interface IFactory
    {
        object CreateInstance(PackageReader reader);
    }
}
