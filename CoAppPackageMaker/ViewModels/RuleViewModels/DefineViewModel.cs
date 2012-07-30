
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
    public class DefineViewModel : ExtraPropertiesForCollectionsViewModelBase
    {


        public DefineViewModel(PackageReader reader)
        {
            RuleNameToDisplay = "Define";
            EditCollectionViewModel = new EditCollectionViewModel(reader,  reader.GetDefineRules(), typeof(DefineItem));
            SourceString = reader.GetRulesSourceStringPropertyValueByName("*");
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
                string itemToRemove = ((DefineItem) e.OldItems[0]).Label;
                MainWindowViewModel.Instance.SearchForAllUsings(itemToRemove);
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
        public override string ProcessSourceValue(string input)
        {
            return this.Reader.SetSourceDefineRules(this.Label, new[] {input});
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
                    if (!_sourceValue.Contains("${" + this.Label + "}"))
                    {
                        Value = ProcessSourceValue(_sourceValue);

                    }
                }
            }
        }
    }
}
