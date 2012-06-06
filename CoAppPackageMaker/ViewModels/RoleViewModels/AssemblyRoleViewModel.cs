using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoAppPackageMaker.ViewModels
{
    //This role specifies shared libraries which are dynamically linked to by other assemblies and applications.
    class AssemblyRoleViewModel:ExtraPropertiesViewModelBase
    {
        //withparam name?
        public AssemblyRoleViewModel(PackageReader reader)
            {
                _include = new ObservableCollection<string>(reader.GetRulesPropertyValues("assembly", "include"));
            }

            private ObservableCollection<string> _include;

            public ObservableCollection<string> Include
            {
                get { return _include; }
                set
                {
                    _include = value;
                    OnPropertyChanged("Include");
                }
            }
    }
}
