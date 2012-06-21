using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoAppPackageMaker.ViewModels.RuleViewModels

{
   

    public class FilesViewModel:ExtraPropertiesViewModelBase
    {
        private ObservableCollection<FilesItemViewModel> _filesCollection;
        public ObservableCollection<FilesItemViewModel> FilesCollection
        {
            get { return _filesCollection; }
            set
            {
                _filesCollection = value;
                OnPropertyChanged("FilesCollection");
            }
        }
        
        public FilesViewModel(PackageReader reader)
        {
            _filesCollection = new ObservableCollection<FilesItemViewModel>();

            foreach (string parameter in reader.ReadFilesParameters())
            {

                ObservableCollection<string> collection = new ObservableCollection<string>(reader.GetFilesIncludeList(parameter));

                FilesItemViewModel model = new FilesItemViewModel(parameter)
                {
                    Root = reader.GetFilesRulesPropertyValueByParameterAndName(parameter, "root"),
                    TrimPath = reader.GetFilesRulesPropertyValueByParameterAndName(parameter, "trim-path"),
                    Include = collection
                };
                _filesCollection.Add(model);
            }
           
        }


        public class  FilesItemViewModel:ViewModelBase
        {
            public FilesItemViewModel(string parameter)
             {
                 Name = String.Format("Files[{0}]", parameter);
             }




             private string _root;

             public string Root
             {
                 get { return _root; }
                 set
                 {
                     _root = value;
                     OnPropertyChanged("Root");
                 }
             }


             private string _trimPath;

             public string TrimPath
             {
                 get { return _trimPath; }
                 set
                 {
                     _trimPath = value;
                     OnPropertyChanged("TrimPath");
                 }
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

             private string _name;

             public string Name
             {
                 get { return _name; }
                 set
                 {
                     _name = value;
                     OnPropertyChanged("Name");
                 }
             }

        }
        

    }


    public class FilesViewModelFactory : IFactory
    {
        public object CreateInstance(PackageReader reader)
        {
           
            FilesViewModel model = new FilesViewModel(reader);
            
            return model;

        }
    }
}
