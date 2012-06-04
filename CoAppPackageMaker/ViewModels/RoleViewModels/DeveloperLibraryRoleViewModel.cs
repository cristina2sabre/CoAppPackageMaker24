using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoAppPackageMaker.ViewModels
{
    //Typically a package with this role will either include an assembly role containing the related end-user library or will list the package containing it in a requires rule.
    //This role handles installing headers, link-libraries, and developer docs for a library.
    //While it is possible to have both an 'assembly' and a 'developer-library' role declared in the same package,
    //doing so is considered very bad form, as it means the developer libraries will always be included when an end-user just needs to use the dll
    class DeveloperLibraryRoleViewModel:ExtraPropertiesViewModelBase
    {

         private  ObservableCollection<string> _libraries;
         private ObservableCollection<string> _headers;
         private ObservableCollection<string> _docs;


         public DeveloperLibraryRoleViewModel(string libraryName)
        { 
        
        }

       
        public ObservableCollection<string> Libraries
        {
            // These will automatically have links made in %AllUsersProfile%\lib\${package.arch}\
            get { return _libraries; }
            set
            {
                _libraries = value;
                OnPropertyChanged("Libraries");
            }
        }

        // These will automatically have links made in %AllUsersProfile%\include\<LibName>\
        public ObservableCollection<string> Headers
        {
            get { return _headers; }
            set
            {
                _headers = value;
                OnPropertyChanged("Headers");
            }
        }

        public ObservableCollection<string> Docs
        {
            get { return _docs; }
            set
            {
                _docs = value;
                OnPropertyChanged("Docs");
            }
        }
    }
}
