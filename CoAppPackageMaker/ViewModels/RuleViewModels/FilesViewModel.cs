using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels.RuleViewModels

{
  
    public class FilesViewModel:ExtraPropertiesForCollectionsViewModelBase
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
            foreach (string parameter in reader.ReadParameters("files"))
            {
                var includeCollection = new ObservableCollection<BaseItemViewModel>(reader.GetManifestFinal(parameter, "include","files", typeof(FileItem)));
                var model = new FilesItemViewModel()
                {
                    Parameter = parameter,
                    PackageReader = reader,
                    FilesRoot = reader.GetFilesRulesPropertyValueByParameterAndName(parameter, "root"),
                    TrimPath = reader.GetFilesRulesPropertyValueByParameterAndName(parameter, "trim-path"),
                    EditCollectionViewModel = new EditCollectionViewModel(reader, includeCollection, typeof(FileItem)),
                    Name = parameter,
                    
                };

                _filesCollection.Add(model);
            }
           
            SourceString = reader.GetRulesSourceStringPropertyValueByName("files");
            _filesCollection.CollectionChanged += FilesCollectionCollectionChanged ;
        }

        private EditCollectionViewModel _editCollectionViewModel;
        public EditCollectionViewModel EditCollectionViewModel
        {
            get { return _editCollectionViewModel; }
            set
            {
                _editCollectionViewModel = value;
                OnPropertyChanged("EditCollectionViewModel");
            }
        }
        void FilesCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
          DefaultChangeFactory.OnCollectionChanged(this, "FilesCollection", FilesCollection, e);
            OnPropertyChanged("FilesCollection");
        }
        
        public class FilesItemViewModel : ExtraPropertiesForCollectionsViewModelBase
        {
            public PackageReader PackageReader;
            private string _filesRoot;
            public string FilesRoot
            {
                get { return _filesRoot; }
                set
                {
                    DefaultChangeFactory.OnChanging(this, "FilesRoot", _filesRoot, value);
                    _filesRoot = PackageReader.SetFiles(this.Parameter, "root", value);
                    OnPropertyChanged("FilesRoot");
                }
            }


            private EditCollectionViewModel _editCollectionViewModel;
            public EditCollectionViewModel EditCollectionViewModel
            {
                get { return _editCollectionViewModel; }
                set
                {
                    _editCollectionViewModel = value;
                    OnPropertyChanged("EditCollectionViewModel");
                }
            }

            private string _trimPath;
            public string TrimPath
            {
                get { return _trimPath; }
                set
                {
                    DefaultChangeFactory.OnChanging(this, "TrimPath", _trimPath, value);
                    _trimPath = PackageReader.SetFiles(this.Parameter, "trim-path", value);
                    OnPropertyChanged("TrimPath");
                }
            }

            private ObservableCollection<string> _include;
            public ObservableCollection<string> Include
            {
                get { return _include; }
                set
                {
                    DefaultChangeFactory.OnChanging(this, "Include", _include, value);
                    _include = value;
                    OnPropertyChanged("Include");
                }
            }

            private string _name = "[]";
            public string Name
            {
                get { return _name; }
                set
                {
                    if (value != null)
                    {
                        DefaultChangeFactory.OnChanging(this, "Name", _name, value);
                        _name = value.StartsWith("[") && value.EndsWith("]") ? value : String.Format("[{0}]", value);
                    }
                    else
                    {
                        _name = String.Format("[{0}]", value);
                    }
                    OnPropertyChanged("Name");
                }
            }

            private string _paramater;
            public string Parameter
            {
                get { return _paramater; }
                set
                {
                    _paramater = value;
                    OnPropertyChanged("Parameter");
                    
                }
            }

            

           
        }

        private FilesItemViewModel _selectedFile;
        public FilesItemViewModel SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                OnPropertyChanged("SelectedFile");
            }
        }



        
        #region Event Handlers



        public ICommand RemoveCommand
        {
            get { return new RelayCommand(Remove, CanRemove); }
        }

        public void Remove()
        {
         this.FilesCollection.Remove(this.SelectedFile);
           
        }
      
           bool CanRemove()
         {
          return this.SelectedFile!=null;
            
         }
          

           public ICommand AddCommand
           {
               get { return new RelayCommand(Add, CanAdd); }
           }
           bool CanAdd()
           {
               return this.FilesCollection!= null;

           }
           public void Add()
           {

               this.FilesCollection.Add(new FilesItemViewModel() { EditCollectionViewModel = new EditCollectionViewModel(null, new ObservableCollection<BaseItemViewModel>(), typeof(FileItem)) });
           }
    
        #endregion
    }

    public class FileItem : BaseItemViewModel
    {
        public override string ProcessSourceValue(string input)
        {
            //string ruleName, int index, string parameter, string colectionName, string newValue
            return this.Reader.SetManifestFinal("files", this.Index, this.Label, this.CollectionName, input);
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
