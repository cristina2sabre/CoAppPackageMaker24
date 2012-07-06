using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.ObjectModel;
using CoApp.Packaging.Client;
using CoAppPackageMaker.ViewModels.Base;
using CoAppPackageMaker.ViewModels.RuleViewModels;

namespace CoAppPackageMaker.ViewModels
{
    public class RequiresViewModel : ExtraPropertiesForCollectionsViewModelBase
    {
       
        public RequiresViewModel (PackageReader reader)
        {

            EditCollectionViewModel = new EditCollectionViewModel(reader,
                                                                  reader.GetRulesSourcePropertyValuesByNameForRequired(
                                                                      "requires", "package"));

            SourceString = reader.GetRulesSourceStringPropertyValueByName("requires");
        }


        private EditCollectionViewModel _editCollectionViewModel;
        public EditCollectionViewModel EditCollectionViewModel
        {
            get { return _editCollectionViewModel; }
            set
            {
                _editCollectionViewModel = value;
                OnPropertyChanged("EditCollectionViewModel");
            }
        }
       
        }
    }

