using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels
{
    public class PackageViewModel : ExtraPropertiesViewModelBase
    {
        private readonly PackageReader _reader;
        private const string Package = "package";
  
        public PackageViewModel()
        {
            
        }

        public PackageViewModel(PackageReader reader)
        {
            _reader = reader;
            ValuePackageViewModel=new PackageViewModel()
                                     {
       
            Name = reader.GetRulesPropertyValueByName(Package, "name"),
            DisplayName = reader.GetRulesPropertyValueByName(Package, "display-name"),
            Architecture = reader.GetRulesPropertyValueByName(Package, "arch"),
            Feed = reader.GetRulesPropertyValueByName(Package, "feed"),
            Location = reader.GetRulesPropertyValueByName(Package, "location"),
            Publisher = reader.GetRulesPropertyValueByName(Package, "publisher"),
            Version = reader.GetRulesPropertyValueByName(Package, "version"),
            SourceString = _reader.GetRulesSourceStringPropertyValueByName(Package),
            IsReadOnly = true,
                                     };

            Name = reader.GetRulesSourcePropertyValueByName(Package, "name");
            DisplayName = reader.GetRulesSourcePropertyValueByName(Package, "display-name");
            Architecture = reader.GetRulesSourcePropertyValueByName(Package, "arch");
            Feed = reader.GetRulesSourcePropertyValueByName(Package, "feed");
            Location = reader.GetRulesSourcePropertyValueByName(Package, "location");
            Publisher = reader.GetRulesSourcePropertyValueByName(Package, "publisher");
            Version = reader.GetRulesSourcePropertyValueByName(Package, "version");
            IsSource =true;
            IsReadOnly = false;
            RuleNameToDisplay = "Package";

            
          this.PropertyChanged += EvaluatedChanged;
          
        }

        private PackageViewModel _valuePackageViewModel;
        public PackageViewModel ValuePackageViewModel
        {
            get { return _valuePackageViewModel; }
            set
            {
                _valuePackageViewModel = value;
                OnPropertyChanged("ValuePackageViewModel");
            }
        }

        #region Properties
       

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

        #endregion

        public void EvaluatedChanged(object sender, PropertyChangedEventArgs args)
        {
            
               
                switch (args.PropertyName)
                {
                    case "Name":
                        //reevaluate
                      
                        this.ValuePackageViewModel.Name = _reader.SetNewSourceValue(Package, "name", Name);
                        break;
                    case "Version":
                        this.ValuePackageViewModel.Version = _reader.SetNewSourceValue(Package, "version", Version);
                        break;
                    case "DisplayName":
                        this.ValuePackageViewModel.DisplayName = _reader.SetNewSourceValue(Package, "display-name", DisplayName);
                        break;
                    case "Location":
                        this.ValuePackageViewModel.Location = _reader.SetNewSourceValue(Package, "location", Location);
                        break;
                    case "Feed":
                        this.ValuePackageViewModel.Feed = _reader.SetNewSourceValue(Package, "feed", Feed);
                        break;
                    case "Architecture":
                        this.ValuePackageViewModel.Architecture = _reader.SetNewSourceValue(Package, "arch", Architecture);
                        break;
                    case "Publisher":
                        this.ValuePackageViewModel.Publisher = _reader.SetNewSourceValue(Package, "publisher", Publisher);
                        break;
                }

                this.ValuePackageViewModel.SourceString = _reader.GetRulesSourceStringPropertyValueByName(Package);
             
            }
        
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
