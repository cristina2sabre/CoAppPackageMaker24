﻿using System;
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
    public class ManifestViewModel : ExtraPropertiesForCollectionsViewModelBase
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

        public ManifestViewModel(PackageReader reader)
        {
            _manifestCollection = new ObservableCollection<ManifestItemViewModel>();
            foreach (string parameter in reader.ReadParameters("manifest"))
            {

                var assemblyCollection = new ObservableCollection<BaseItemViewModel>(reader.GetRulesByParamater(parameter, "assembly", "manifest", typeof(ManifestItem)));
                var includeCollection = new ObservableCollection<BaseItemViewModel>(reader.GetRulesByParamater(parameter, "include", "manifest", typeof(ManifestItem)));
                var model = new ManifestItemViewModel()
                {
                    RuleNameToDisplay = "Manifest",
                    AssemblyCollection = new EditCollectionViewModel( assemblyCollection, typeof(ManifestItem)),
                    IncludeCollection = new EditCollectionViewModel( includeCollection, typeof(ManifestItem)),
                     Parameter= parameter,
                    
                };
                _manifestCollection.Add(model);

            }


            SourceString = reader.GetRulesSourceStringPropertyValueByName("manifest");
            _manifestCollection.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ManifestCollectionCollectionChanged);

        }

        void ManifestCollectionCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DefaultChangeFactory.OnCollectionChanged(this, "ManifestCollection", ManifestCollection, e);
            OnPropertyChanged("ManifestCollection");
        }

        private ManifestItemViewModel _selectedFile;
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

            this.ManifestCollection.Add(new ManifestItemViewModel() { IncludeCollection = new EditCollectionViewModel( new ObservableCollection<BaseItemViewModel>(), typeof(ManifestItem)), AssemblyCollection = new EditCollectionViewModel( new ObservableCollection<BaseItemViewModel>(), typeof(ManifestItem)) });
        }

        #endregion


     

    }
    public class ManifestItemViewModel : ExtraPropertiesForCollectionsViewModelBase
    {

        public ManifestItemViewModel()
        {
            //this.IncludeCollection.EditableItems.
        }

        private EditCollectionViewModel _includeCollection;
        public EditCollectionViewModel IncludeCollection
        {
            get { return _includeCollection; }
            set
            {
                _includeCollection = value;
                OnPropertyChanged("IncludeCollection");
            }
        }


        private EditCollectionViewModel _assemblyCollection;
        public EditCollectionViewModel AssemblyCollection
        {
            get { return _assemblyCollection; }
            set
            {
                _assemblyCollection = value;
                OnPropertyChanged("AssemblyCollection");
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
                    MainWindowViewModel.Instance.Reader.SetNewParameter("manifest", _parameter, value);
                    UpdateParameterForEveryItemInTheCollection(value, AssemblyCollection.EditableItems);
                   UpdateParameterForEveryItemInTheCollection(value,IncludeCollection.EditableItems);
                   DefaultChangeFactory.OnChanging(this, "Parameter", _parameter, value);
                    _parameter = value;
                   
                }

                OnPropertyChanged("Parameter");

            }
        }
       

    }
    public class ManifestItem : BaseItemViewModel
    {
        
        public override string ProcessSourceValue(string newValue, string oldValue)
        {
            this.RuleNameToDisplay = "Manifest";
            //string ruleName, int index, string parameter, string colectionName, string newValue
            return MainWindowViewModel.Instance.Reader.SetRulesWithParameters("manifest", this.Parameter, this.CollectionName,oldValue, newValue);
        }
    }
}