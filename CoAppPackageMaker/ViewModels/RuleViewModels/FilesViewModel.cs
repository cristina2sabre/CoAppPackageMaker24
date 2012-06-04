using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoAppPackageMaker.ViewModels.RuleViewModels

{
   

    class FilesViewModel:ExtraPropertiesViewModelBase
    {
        private ObservableCollection<FilesViewModel> _filesCollection;
        public ObservableCollection<FilesViewModel> FilesCollection
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
            _filesCollection = new ObservableCollection<FilesViewModel>();

            foreach (string parameter in reader.ReadFilesParameters())
            {

                ObservableCollection<string> collection = new ObservableCollection<string>(reader.GetFilesIncludeList(parameter));

                FilesViewModel model = new FilesViewModel(parameter)
                {
                    Root = reader.GetFilesRulesPropertyValueByParameterAndName(parameter, "root"),
                    TrimPath = reader.GetFilesRulesPropertyValueByParameterAndName(parameter, "trim-path"),
                    Include = collection
                };
                _filesCollection.Add(model);
            }
           
        }

        public FilesViewModel(String paramater)
        {
            Name = String.Format("Files[{0}]",paramater);
        }

        public FilesViewModel()
        {
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
