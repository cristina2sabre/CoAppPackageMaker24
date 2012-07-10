using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

using CoApp.Packaging;
using CoApp.Packaging.Client;
using CoAppPackageMaker.ViewModels.RuleViewModels;
using MonitoredUndo;


namespace CoAppPackageMaker.ViewModels.Base
{
  public  class MainWindowViewModel:ViewModelBase, ISupportsUndo
    {
        private string _ruleNameSelectedItem;
        private ObservableCollection<string> _ruleNames;
        private ObservableCollection<string> _roleNames;
       
        private PackageViewModel _packageViewModel;
        private MetadataViewModel _metadataViewModel;
        private RequiresViewModel _requiresViewModel;
        private PackageCompositionViewModel _packageCompositionViewModel;
        private FilesViewModel _filesViewModel;
        private ManifestViewModel _manifestViewModel;
        private ObservableCollection<ExtraPropertiesViewModelBase> _allViewModels =new ObservableCollection<ExtraPropertiesViewModelBase>();
      private PackageReader _reader;
        public ObservableCollection<ExtraPropertiesViewModelBase> AllViewModels
        {
            get { return _allViewModels; }
            set
            {
                _allViewModels = value;
                OnPropertyChanged("AllViewModels");
            }
        }


        public MainWindowViewModel()
        {
            //  PathToFile = "D:\\P\\glib\\COPKG\\glib.autopkg";
            //  PackageReader reader = new PackageReader();
            //  reader.Read(PathToFile);

            //  _ruleNames = reader.Rules;
            //  Dictionary<string, IFactory> dic = new Dictionary<string, IFactory>();
            //  dic.Add("package", new PackageViewModelFactory());
            //  dic.Add("files",new FilesViewModelFactory());
            //  foreach (string str in _ruleNames)
            //  {
            //      if (dic.ContainsKey(str))
            //      {
            //          object instance = (dic[str].CreateInstance(reader));
            //          string name = instance.GetType().Name;
            //          var p = GetType().GetProperty(name);
            //          p.SetValue(this, instance, null);
            //      }

            //}

        //  PathToFile = "D:\\P\\COPKG\\test2.autopkg";
           // PathToFile = "D:\\P\\procmon\\copkg\\procmon.autopkg";
         // PathToFile = "D:\\P\\glib\\COPKG\\glib.autopkg";
            PathToFile = @"C:\Users\Eric\Desktop\glib\COPKG\glib.autopkg";
            if (PathToFile != null && File.Exists(PathToFile))
            {
                LoadData();
            }


        }

        private void LoadData()
        {
            
           _reader = new PackageReader();
           _reader.Read(PathToFile);

           PackageViewModel = new PackageViewModel(_reader);
             //{ Root = this }
            PackageViewModel.SourcePackageViewModel.Root = this;
            MetadataViewModel = new MetadataViewModel(_reader);
            MetadataViewModel.SourceMetadataViewModel.Root = this;
            ManifestViewModel = new ManifestViewModel(_reader, this);
            ManifestViewModel.SourceManifestViewModel.Root = this;
            SigningViewModel = new SigningViewModel(_reader, this);
            SigningViewModel.SourceSigningViewModel.Root = this;
            RequiresViewModel = new RequiresViewModel(_reader, this);
            DefineViewModel = new DefineViewModel(_reader, this);
           // DefineViewModel.SourceDefineViewModel.Root = this;


            LicenseViewModel = new LicenseViewModel(_reader);
            LicenseViewModel.SourceValueLicenseViewModel.Root = this;
            CompatibilityPolicy = new CompatibilityPolicyViewModel(_reader);
            CompatibilityPolicy.SourceValueCompatibilityPolicyViewModel.Root = this;
            ApplicationRoleViewModel = new ApplicationRoleViewModel(_reader, this);
            AssemblyRoleViewModel = new AssemblyRoleViewModel(_reader, this);
            PackageCompositionViewModel = new PackageCompositionViewModel(_reader);
            FilesViewModel = new FilesViewModel(_reader, this);
            //_editCollectionViewModel=new EditCollectionViewModel(reader,this,RequiresViewModel);

            _allViewModels.Add(_packageViewModel);

        }

        private  string _pathToFile;
        public string PathToFile
        {
            get { return _pathToFile; }
            set
            {
                _pathToFile = value;
                OnPropertyChanged("PathToFile");
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
             if (CanDeleteExecute() )
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
          return this.RuleNameSelectedItem!=null;
            
         }

        void ResetExecute()
        {
            if (CanResetExecute())
            {
                try
                {
                    UndoService.Current.Clear();
                    LoadData();
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
                    this._reader.Save("D:\\P\\COPKG\\test2.autopkg");
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
             return this;
         }

         #endregion


       
    }
}
