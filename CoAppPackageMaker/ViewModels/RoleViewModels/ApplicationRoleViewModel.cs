﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CoAppPackageMaker.ViewModels.Base;
using CoAppPackageMaker.ViewModels.RuleViewModels;
using MonitoredUndo;

//This role is used to package an executable application.
//Frequently an application package will depend upon many assembly packages, which must be listed in a requires rule and referenced by way of manifest rules.
namespace CoAppPackageMaker.ViewModels
{
    public class ApplicationRoleViewModel : ExtraPropertiesForCollectionsViewModelBase
    {
        public ApplicationRoleViewModel(PackageReader reader)
        {
            _applicationCollection = new ObservableCollection<RoleItemViewModel>();

            foreach (string parameter in reader.ReadParameters("application"))
            {
                var includeCollection = new ObservableCollection<BaseItemViewModel>(reader.GetManifestFinal(parameter, "include", "application", typeof(ApplicationItem)));
                var model = new RoleItemViewModel()
                {
                    Label = "Application",
                    EditCollectionViewModel =
                        new EditCollectionViewModel(reader, includeCollection,typeof(ApplicationItem)),
                    Name = parameter,
                };
                _applicationCollection.Add(model);
            }

            SourceString = reader.GetRulesSourceStringPropertyValueByName("application");
            _applicationCollection.CollectionChanged += FilesCollectionCollectionChanged;
        }

        public class RoleItemViewModel : ExtraPropertiesForCollectionsViewModelBase
        {
            

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

        public void FilesCollectionCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DefaultChangeFactory.OnCollectionChanged(this, "ApplicationCollection", ApplicationCollection, e);
            OnPropertyChanged("ApplicationCollection");
        }


        private RoleItemViewModel _selectedFile;
        public RoleItemViewModel SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                OnPropertyChanged("SelectedFile");
            }
        }
        private ObservableCollection<RoleItemViewModel> _applicationCollection;

        protected ApplicationRoleViewModel()
        {

        }

        public ObservableCollection<RoleItemViewModel> ApplicationCollection
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

            this.ApplicationCollection.Add(new ApplicationRoleViewModel.RoleItemViewModel() { Label = "Application", EditCollectionViewModel = new EditCollectionViewModel(null, new ObservableCollection<BaseItemViewModel>(),typeof(ApplicationItem)) });
        }

        #endregion
    }

    public class ApplicationItem : BaseItemViewModel
    {
        public override string ProcessSourceValue(string input)
        {
            //string ruleName, int index, string parameter, string colectionName, string newValue
            return this.Reader.SetManifestFinal("application", this.Index, this.Label, this.CollectionName, input);
        }
    }
}