﻿﻿using System.Collections.ObjectModel;
﻿using System.Linq;
﻿using System.Windows.Forms;
﻿using System.Windows.Input;
using CoAppPackageMaker.ViewModels.Base;
using CoAppPackageMaker.ViewModels.RuleViewModels;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels
{
    public class DeveloperLibraryViewModel : ExtraPropertiesForCollectionsViewModelBase
    {

        private ObservableCollection<DeveloperLibraryItemViewModel> _developerLibraryCollection;
        public ObservableCollection<DeveloperLibraryItemViewModel> DeveloperLibraryCollection
        {
            get { return _developerLibraryCollection; }
            set
            {
                _developerLibraryCollection = value;
                OnPropertyChanged("DeveloperLibraryCollection");
            }
        }


        public DeveloperLibraryViewModel()
        {
        }

        public DeveloperLibraryViewModel(PackageReader reader)
        {
            _developerLibraryCollection = new ObservableCollection<DeveloperLibraryItemViewModel>();
            foreach (string parameter in reader.ReadParameters("developer-library"))
            {

                var headersCollection = new ObservableCollection<BaseItemViewModel>(reader.GetRulesByParameter(parameter, "headers", "developer-library", typeof(DeveloperLibraryItem)));
                var librariesCollection = new ObservableCollection<BaseItemViewModel>(reader.GetRulesByParameter(parameter, "libraries", "developer-library", typeof(DeveloperLibraryItem)));
                var docsCollection = new ObservableCollection<BaseItemViewModel>(reader.GetRulesByParameter(parameter, "docs", "developer-library", typeof(DeveloperLibraryItem)));
                var model = new DeveloperLibraryItemViewModel()
                {
                    RuleNameToDisplay = "developer-library",
                    LibrariesCollection = new EditCollectionViewModel(headersCollection, "headers", "developer-library", typeof(DeveloperLibraryItem),parameter),
                    HeadersCollection = new EditCollectionViewModel(librariesCollection, "libraries", "developer-library", typeof(DeveloperLibraryItem),parameter),
                    DocsCollection = new EditCollectionViewModel(docsCollection, "docs", "developer-library", typeof(DeveloperLibraryItem),parameter),
                    Parameter= parameter,
                    
                };
                _developerLibraryCollection.Add(model);

            }


            SourceString = reader.GetRulesSourceStringPropertyValueByName("developer-library");
            _developerLibraryCollection.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(DeveloperLibraryCollectionChanged);

        }

        void DeveloperLibraryCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DefaultChangeFactory.OnCollectionChanged(this, "DeveloperLibraryCollection", DeveloperLibraryCollection, e);
            OnPropertyChanged("DeveloperLibraryCollection");
        }

        private DeveloperLibraryItemViewModel _selectedFile;
        public DeveloperLibraryItemViewModel SelectedFile
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
            this.DeveloperLibraryCollection.Remove(this.SelectedFile);

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
            return this.DeveloperLibraryCollection != null;

        }
        public void Add()
        {
            var col = this.DeveloperLibraryCollection.Where(item => item.Parameter == "[]");
            if (!col.Any())
            {
                var newItem =
                    new DeveloperLibraryItemViewModel()
                        {
                            HeadersCollection =
                                new EditCollectionViewModel(new ObservableCollection<BaseItemViewModel>(), "headers",
                                                            "developer-library", typeof (DeveloperLibraryItem)),
                            LibrariesCollection =
                                new EditCollectionViewModel(new ObservableCollection<BaseItemViewModel>(), "libraries",
                                                            "developer-library", typeof (DeveloperLibraryItem)),
                            DocsCollection =
                                new EditCollectionViewModel(new ObservableCollection<BaseItemViewModel>(), "docs",
                                                            "developer-library", typeof (DeveloperLibraryItem))
                        };
                this.DeveloperLibraryCollection.Add(newItem);
            }
            else
            {
                MessageBox.Show("An item with the same parameters exist aready in the collection");
            }
         
        }

        #endregion

        
    }

    public class DeveloperLibraryItemViewModel : ExtraPropertiesForCollectionsViewModelBase
    {

        private EditCollectionViewModel _headersCollection;
        public EditCollectionViewModel HeadersCollection
        {
            get { return _headersCollection; }
            set
            {
                _headersCollection = value;
                OnPropertyChanged("HeadersCollection");
            }
        }


        private EditCollectionViewModel _librariesCollection;
        public EditCollectionViewModel LibrariesCollection
        {
            get { return _librariesCollection; }
            set
            {
                _librariesCollection = value;
                OnPropertyChanged("LibrariesCollection");
            }
        }

        private EditCollectionViewModel _docsCollection;
        public EditCollectionViewModel DocsCollection
        {
            get { return _docsCollection; }
            set
            {
                _docsCollection = value;
                OnPropertyChanged("DocsCollection");
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
                    MainWindowViewModel.Instance.Reader.SetNewParameter("developer-library", _parameter, value);
                    UpdateParameterForEveryItemInTheCollection(value, LibrariesCollection.EditableItems);
                    UpdateParameterForEveryItemInTheCollection(value,HeadersCollection.EditableItems);
                    UpdateParameterForEveryItemInTheCollection(value,DocsCollection.EditableItems);
                    LibrariesCollection.Parameter = value;
                    HeadersCollection.Parameter = value;
                    DocsCollection.Parameter = value;
                    DefaultChangeFactory.OnChanging(this, "Parameter", _parameter, value);
                    _parameter = value;
                   
                }

                OnPropertyChanged("Parameter");

            }
        }
    }

    public class DeveloperLibraryItem : BaseItemViewModel
    {
        public DeveloperLibraryItem(string ruleName, string collectionName)
        {
            RuleNameToDisplay = ruleName;
            CollectionName = collectionName;
            
        }
        public override string ProcessSourceValue(string newValue, string oldValue)
        {
            this.RuleNameToDisplay = "developer-library";
            return MainWindowViewModel.Instance.Reader.SetRulesWithParameters("developer-library", this.CollectionName, oldValue, newValue, this.Parameter);
        }

          
    }
}