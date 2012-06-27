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
    public class RequiresViewModel : ExtraPropertiesViewModelBase
    {
       
        public RequiresViewModel (PackageReader reader, MainWindowViewModel mainWindowViewModel )
        {

            EditCollectionViewModel = new EditCollectionViewModel(reader, mainWindowViewModel,
                                                                  reader.GetRulesSourcePropertyValuesByNameForRequired(
                                                                      "requires", "package", mainWindowViewModel));

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

