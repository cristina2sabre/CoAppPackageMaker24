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
                var includeCollection = new ObservableCollection<BaseItemViewModel>(reader.GetRulesByParamater(parameter, "include", "application", typeof(ApplicationItem)));
                var model = new RoleItemViewModel()
                {
                    //RoleName = "Application",
                    RuleNameToDisplay = "application",
                    EditCollectionViewModel =
                        new EditCollectionViewModel( includeCollection,typeof(ApplicationItem)),
                    Parameter = parameter,
                    
                };
                _applicationCollection.Add(model);
            }

            SourceString = reader.GetRulesSourceStringPropertyValueByName("application");
            _applicationCollection.CollectionChanged += RolesCollectionChanged;
        }

        protected ApplicationRoleViewModel()
        {

        }

        public void RolesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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

            this.ApplicationCollection.Add(new RoleItemViewModel() {RuleNameToDisplay  = "application",EditCollectionViewModel = new EditCollectionViewModel( new ObservableCollection<BaseItemViewModel>(),typeof(ApplicationItem)) });
        }

        #endregion
    }

    public class RoleItemViewModel : ExtraPropertiesForCollectionsViewModelBase
    {
       

        private string _parameter;
        public string Parameter
        {
            get { return _parameter; }
            set
            {

                if (value != null)
                {
                    MainWindowViewModel.Instance.Reader.SetNewParameter(this.RuleNameToDisplay, _parameter, value);
                    UpdateParameterForEveryItemInTheCollection(value, this.EditCollectionViewModel.EditableItems);

                    DefaultChangeFactory.OnChanging(this, "Parameter", _parameter, value);
                    _parameter = value;
                }

                OnPropertyChanged("Parameter");

            }
        }

    }

    public class ApplicationItem : BaseItemViewModel
    {
        
       public override string ProcessSourceValue(string newValue,string oldValue)
        {
            //string ruleName, int index, string parameter, string colectionName, string newValue
            return MainWindowViewModel.Instance.Reader.SetRulesWithParameters(this.RuleNameToDisplay, this.Parameter, this.CollectionName, oldValue, newValue);
        }
    }
}