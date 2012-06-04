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
        private ObservableCollection<Package> _requiredPackages;

        public ObservableCollection<Package> RequiredPackages
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
