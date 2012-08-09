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
      
       public FilesViewModel(PackageReader reader)
        {
            _filesCollection = new ObservableCollection<FilesItemViewModel>();
            //create FilesItemViewModel() for each parameter
            foreach (string parameter in reader.ReadParameters("files"))
            {
                var includeCollection = new ObservableCollection<BaseItemViewModel>(reader.GetRulesByParamater(parameter, "include","files", typeof(FileItem)));
                var model = new FilesItemViewModel()
                {
                    EditCollectionViewModel = new EditCollectionViewModel(includeCollection, "include", "files", typeof(FileItem)),
                    Parameter = parameter,
                    FilesRoot = reader.GetFilesRulesPropertyValueByParameterAndName(parameter, "root"),
                    TrimPath = reader.GetFilesRulesPropertyValueByParameterAndName(parameter, "trim-path"),
                };

                _filesCollection.Add(model);
            }
           
            SourceString = reader.GetRulesSourceStringPropertyValueByName("files");
            _filesCollection.CollectionChanged += FilesCollectionCollectionChanged ;
        }


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

        //needed for undo/redo
        void FilesCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
          DefaultChangeFactory.OnCollectionChanged(this, "FilesCollection", FilesCollection, e);
          OnPropertyChanged("FilesCollection");
        }
        
       
        //used for delete
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
            return this.SelectedFile != null;

        }
        
        public ICommand AddCommand
        {
            get { return new RelayCommand(Add, CanAdd); }
        }

        bool CanAdd()
        {
            return this.FilesCollection != null;

        }

        public void Add()
        {

            this.FilesCollection.Add(new FilesItemViewModel()
                                         {
                                             EditCollectionViewModel =
                                                 new EditCollectionViewModel(
                                                 new ObservableCollection<BaseItemViewModel>(), "include", "files", typeof(FileItem))
                                         });
        }

        #endregion
    }

    /// <summary>
    /// FilesItem have 3 components:root,trimpath, include.
    /// Parameter is: Files[parameter]
    /// </summary>
    public class FilesItemViewModel : ExtraPropertiesForCollectionsViewModelBase
    {
        private string _filesRoot = String.Empty;
        public string FilesRoot
        {
            get { return _filesRoot; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "FilesRoot", _filesRoot, value);
                _filesRoot = MainWindowViewModel.Instance.Reader.SetFiles(this.Parameter, "root", value);
                OnPropertyChanged("FilesRoot");
            }
        }


        private string _trimPath;
        public string TrimPath
        {
            get { return _trimPath; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "TrimPath", _trimPath, value);
                _trimPath = MainWindowViewModel.Instance.Reader.SetFiles(this.Parameter, "trim-path", value);
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

        private string _parameter = "[]";
        public string Parameter
        {
            get { return _parameter; }
            set
            {
                if (value != null)
                {
                    MainWindowViewModel.Instance.Reader.SetNewParameter("files", _parameter, value);
                    UpdateParameterForEveryItemInTheCollection(value, EditCollectionViewModel.EditableItems);
                    DefaultChangeFactory.OnChanging(this,"Parameter", _parameter, value);
                    _parameter = value;

                }
                OnPropertyChanged("Parameter");

            }
        }
    }

    /// <summary>
    /// Used in Include collection 
    /// </summary>
    public class FileItem : BaseItemViewModel
    {
        public FileItem(string ruleName, string collectionName)
        {
            RuleNameToDisplay = ruleName;
            CollectionName = collectionName;
            
        }
        public override string ProcessSourceValue(string newValue, string oldValue)
        {
            return MainWindowViewModel.Instance.Reader.SetRulesWithParameters("files", this.Parameter, this.CollectionName,oldValue, newValue);
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
