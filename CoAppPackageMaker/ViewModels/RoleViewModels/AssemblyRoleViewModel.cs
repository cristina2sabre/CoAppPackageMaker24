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

         private  ObservableCollection<string> _setOfFiles;


         public AssemblyRoleViewModel(string assemblyName)
         {

         }

        // These files will be stored in Side-by-Side as a versioned assembly named <AssemblyName>
        public ObservableCollection<string> SetOfFiles
        {
            get { return _setOfFiles; }
            set
            {
                _setOfFiles = value;
                OnPropertyChanged("SetOfFiles");
            }
        }
    }
}
