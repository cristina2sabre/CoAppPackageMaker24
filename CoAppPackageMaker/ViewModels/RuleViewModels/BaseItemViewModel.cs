using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{    
    /// <summary>
    /// Used in All Editable Collections, in order to update correctly value when source value is changed
    /// </summary>
    public abstract class BaseItemViewModel : ExtraPropertiesForCollectionsViewModelBase
    {
        //setted the same as Source because,to avoid difference between them
        //when a new item is added is added to a temporary collection, source is not reevaluated 
        private string _value="new";
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        //set defaultValue for SourceValue - when add a new Item to the collection
        private string _sourceValue = "new";
        public string SourceValue
        {
            get { return _sourceValue; }
            set
            {
                if (value != String.Empty)
                {
                    DefaultChangeFactory.OnChanging(this, "SourceValue", _sourceValue, value);
                    _sourceValue = value;
                    OnPropertyChanged("SourceValue");
                    Value = ProcessSourceValue(_sourceValue);
                }
            }
        }

        //updating source value and getting new reevaluated 
        public abstract string ProcessSourceValue(string newValue);

        //used for updating colection
        private int _index;
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                OnPropertyChanged("Index");
            }
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

        private string _collectionName;
        public string CollectionName
        {
            get { return _collectionName; }
            set
            {
                _collectionName = value;
                OnPropertyChanged("CollectionName");
            }
        }
         private PackageReader _reader;
        public PackageReader Reader
        {
            get { return _reader; }
            set
            {
                _reader = value;
                OnPropertyChanged("Reader");
            }
        }
        
    }

   
   
}
