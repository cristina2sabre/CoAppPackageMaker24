using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
    public class LicenseViewModel : ExtraPropertiesForCollectionsViewModelBase
    {
       
        private const string LicenseString = "license";

        public LicenseViewModel(PackageReader reader)
        {

            _licenseCollection = new ObservableCollection<LicenseItemViewModel>();
           
            foreach (string parameter in reader.ReadParameters(LicenseString))
            {

                var model = new LicenseItemViewModel()
                                {
                                  
                                    Parameter = parameter,
                                    License = reader.GetFilesRulesPropertyValueByParameterAndName(LicenseString,parameter, "license"),
                                   LicenseUrl =
                                        reader.GetFilesRulesPropertyValueByParameterAndName(LicenseString,parameter,"license-url" ),
                                    LicenseType = reader.GetFilesRulesPropertyValueByParameterAndName(LicenseString, parameter, "license-type"),
                                };

                _licenseCollection.Add(model);
            }

            RuleNameToDisplay = "License";
            SourceString = reader.GetRulesSourceStringPropertyValueByName(LicenseString);
            _licenseCollection.CollectionChanged += LicenseCollectionCollectionChanged;


        }

        private ObservableCollection<LicenseItemViewModel> _licenseCollection;
        public ObservableCollection<LicenseItemViewModel> LicenseCollection
        {
            get { return _licenseCollection; }
            set
            {
                _licenseCollection = value;
                OnPropertyChanged("LicenseCollection");
            }
        }

        //needed for undo/redo
        private void LicenseCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            DefaultChangeFactory.OnCollectionChanged(this, "LicenseCollection", LicenseCollection, e);
            OnPropertyChanged("LicenseCollection");
        }


        //used for delete
        private LicenseItemViewModel _selectedFile;

        public LicenseItemViewModel SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                OnPropertyChanged("SelectedFile");
            }
        }

       

        public ICommand RemoveCommand
        {
            get { return new RelayCommand(Remove, CanRemove); }
        }

        public void Remove()
        {
            this.LicenseCollection.Remove(this.SelectedFile);
            //MainWindowViewModel.Instance.Reader.RemoveFromList(this.SelectedFile.RuleNameToDisplay,
            //                                                                this.SelectedFile.CollectionName,
            //                                                                this.SelectedFile.SourceValue);
        }

        private bool CanRemove()
        {
            return this.SelectedFile != null;

        }

        public ICommand AddCommand
        {
            get { return new RelayCommand(Add, CanAdd); }
        }

        private bool CanAdd()
        {
            return this.LicenseCollection != null;

        }

        public void Add()
        {

            var col = this.LicenseCollection.Where(item => item.Parameter == null);
            if (!col.Any())
            {
              
                this.LicenseCollection.Add(new LicenseItemViewModel());
            }
            else
            {
                MessageBox.Show("An item with the same parameters exist aready in the collection");
            }

        }








        public class LicenseItemViewModel : ExtraPropertiesForCollectionsViewModelBase
        {

            private string _license;
            public string License
            {
                get { return _license; }
                set
                {
                    DefaultChangeFactory.OnChanging(this, "License", _license, value);
                    _license = MainWindowViewModel.Instance.Reader.SetNewSourceValue("license", "license", value, this.Parameter);
                    OnPropertyChanged("License");
                }
            }

            private string _licenseUrl;
            public string LicenseUrl
            {
                get { return _licenseUrl; }
                set
                {
                    DefaultChangeFactory.OnChanging(this, "LicenseUrl", _licenseUrl, value);
                    _licenseUrl = MainWindowViewModel.Instance.Reader.SetNewSourceValue("license", "license-url", value, this.Parameter);
                    OnPropertyChanged("LicenseUrl");
                }
            }


            private string _licenseType;
            public string LicenseType
            {
                get { return _licenseType; }
                set
                {
                    DefaultChangeFactory.OnChanging(this, "LicenseType", _licenseType, value);
                    _licenseType =  MainWindowViewModel.Instance.Reader.SetNewSourceValue("license","license-type", value, this.Parameter);
                    OnPropertyChanged("LicenseType");
                }
            }

            private string _parameter;
            public string Parameter
            {
                get { return _parameter; }
                set
                {
                    if (value != null)
                    {
                        var contains = MainWindowViewModel.Instance.Reader.ReadParameters(LicenseString).Contains(value);
                         MainWindowViewModel.Instance.Reader.SetNewParameter(LicenseString, _parameter, value);
                        //UpdateParameterForEveryItemInTheCollection(value, EditCollectionViewModel.EditableItems);
                        //EditCollectionViewModel.Parameter = value;
                        DefaultChangeFactory.OnChanging(this, "Parameter", _parameter, value);
                        _parameter = value;

                    }
                    OnPropertyChanged("Parameter");

                }
            }
        }


    }

}