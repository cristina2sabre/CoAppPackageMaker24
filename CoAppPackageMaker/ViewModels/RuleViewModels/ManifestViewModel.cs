﻿using System.Collections.ObjectModel;
﻿using System.Linq;
﻿using System.Windows.Forms;
﻿using System.Windows.Input;
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
                    RuleNameToDisplay = "manifest",
                    AssemblyCollection = new EditCollectionViewModel(assemblyCollection, "assembly", "manifest", typeof(ManifestItem),parameter),
                    IncludeCollection = new EditCollectionViewModel(includeCollection, "include", "manifest", typeof(ManifestItem),parameter),
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
           

            var col = this.ManifestCollection.Where(item => item.Parameter == null);
            if (!col.Any())
            {
                var newItem = new ManifestItemViewModel()
                {
                    IncludeCollection =
                        new EditCollectionViewModel(new ObservableCollection<BaseItemViewModel>(),
                                                    "include", "manifest", typeof(ManifestItem)),
                    AssemblyCollection =
                        new EditCollectionViewModel(new ObservableCollection<BaseItemViewModel>(),
                                                    "assembly", "manifest", typeof(ManifestItem))
                };
               this.ManifestCollection.Add(newItem);
            }
            else
            {
                MessageBox.Show("An item with the same parameters exist aready in the collection");
            }
        }

        #endregion

        
    }

    public class ManifestItemViewModel : ExtraPropertiesForCollectionsViewModelBase
    {

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


        private string _parameter ;
        public string Parameter
        {
            get { return _parameter; }
            set
            {
                
                if (value != null)
                {
                    MainWindowViewModel.Instance.Reader.SetNewParameter("manifest", _parameter, value);
                    UpdateParameterForEveryItemInTheCollection(value, AssemblyCollection.EditableItems);
                    AssemblyCollection.Parameter = value;
                    IncludeCollection.Parameter = value;
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
        public ManifestItem(string ruleName, string collectionName)
        {
            RuleNameToDisplay = ruleName;
            CollectionName = collectionName;
           

        }
        public override string ProcessSourceValue(string newValue, string oldValue)
        {
            this.RuleNameToDisplay = "manifest";
            return MainWindowViewModel.Instance.Reader.SetRulesWithParameters("manifest", this.CollectionName, oldValue, newValue, this.Parameter);
        }

          
    }
}