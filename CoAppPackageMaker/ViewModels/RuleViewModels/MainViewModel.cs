using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CoAppPackageMaker.ViewModels.RuleViewModels;
using MonitoredUndo;



namespace CoAppPackageMaker.ViewModels.Base
{
    public class MainWindowViewModel : ViewModelBase, ISupportsUndo
    {
        //http://msdn.microsoft.com/en-us/library/ff650316.aspx
        private readonly static MainWindowViewModel _instance = new MainWindowViewModel();
        public static MainWindowViewModel Instance
        {
            get { return _instance; }

        }

        private MainWindowViewModel()
        {
            PathToFile = "D:\\P\\glib2\\COPKG\\glib.autopkg";
            if (PathToFile != null && File.Exists(PathToFile))
            {
                //_reader = new PackageReader();
                //_reader.Read(PathToFile);
                //this._reader.Save("D:\\P\\COPKG\\test2.autopkg");
                //LoadData();
              _errorsCollection.CollectionChanged += (ErrorsCollection_CollectionChanged);
            }
        }

        void ErrorsCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            HeaderColor = ErrorsCollection.Count != 0 ? Colors.Red : Colors.Green;
            //if Erroor count is 0 1*0.3=initial opacity
            HeaderOpacity = 1*(ErrorsCollection.Count*0.05+0.3);
        }

        private double _headerOpacity = 0.3;
        public double HeaderOpacity
        {
            get { return _headerOpacity; }
            set
            {
                _headerOpacity = value;
                OnPropertyChanged("HeaderOpacity");
            }
        }

        
        private Color _headerColor=Colors.Green;
        public Color HeaderColor
        {
            get { return _headerColor; }
            set
            {
                _headerColor = value;
                OnPropertyChanged("HeaderColor");
            }
        }

        

        private string _ruleNameSelectedItem;
        private ObservableCollection<string> _ruleNames;
        private ObservableCollection<string> _roleNames;

        private PackageViewModel _packageViewModel;
        private MetadataViewModel _metadataViewModel;
        private RequiresViewModel _requiresViewModel;
        private PackageCompositionViewModel _packageCompositionViewModel;
        private FilesViewModel _filesViewModel;
        private ManifestViewModel _manifestViewModel;
        private ObservableCollection<ExtraPropertiesForCollectionsViewModelBase> _allViewModels = new ObservableCollection<ExtraPropertiesForCollectionsViewModelBase>();
        private PackageReader _reader;

        public PackageReader Reader
        {
            get { return _reader; }
        }

        public void SearchForAllUsings(string definePropertyName)
        {

            ////////////////
            var allProp = AllViewModels.SelectMany(item => item.Search(definePropertyName));
            foreach (Tuple<string, string> tuple in allProp)
            {
               ErrorsCollection.Add(new Warning() { ErrorHeader = definePropertyName, ErrorDetails = String.Format("{0} is used in {1} rule for {2}", definePropertyName, tuple.Item1, tuple.Item2) });
            }
       //a propety have been changed, for ex rachitecture in Pack- to remove the warning
           RefreshAllBindings();
        }

        public void RefreshAllBindings()
        {
            foreach (ExtraPropertiesForCollectionsViewModelBase viewModel in AllViewModels)
            {
                foreach (var property in viewModel.GetType().GetProperties())
                {
                    viewModel.OnPropertyChanged(property.Name);
                }

            }
        }

        public void RemoveError(string errorHeader)
        {
            //var newErrorCollection = ErrorsCollection.Where(item => item.ErrorHeader != errorHeader);
            //ErrorsCollection = new ObservableCollection<Error>(newErrorCollection);
            var copy = new ObservableCollection<Error>(ErrorsCollection);
            foreach (var item in copy)
            {
                if (item.ErrorHeader == errorHeader)
                {
                    ErrorsCollection.Remove(item);
                }
            }
        }

        public ObservableCollection<ExtraPropertiesForCollectionsViewModelBase> AllViewModels
        {
            get { return _allViewModels; }
            set
            {
                _allViewModels = value;
                OnPropertyChanged("AllViewModels");
            }
        }




        public void LoadData()
        {
            _reader = new PackageReader();
            _reader.Read(PathToFile); 
            AllViewModels=new ObservableCollection<ExtraPropertiesForCollectionsViewModelBase>();
           //this._reader.Save("D:\\P\\COPKG\\testsave.autopkg");
            if (_reader.SusscesfullRead)
                try
                {
                    PackageViewModel = new PackageViewModel(_reader);
                    MetadataViewModel = new MetadataViewModel(_reader);
                    ManifestViewModel = new ManifestViewModel(_reader);
                    RequiresViewModel = new RequiresViewModel(_reader);
                    DefineViewModel = new DefineViewModel(_reader);
                    SigningViewModel = new SigningViewModel(_reader);
                    LicenseViewModel = new LicenseViewModel(_reader);
                    CompatibilityPolicy = new CompatibilityPolicyViewModel(_reader);
                    ApplicationRoleViewModel = new ApplicationRoleViewModel(_reader);
                    AssemblyRoleViewModel = new AssemblyRoleViewModel(_reader);
                    FilesViewModel = new FilesViewModel(_reader);
                    DeveloperLibraryViewModel = new DeveloperLibraryViewModel(_reader);
                    //PackageCompositionViewModel = new PackageCompositionViewModel(_reader);
                    AllViewModels.Add(PackageViewModel);
                    AllViewModels.Add(LicenseViewModel);
                    AllViewModels.Add(MetadataViewModel);
                    AllViewModels.Add(ManifestViewModel);
                    AllViewModels.Add(SigningViewModel);
                    AllViewModels.Add(RequiresViewModel);
                    AllViewModels.Add(DefineViewModel);
                    AllViewModels.Add(CompatibilityPolicy);
                    AllViewModels.Add(FilesViewModel);
                    AllViewModels.Add(DeveloperLibraryViewModel);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }

        }

        private string _pathToFile;
        public string PathToFile
        {
            get { return _pathToFile; }
            set
            {
                _pathToFile = value;
                OnPropertyChanged("PathToFile");
            }
        }

        private ObservableCollection<Error> _errorsCollection = new ObservableCollection<Error>();
        public ObservableCollection<Error> ErrorsCollection
        {
            get { return _errorsCollection; }
            set
            {
                _errorsCollection = value;
                OnPropertyChanged("ErrorsCollection");
            }
        }

        #region ViewModels


        public PackageViewModel PackageViewModel
        {
            get
            {
                return _packageViewModel;
            }
            set
            {
                _packageViewModel = value;
                OnPropertyChanged("PackageViewModel");
            }
        }


        public MetadataViewModel MetadataViewModel
        {
            get
            {
                return _metadataViewModel;
            }
            set
            {
                _metadataViewModel = value;
                OnPropertyChanged("MetadataViewModel");
            }
        }

        public RequiresViewModel RequiresViewModel
        {
            get
            {
                return _requiresViewModel;
            }
            set
            {
                _requiresViewModel = value;
                OnPropertyChanged("RequiresViewModel");
            }
        }

        public PackageCompositionViewModel PackageCompositionViewModel
        {
            get
            {
                return _packageCompositionViewModel;
            }
            set
            {
                _packageCompositionViewModel = value;
                OnPropertyChanged("PackageCompositionViewModel");
            }
        }

        public FilesViewModel FilesViewModel
        {
            get
            {
                return _filesViewModel;
            }
            set
            {
                _filesViewModel = value;
                OnPropertyChanged("FilesViewModel");
            }
        }

        public DeveloperLibraryViewModel DeveloperLibraryViewModel
        {
            get
            {
                return _developerLibraryViewModel;
            }
            set
            {
                _developerLibraryViewModel = value;
                OnPropertyChanged("DeveloperLibraryViewModel");
            }
        }

        public ManifestViewModel ManifestViewModel
        {
            get
            {
                return _manifestViewModel;
            }
            set
            {
                _manifestViewModel = value;
                OnPropertyChanged("ManifestViewModel");
            }
        }

        private SigningViewModel _signingViewModel;
        public SigningViewModel SigningViewModel
        {
            get
            {
                return _signingViewModel;
            }
            set
            {
                _signingViewModel = value;
                OnPropertyChanged("SigningViewModel");
            }
        }

        private DefineViewModel _defineViewModel;

        public DefineViewModel DefineViewModel
        {
            get { return _defineViewModel; }
            set
            {
                _defineViewModel = value;
                OnPropertyChanged("DefineViewModel");
            }
        }

        private LicenseViewModel _licenseViewModel;

        public LicenseViewModel LicenseViewModel
        {
            get { return _licenseViewModel; }
            set
            {
                _licenseViewModel = value;
                OnPropertyChanged("LicenseViewModel");
            }
        }


        private CompatibilityPolicyViewModel _compatibilityPolicy;

        public CompatibilityPolicyViewModel CompatibilityPolicy
        {
            get { return _compatibilityPolicy; }
            set
            {
                _compatibilityPolicy = value;
                OnPropertyChanged("CompatibilityPolicy");
            }
        }

        private ApplicationRoleViewModel _applicationRoleViewModel;

        public ApplicationRoleViewModel ApplicationRoleViewModel
        {
            get { return _applicationRoleViewModel; }
            set
            {
                _applicationRoleViewModel = value;
                OnPropertyChanged("ApplicationRoleViewModel");
            }
        }

        private AssemblyRoleViewModel _assemblyRoleViewModel;

        public AssemblyRoleViewModel AssemblyRoleViewModel
        {
            get { return _assemblyRoleViewModel; }
            set
            {
                _assemblyRoleViewModel = value;
                OnPropertyChanged("AssemblyRoleViewModel");
            }
        }


        private EditCollectionViewModel _editCollectionViewModel;

        public EditCollectionViewModel EditCollectionViewModel
        {
            get { return _editCollectionViewModel; }
            set
            {
                _editCollectionViewModel = value;
                OnPropertyChanged(" EditCollectionViewModel");
            }
        }
        #endregion ViewModels


        public ObservableCollection<string> RulesNames
        {
            get { return _ruleNames; }
            set
            {
                _ruleNames = value;
                OnPropertyChanged("RulesNames");
            }
        }

        public ObservableCollection<string> RolesNames
        {
            get { return _roleNames; }
            set
            {
                _roleNames = value;
                OnPropertyChanged("RolesNames");
            }
        }

        //used for binding between Rule and it's properties and values
        public String RuleNameSelectedItem
        {
            get { return _ruleNameSelectedItem; }
            set
            {
                _ruleNameSelectedItem = value;
                OnPropertyChanged("RuleNameSelectedItem");
            }
        }




        #region Commands

        public ICommand DeleteCommand
        {
            get { return new RelayCommand(DeleteExecute, CanDeleteExecute); }
        }

        public ICommand SaveCommand
        {
            get { return new RelayCommand(SaveExecute, CanSaveExecute); }
        }

        public ICommand ResetCommand
        {
            get { return new RelayCommand(ResetExecute, CanResetExecute); }
        }

        public ICommand NewCommand
        {
            get { return new RelayCommand(NewExecute, CanNewExecute); }
        }





        #endregion


        #region Methods

        void DeleteExecute()
        {
            if (CanDeleteExecute())
            {
                try
                {
                    _ruleNames.Remove(this._ruleNameSelectedItem);

                    // ResetForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        bool CanDeleteExecute()
        {
            return this.RuleNameSelectedItem != null;

        }

      public  void ResetExecute()
        {
            if (CanResetExecute())
            {
                try
                {
                    this.ErrorsCollection=new ObservableCollection<Error>();
                    LoadData();
                    UndoService.Current[this].Clear();
                    HeaderColor = Colors.Green;
                    // ResetForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        bool CanResetExecute()
        {
            return
                    UndoService.Current[this].CanUndo;

        }




        void SaveExecute()
        {
            if (CanResetExecute())
            {
                try
                {
                    this._reader.Save("D:\\P\\COPKG\\testsave.autopkg");
                    // ResetForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        bool CanSaveExecute()
        {
            return
                    UndoService.Current[this].CanUndo;

        }

        void NewExecute()
        {
            if (CanNewExecute())
            {
                try
                {
                    _ruleNames.Add(this._ruleNameSelectedItem);

                    // ResetForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        bool CanNewExecute()
        {
            //to add if rule properties have correct values
            return this.RuleNameSelectedItem != null;
        }

        #endregion

        #region ISupportsUndo Members

        public object GetUndoRoot()
        {
            return MainWindowViewModel.Instance;
        }

        #endregion




        public ViewModels.DeveloperLibraryViewModel _developerLibraryViewModel { get; set; }
    }
}