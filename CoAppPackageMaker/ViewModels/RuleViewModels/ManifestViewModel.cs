using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace CoAppPackageMaker.ViewModels
{
    class ManifestViewModel : ExtraPropertiesViewModelBase
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
                    Assembly = assemblyCollection
                };
                _manifestCollection.Add(model);
            }

        }
        



        public class ManifestItemViewModel : ViewModelBase
        {


            private ObservableCollection<string> _assembly;
            private ObservableCollection<string> _include;


            public ObservableCollection<string> Assembly
            {
                get { return _assembly; }
                set
                {
                    _assembly = value;
                    OnPropertyChanged("Assembly");
                }
            }

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
