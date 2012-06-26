using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;

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
               //DefaultChangeFactory.OnChanging(this, "FilesCollection", _filesCollection, value);
                _filesCollection = value;
                OnPropertyChanged("FilesCollection");
            }
        }
        
        public FilesViewModel(PackageReader reader, MainWindowViewModel root)
        {
            Root = root;
            _filesCollection = new ObservableCollection<FilesItemViewModel>();
           
            foreach (string parameter in reader.ReadFilesParameters())
            {
             //  ObservableCollection<string> collection = new ObservableCollection<string>(reader.GetFilesIncludeList(parameter));
               ObservableCollection<ItemViewModel> includeCollection = new ObservableCollection<ItemViewModel>(reader.FilesIncludeList(parameter, "include", root));
                FilesItemViewModel model = new FilesItemViewModel()
                {
                    FilesRoot = reader.GetFilesRulesPropertyValueByParameterAndName(parameter, "root"),
                    TrimPath = reader.GetFilesRulesPropertyValueByParameterAndName(parameter, "trim-path"),
                    EditCollectionViewModel = new EditCollectionViewModel(reader, root, includeCollection),
                  //  Include = collection,
                    Name = parameter,
                    Root = root,
                };

                _filesCollection.Add(model);
            }
           
            SourceString = reader.GetRulesSourceStringPropertyValueByName("files");
            _filesCollection.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(FilesCollectionCollectionChanged);
        }

        void FilesCollectionCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
          DefaultChangeFactory.OnCollectionChanged(this, "FilesCollection", FilesCollection, e);
            OnPropertyChanged("FilesCollection");
        }

       

        public class   FilesItemViewModel:ExtraPropertiesViewModelBase
        {
          private string _filesRoot;
             public string FilesRoot
             {
                 get { return _filesRoot; }
                 set
                 {
                     DefaultChangeFactory.OnChanging(this, "FilesRoot", _filesRoot, value);
                     _filesRoot = value;
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
                     DefaultChangeFactory.OnChanging(this, "Include", _include, value);
                     _include = value;
                     OnPropertyChanged("Include");
                 }
             }

             private string _name="[]";
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


             public EditCollectionViewModel _editCollectionViewModel;

             public EditCollectionViewModel EditCollectionViewModel
             {
                 get { return _editCollectionViewModel; }
                 set
                 {
                     _editCollectionViewModel = value;
                     OnPropertyChanged("EditCollectionViewModel");
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
              
               this.FilesCollection.Add(new FilesItemViewModel() { Root = this.Root });
           }
    
        #endregion
    }


    public class FilesViewModelFactory : IFactory
    {
        public object CreateInstance(PackageReader reader)
        {
           
            FilesViewModel model = new FilesViewModel(reader, null);
            
            return model;

        }
    }
}
