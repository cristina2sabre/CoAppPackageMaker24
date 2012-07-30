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

                var assemblyCollection = new ObservableCollection<BaseItemViewModel>(reader.GetManifestFinal(parameter, "assembly", "manifest", typeof(ManifestItem)));
                var includeCollection = new ObservableCollection<BaseItemViewModel>(reader.GetManifestFinal(parameter, "include", "manifest", typeof(ManifestItem)));
                var model = new ManifestItemViewModel()
                {
                    AssemblyCollection = new EditCollectionViewModel(reader, assemblyCollection, typeof(ManifestItem)),
                    IncludeCollection = new EditCollectionViewModel(reader, includeCollection, typeof(ManifestItem)),
                    Name = parameter,
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
            
            this.ManifestCollection.Add(new ManifestItemViewModel() { IncludeCollection = new EditCollectionViewModel(null, new ObservableCollection<BaseItemViewModel>(), null), AssemblyCollection = new EditCollectionViewModel(null, new ObservableCollection<BaseItemViewModel>(), typeof(ManifestItem)) });
        }

        #endregion


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

    }

    public class ManifestItem : BaseItemViewModel
    {
        public override string ProcessSourceValue(string input)
        {
            //string ruleName, int index, string parameter, string colectionName, string newValue
            return this.Reader.SetManifestFinal("manifest",this.Index,this.Label,this.CollectionName, input);
        }
    }
}