
namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
    public class DefineViewModel : ExtraPropertiesForCollectionsViewModelBase
    {


        public DefineViewModel(PackageReader reader)
        {
            EditCollectionViewModel = new EditCollectionViewModel(reader,  reader.GetDefineRules());
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

    }
}
