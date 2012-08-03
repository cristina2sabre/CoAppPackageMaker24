using System.Collections.Specialized;


using CoAppPackageMaker.ViewModels.Base;
using CoAppPackageMaker.ViewModels.RuleViewModels;

namespace CoAppPackageMaker.ViewModels
{
    public class RequiresViewModel : ExtraPropertiesForCollectionsViewModelBase
    {
       
        public RequiresViewModel (PackageReader reader)
        {
          
            RuleNameToDisplay = "Require";
            EditCollectionViewModel = new EditCollectionViewModel(
                                                                  reader.GetRulesSourceValuesByNameForEditableCollections(
                                                                      "requires", "package", typeof(RequireItem)), "package", "requires", typeof(RequireItem));

            SourceString = reader.GetRulesSourceStringPropertyValueByName("requires");
            this.EditCollectionViewModel.EditableItems.CollectionChanged += FilesCollectionCollectionChanged;
        }



      

        void FilesCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //((RequireItem) e.NewItems[0]).SourceValue = "Hello world";
            //this.EditCollectionViewModel.EditableItems
            
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                //update source 
                //(e.OldStartingIndex)
               
                
            }
            SourceString = "GETNEWWWWW -setat in viewmodelrequires";
        }

        //item for the editable collection  - get reevaluated value, after updating source 
       
    }
    public class RequireItem : BaseItemViewModel
    {
        public RequireItem(string ruleName, string collectionName)
        {
            RuleNameToDisplay = ruleName;
            CollectionName = collectionName;
           
        }

        public override string ProcessSourceValue(string newValue, string oldValue)
        {
            return MainWindowViewModel.Instance.Reader.SetSourceRequireSigningRules("requires", "package", oldValue, newValue);
        }
    }
    }

