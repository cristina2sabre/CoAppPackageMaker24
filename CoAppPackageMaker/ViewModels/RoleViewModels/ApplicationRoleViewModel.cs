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
    class ApplicationRoleViewModel:ExtraPropertiesViewModelBase
    {

        private  ObservableCollection<string> _includeFiles;
       

        public ApplicationRoleViewModel(string appName)
        { 
        
        }

        //  All files listed here will be included in the application directory.
        public ObservableCollection<string> IncludeFiles
        {
            get { return _includeFiles; }
            set
            {
                _includeFiles = value;
                OnPropertyChanged("IncludeFiles");
            }
        }

    }
}
