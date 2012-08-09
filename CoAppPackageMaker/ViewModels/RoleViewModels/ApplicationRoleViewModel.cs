﻿using System;
using System.Collections.ObjectModel;
﻿using System.Drawing;
﻿using System.Windows.Input;
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
            _roleCollection = new ObservableCollection<RoleItemViewModel>();

            foreach (string parameter in reader.ReadParameters("application"))
            {
                var includeCollection = new ObservableCollection<BaseItemViewModel>(reader.GetRulesByParamater(parameter, "include", "application", typeof(ApplicationItem)));
                var model = new RoleItemViewModel()
                {
                    RuleNameToDisplay = "application",
                    EditCollectionViewModel =
                        new EditCollectionViewModel(includeCollection, "include", "application", typeof(ApplicationItem)),
                    Parameter = parameter,
                    
                };
                _roleCollection.Add(model);
            }

            SourceString = reader.GetRulesSourceStringPropertyValueByName("application");

           
              
            _roleCollection.CollectionChanged += RolesCollectionChanged;
        }

        protected ApplicationRoleViewModel()
        {

        }

        public void RolesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DefaultChangeFactory.OnCollectionChanged(this, "RoleCollection", RoleCollection, e);
            OnPropertyChanged("RoleCollection");
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



        private ObservableCollection<RoleItemViewModel> _roleCollection;
        public ObservableCollection<RoleItemViewModel> RoleCollection
        {
            get { return _roleCollection; }
            set
            {
                _roleCollection = value;
                OnPropertyChanged("RoleCollection");
            }
        }
        

        #region Event Handlers

        public ICommand RemoveCommand
        {
            get { return new RelayCommand(Remove, CanRemove); }
        }

        public void Remove()
        {
            this.RoleCollection.Remove(this.SelectedFile);

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
            return this.RoleCollection != null;

        }

        public virtual void Add()
        {

            this.RoleCollection.Add(new RoleItemViewModel() {RuleNameToDisplay  = "application",EditCollectionViewModel = new EditCollectionViewModel( new ObservableCollection<BaseItemViewModel>(), "include", "application", typeof(ApplicationItem)) });
        }

        #endregion
    }

    public class RoleItemViewModel : ExtraPropertiesForCollectionsViewModelBase
    {
       

        private string _parameter="[]";
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
        public ApplicationItem(string ruleName, string collectionName)
        {
            RuleNameToDisplay = ruleName;
            CollectionName = collectionName;
            
        }
        
       public override string ProcessSourceValue(string newValue,string oldValue)
        {
            return MainWindowViewModel.Instance.Reader.SetRulesWithParameters(this.RuleNameToDisplay, this.Parameter, this.CollectionName, oldValue, newValue);
        }
    }
}