using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CoAppPackageMaker.ViewModels.Base;
using CoAppPackageMaker.ViewModels.RuleViewModels;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels
{
     public class ManifestViewModel : ExtraPropertiesViewModelBase
    {

        private ObservableCollection<ManifestItemViewModel> _manifestCollection;
        public ObservableCollection<ManifestItemViewModel> ManifestCollection
        {
            get { return _manifestCollection; }
            set
            {
                _manifestCollection = value;
                OnPropertyChanged("ManifestCollection");
            }
        }

       
        public ManifestViewModel()
        {
        }

        public ManifestViewModel(PackageReader reader, MainWindowViewModel root)
        {
            Root = root;
            _manifestCollection = new ObservableCollection<ManifestItemViewModel>();

            foreach (string parameter in reader.ReadManifestParameters())
            {
                ObservableCollection<ItemViewModel> includeCollection = new ObservableCollection<ItemViewModel>(reader.ManifestIncludeList(parameter, "include", root));
               // ObservableCollection<ItemViewModel> assemblyCollection = new ObservableCollection<ItemViewModel>(reader.GetManifestIncludeList(parameter, "assembly"));
                //ObservableCollection<string> includeCollection = new ObservableCollection<string>(reader.GetManifestIncludeList(parameter, "include"));
                //ObservableCollection<string> assemblyCollection = new ObservableCollection<string>(reader.GetManifestIncludeList(parameter, "assembly"));
                ManifestItemViewModel model = new ManifestItemViewModel()
                {
                    EditCollectionViewModel = new EditCollectionViewModel(reader, root, includeCollection),
                    Name = parameter,
                    Root = root,
              
                };
                _manifestCollection.Add(model);
            }

            {
                _sourceManifestViewModel = new ManifestViewModel();
                SourceManifestViewModel._manifestCollection = new ObservableCollection<ManifestItemViewModel>();

                foreach (string parameter in reader.ReadManifestParameters())
                {
                  
                    ObservableCollection<string> includeCollection = new ObservableCollection<string>(reader.GetManifestIncludeList2("manifest", "include"));
                    ObservableCollection<string> assemblyCollection = new ObservableCollection<string>(reader.GetManifestIncludeList2("manifest", "assembly"));
                    ManifestItemViewModel model = new ManifestItemViewModel()
                    {
                        
                  
                    };
                    SourceManifestViewModel._manifestCollection.Add(model);
                }

                
            }

          
           
           
             SourceString = reader.GetRulesSourceStringPropertyValueByName("manifest");
             _manifestCollection.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(FilesCollectionCollectionChanged);

        }
        void FilesCollectionCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DefaultChangeFactory.OnCollectionChanged(this, "ManifestCollection", ManifestCollection, e);
            OnPropertyChanged("ManifestCollection");
        }

        private ManifestViewModel _sourceManifestViewModel;
        public ManifestViewModel SourceManifestViewModel
        {
            get { return _sourceManifestViewModel; }
            set
            {
                _sourceManifestViewModel = value;
                OnPropertyChanged("SourceManifestViewModel");
            }
        }

        public class ManifestItemViewModel : ExtraPropertiesViewModelBase
        {
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

        }


        public ManifestItemViewModel _selectedFile;
        public ManifestItemViewModel SelectedFile
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
            this.ManifestCollection.Remove(this.SelectedFile);

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
            return this.ManifestCollection != null;

        }
        public void Add()
        {

            this.ManifestCollection.Add(new ManifestItemViewModel() { Root = this.Root ,EditCollectionViewModel = new EditCollectionViewModel(null,Root,new ObservableCollection<ItemViewModel>())});
        }

        #endregion
    }
}
