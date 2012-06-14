using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

using System.Security.Policy;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels
{
    class PackageViewModel : ExtraPropertiesViewModelBase,ISupportsUndo
    {
        
  
        public PackageViewModel()
        {

        }

        public PackageViewModel(PackageReader reader)
        {
            string package = "package";

            SourcePackageViewModel=new PackageViewModel()
                                     {
                                       
            Name = reader.GetRulesSourcePropertyValueByName(package, "name"),
            DisplayName = reader.GetRulesSourcePropertyValueByName(package, "display-name"),
            Architecture = reader.GetRulesSourcePropertyValueByName(package, "arch"),
            Feed = reader.GetRulesSourcePropertyValueByName(package, "feed"),
            Location = reader.GetRulesSourcePropertyValueByName(package, "location"),
            Publisher = reader.GetRulesSourcePropertyValueByName(package, "publisher"),
            Version = reader.GetRulesSourcePropertyValueByName(package, "version"),
            IsEditable = true,


                                     };

            Name = reader.GetRulesPropertyValueByName(package, "name");
            DisplayName = reader.GetRulesPropertyValueByName(package, "display-name");
            Architecture = reader.GetRulesPropertyValueByName(package, "arch");
            Feed = reader.GetRulesPropertyValueByName(package, "feed");
            Location = reader.GetRulesPropertyValueByName(package, "location");
            Publisher = reader.GetRulesPropertyValueByName(package, "publisher");
            Version = reader.GetRulesPropertyValueByName(package, "version");
            SourceString = reader.GetRulesSourceStringPropertyValueByName(package);
            IsEditable = false;

            SourcePackageViewModel.PropertyChanged += EvaluatedChanged;
        }

        private PackageViewModel _sourcePackageViewModel;
        public PackageViewModel SourcePackageViewModel
        {
            get { return _sourcePackageViewModel; }
            set
            {
                _sourcePackageViewModel = value;
                OnPropertyChanged(" SourcePackageViewModel");
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
               DefaultChangeFactory.OnChanging(this, "Name", _name, value);
               _name = value;
               OnPropertyChanged("Name");
              
            }
        }

        private string _version;
        public string Version
        {
            get { return _version; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "Version", _version, value);
                _version = value;
                OnPropertyChanged("Version");
             
            }
        }

        private string _architecture;
        public string Architecture
        {
            get { return _architecture; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "Architecture", _architecture, value);
                _architecture = value;
                OnPropertyChanged("Architecture");
            }
        }

        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "DisplayName", _displayName, value);
                _displayName = value;
                OnPropertyChanged("DisplayName");
            }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "Location", _location, value);
                _location = value;
                OnPropertyChanged("Location");
            }
        }

        private string _feed;
        public string Feed
        {
            get { return _feed; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "Feed", _feed, value);
                _feed = value;
                OnPropertyChanged("Feed");
            }
        }

        private string _publisher;
        public string Publisher
        {
            get { return _publisher; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "Publisher", _publisher, value);
                _publisher = value;
                OnPropertyChanged("Publisher");
            }
        }

        public void EvaluatedChanged(object sender, PropertyChangedEventArgs args)
        {
            {
                switch (args.PropertyName)
                {
                    case "Name":
                        //reevaluate
                        Name = ((PackageViewModel) sender).Name;
                       
                        break;
                    case "Version":
                        Version = ((PackageViewModel)sender).Version;
                        break;
                    case "DisplayName":
                        DisplayName = ((PackageViewModel)sender).DisplayName;
                        break;
                    case "Location":
                        Location = ((PackageViewModel)sender).Location;
                        break;
                    case "Feed":
                        Feed = ((PackageViewModel)sender).Feed;
                        break;
                    case "Architecture":
                        Architecture = ((PackageViewModel)sender).Architecture;
                        break;
                    case "Publisher":
                        Publisher = ((PackageViewModel)sender).Publisher;
                        break;

                }

            }
        }

        private MainWindowViewModel _Root;
        public MainWindowViewModel Root
        {
            get { return _Root; }
            set
            {
                if (value == _Root)
                    return;

                // This line will log the property change with the undo framework.
                DefaultChangeFactory.OnChanging(this, "Root", _Root, value);

                _Root = value;
                OnPropertyChanged("Root");
            }
        }

        #region ISupportsUndo Members

        public object GetUndoRoot()
        {
            return this.Root;
        }

        #endregion
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
