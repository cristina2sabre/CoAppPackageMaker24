using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.ObjectModel;
using CoApp.Packaging.Client;
using CoAppPackageMaker.ViewModels.RuleViewModels;

namespace CoAppPackageMaker.ViewModels
{
    public class RequiresViewModel : ExtraPropertiesViewModelBase
    {
        public RequiresViewModel()
        {

        }

        public RequiresViewModel (PackageReader reader)
        {
          
          _requiredPackages = new ObservableCollection<string>(reader.GetRulesPropertyValues("requires", "package"));
            SourceValueRequiresViewModel = new RequiresViewModel()
                                               {
                                                    _requiredPackages = new ObservableCollection<string>(reader.GetRulesSourcePropertyValuesByName("requires", "package"))
                                               };

            SourceString = reader.GetRulesSourceStringPropertyValueByName("requires");
        }

        private ObservableCollection<string> _requiredPackages;

        ///update the name back
        //public ObservableCollection<string> FilesCollection
        //{
        //    get { return _requiredPackages; }
        //    set
        //    {
        //        _requiredPackages = value;
        //        OnPropertyChanged("FilesCollection");
        //    }
        //}

        private RequiresViewModel _sourceValueRequiresViewModel;

        public RequiresViewModel SourceValueRequiresViewModel
        {
            get { return _sourceValueRequiresViewModel; }
            set
            {
                _sourceValueRequiresViewModel = value;
                OnPropertyChanged("RequiresViewModel");
            }
        }
     
        }
    }

