using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using System.Collections.ObjectModel;

using CoAppPackageMaker.ViewModels.Base;
using CoAppPackageMaker.ViewModels.RuleViewModels;

namespace CoAppPackageMaker.ViewModels
{
    public class RequiresViewModel : ExtraPropertiesForCollectionsViewModelBase
    {
        private PackageReader _packageReader;
        public RequiresViewModel (PackageReader reader)
        {
            _packageReader = reader;
            RuleNameToDisplay = "Require";
            EditCollectionViewModel = new EditCollectionViewModel(reader,
                                                                  reader.GetRulesSourceValuesByNameForEditableCollections(
                                                                      "requires", "package"), typeof(RequireItem));

            SourceString = reader.GetRulesSourceStringPropertyValueByName("requires");
            this.EditCollectionViewModel.EditableItems.CollectionChanged += FilesCollectionCollectionChanged;
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

        void FilesCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                //update source 
                //(e.OldStartingIndex)
            }
            SourceString = "GETNEWWWWW -setat in viewmodelrequires";
        }

        //item for the editable collection  - get reevaluated value, after updating source 
        public class RequireItem : BaseItemViewModel
        {
            public override string ProcessSourceValue(string input)
            {
                return this.Reader.SetSourceRequireSigningRules("requires", "package", this.Index, input); 
            }
        }
    }
    }

