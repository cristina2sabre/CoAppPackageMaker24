using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
    public class DefineViewModel : ExtraPropertiesForCollectionsViewModelBase
    {


        public DefineViewModel(PackageReader reader)
        {

            EditCollectionViewModel = new EditCollectionViewModel(reader,  reader.GetDefineRules());
            //_reader = reader;
            //Root = root;
            //_defineCollection = reader.GetDefineRules(this);
            SourceString = reader.GetRulesSourceStringPropertyValueByName("*");
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

        public class DefineItemViewModel : ExtraPropertiesForCollectionsViewModelBase
        {

          

        }

    }
}
