using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace CoAppPackageMaker.ViewModels
{
     public class ManifestViewModel : ExtraPropertiesViewModelBase
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

            foreach (string parameter in reader.ReadManifestParameters())
            {

                ObservableCollection<string> includeCollection = new ObservableCollection<string>(reader.GetManifestIncludeList(parameter, "include"));
                ObservableCollection<string> assemblyCollection = new ObservableCollection<string>(reader.GetManifestIncludeList(parameter, "assembly"));
                ManifestItemViewModel model = new ManifestItemViewModel(parameter)
                {
                    
                    Include = includeCollection,
                    Assembly = assemblyCollection,
                   
                };
                _manifestCollection.Add(model);
            }

            {
                _sourceManifestViewModel = new ManifestViewModel();
                SourceManifestViewModel._manifestCollection = new ObservableCollection<ManifestItemViewModel>();

                foreach (string parameter in reader.ReadManifestParameters())
                {
                  
                    ObservableCollection<string> includeCollection = new ObservableCollection<string>(reader.GetManifestIncludeList2("manifest", "include"));
                    ObservableCollection<string> assemblyCollection = new ObservableCollection<string>(reader.GetManifestIncludeList2("manifest", "assembly"));
                    ManifestItemViewModel model = new ManifestItemViewModel(parameter)
                    {

                        Include = includeCollection,
                        Assembly = assemblyCollection,

                    };
                    SourceManifestViewModel._manifestCollection.Add(model);
                }

                
            }

          
           
           
             SourceString = reader.GetRulesSourceStringPropertyValueByName("manifest");

        }


        private ManifestViewModel _sourceManifestViewModel;

        public ManifestViewModel SourceManifestViewModel
        {
            get { return _sourceManifestViewModel; }
            set
            {
                _sourceManifestViewModel = value;
                OnPropertyChanged("SourceManifestViewModel");
            }
        }

        public class ManifestItemViewModel : ViewModelBase
        {

            private ObservableCollection<string> _assembly;
            public ObservableCollection<string> Assembly
            {
                get { return _assembly; }
                set
                {
                    _assembly = value;
                    OnPropertyChanged("Assembly");
                }
            }

            private ObservableCollection<string> _include;
            public ObservableCollection<string> Include
            {
                get { return _include; }
                set
                {
                    _include = value;
                    OnPropertyChanged("Include");
                }
            }

            public ManifestItemViewModel(string parameter)
            {
                Name = String.Format("Manifest[{0}]", parameter);
            }

            private string _name;
            public string Name
            {
                get { return _name; }
                set
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }

        }

       
    }
}
