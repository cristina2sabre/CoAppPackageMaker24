using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Input;
using CoAppPackageMaker.ViewModels.Base;
using CoAppPackageMaker.ViewModels.RuleViewModels;
using MonitoredUndo;

//This role is used to package an executable application.
//Frequently an application package will depend upon many assembly packages, which must be listed in a requires rule and referenced by way of manifest rules.
namespace CoAppPackageMaker.ViewModels
{
    public class ApplicationRoleViewModel:ExtraPropertiesViewModelBase
    {
        //private string _upper;

        //public string Label
        //{
        //    get { return _upper; }
           
        //}

        
        private  string Label = "Application";
        public ApplicationRoleViewModel(PackageReader reader, MainWindowViewModel root)
            {
                Root = root;
              
                _applicationCollection = new ObservableCollection<ApplicationItemViewModel>();

                foreach (string parameter in reader.ReadParameters("application"))
                {
                    ObservableCollection<ItemViewModel> includeCollection = new ObservableCollection<ItemViewModel>(reader.ApplicationIncludeList("application", parameter, "include", root));
                  
                    ApplicationItemViewModel model = new ApplicationItemViewModel()
                    {
                          Label = "Application",
                        EditCollectionViewModel = new EditCollectionViewModel(reader, root, includeCollection),
                        Name = parameter,
                        Root = root,

                    };
                    _applicationCollection.Add(model);
                }

                SourceString = reader.GetRulesSourceStringPropertyValueByName("application");
                _applicationCollection.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(FilesCollectionCollectionChanged);
            }

        public class ApplicationItemViewModel : ExtraPropertiesViewModelBase
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

            private string _label;
            public string Label
            {
                get { return _label; }
                set
                {
                    _label = value;
                    OnPropertyChanged("Label");
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

    public    void FilesCollectionCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DefaultChangeFactory.OnCollectionChanged(this, "ApplicationCollection", ApplicationCollection, e);
            OnPropertyChanged("ApplicationCollection");
        }

        public ApplicationItemViewModel _selectedFile;
        public ApplicationItemViewModel SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                OnPropertyChanged("SelectedFile");
            }
        }
        private ObservableCollection<ApplicationItemViewModel> _applicationCollection;

        protected ApplicationRoleViewModel()
        {
           
        }

        public ObservableCollection<ApplicationItemViewModel> ApplicationCollection
        {
            get { return _applicationCollection; }
            set
            {
                _applicationCollection = value;
                OnPropertyChanged("ApplicationCollection");
            }
        }

       

        
       
        #region Event Handlers



        public ICommand RemoveCommand
        {
            get { return new RelayCommand(Remove, CanRemove); }
        }

        public void Remove()
        {
            this.ApplicationCollection.Remove(this.SelectedFile);

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
            return this.ApplicationCollection != null;

        }

        public virtual void Add()
        {

            this.ApplicationCollection.Add(new ApplicationRoleViewModel.ApplicationItemViewModel() { Label = "Application", Root = this.Root, EditCollectionViewModel = new EditCollectionViewModel(null, Root, new ObservableCollection<ItemViewModel>()) });
        }

        #endregion
    }
}
