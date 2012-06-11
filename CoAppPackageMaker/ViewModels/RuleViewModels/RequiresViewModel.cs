using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.ObjectModel;
using CoApp.Packaging.Client;

namespace CoAppPackageMaker.ViewModels
{
    class RequiresViewModel : ExtraPropertiesViewModelBase
    {
        public RequiresViewModel()
        {

        }

        public RequiresViewModel (PackageReader reader)
        {
            _requiredPackages = new ObservableCollection<string>(reader.GetRulesPropertyValues("requires", "package"));
            SourceValueRequiresViewModel = new SourceRequiresViewModel(reader);
            SourceString = reader.GetRulesSourceStringPropertyValueByName("requires");
        }

        private ObservableCollection<string> _requiredPackages;

        public ObservableCollection<string> RequiredPackages
        {
            get { return _requiredPackages; }
            set
            {
                _requiredPackages = value;
                OnPropertyChanged("RequiredPackages");
            }
        }

        private SourceRequiresViewModel _sourceValueRequiresViewModel;

        public SourceRequiresViewModel SourceValueRequiresViewModel
        {
            get { return _sourceValueRequiresViewModel; }
            set
            {
                _sourceValueRequiresViewModel = value;
                OnPropertyChanged("SourceValueRequiresViewModel");
            }
        }
        private string _sourceString;
        public string SourceString
        {
            get { return _sourceString; }
            set
            {
                _sourceString = value;
                OnPropertyChanged("SourceString");
            }
        }

        public class SourceRequiresViewModel:RequiresViewModel
        {
        public SourceRequiresViewModel(PackageReader reader)
        {
      
            _requiredPackages = new ObservableCollection<string>(reader.GetRulesSourcePropertyValuesByName("requires", "package"));
        }
        }
    }
}
