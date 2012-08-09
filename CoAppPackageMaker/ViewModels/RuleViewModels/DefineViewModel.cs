
using System;
using System.Collections.Specialized;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
    public class DefineViewModel : ExtraPropertiesForCollectionsViewModelBase
    {


        public DefineViewModel(PackageReader reader)
        {
            RuleNameToDisplay = "Define";
            EditCollectionViewModel = new EditCollectionViewModel( reader.GetDefineRules(), String.Empty,"define",typeof(DefineItem));
            SourceString = reader.GetRulesSourceStringPropertyValueByName("*");
            this.EditCollectionViewModel.EditableItems.CollectionChanged += FilesCollectionCollectionChanged;
        }

        

        /// <summary>
        /// Add error to the collection if a define is used in some rules
        /// Remove the error from the list when an define rule is added (redo/undo/add)  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FilesCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action==NotifyCollectionChangedAction.Remove)
            {
                string itemToRemove = ((DefineItem)e.OldItems[0]).Label;
                MainWindowViewModel.Instance.SearchForAllUsings(itemToRemove);
                //MainWindowViewModel.Instance.RemoveError(itemToRemove);
            }

            else if (e.Action==NotifyCollectionChangedAction.Add)
            {
                string itemToAdd = ((DefineItem)e.NewItems[0]).Label;
                MainWindowViewModel.Instance.RemoveError(itemToAdd);
            }
        }
    }


    public class DefineItem : BaseItemViewModel
    {

        public DefineItem(string ruleName, string collectionName)
        {
            RuleNameToDisplay = ruleName;
            CollectionName = collectionName;
            
        }

        public override string ProcessSourceValue(string newValue, string oldValue)
        {
            return MainWindowViewModel.Instance.Reader.SetSourceDefineRules(this.Label, new[] { newValue });
        }

        private string _label = "NewLabel";
        public string Label
        {
            get { return _label; }
            set
            {
                if (MainWindowViewModel.Instance != null)
                {
                    MainWindowViewModel.Instance.RemoveError(_label);
                }

                DefaultChangeFactory.OnChanging(this, "Label", _label, value);
                _label = value;
                OnPropertyChanged("Label");
            }
        }

        private string _sourceValue = "new";
        public new string SourceValue
        {
            get { return _sourceValue; }
            set
            {
                if (value != String.Empty)
                {
                    DefaultChangeFactory.OnChanging(this, "SourceValue", _sourceValue, value);
                    _sourceValue = value;
                    OnPropertyChanged("SourceValue");
                    //in order do not set self-referenced values like this: "arch :${arch}"
                    if (!_sourceValue.Contains("${" + this.Label + "}"))
                    {
                        Value = ProcessSourceValue(value, _sourceValue);
                    }
                }
                MainWindowViewModel.Instance.RefreshAllBindings();
            }
        }
    }
}
