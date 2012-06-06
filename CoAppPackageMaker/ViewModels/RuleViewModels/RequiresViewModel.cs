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

        public RequiresViewModel (PackageReader reader)
        {
            _requiredPackages = new ObservableCollection<string>(reader.GetRulesPropertyValues("requires", "package"));
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
    }
}
