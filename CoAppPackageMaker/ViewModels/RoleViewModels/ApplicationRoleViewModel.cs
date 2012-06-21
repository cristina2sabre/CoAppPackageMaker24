using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

//This role is used to package an executable application.
//Frequently an application package will depend upon many assembly packages, which must be listed in a requires rule and referenced by way of manifest rules.
namespace CoAppPackageMaker.ViewModels
{
    public class ApplicationRoleViewModel:ExtraPropertiesViewModelBase
    {
        //with paramater name?
        public ApplicationRoleViewModel(PackageReader reader)
            {
                _include = new ObservableCollection<string>(reader.GetRulesPropertyValues("application", "include"));
              
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
