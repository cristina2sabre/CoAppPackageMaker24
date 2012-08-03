using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{    
    /// <summary>
    /// Used in All Editable Collections, in order to update correctly value when source value is changed
    /// </summary>
    public abstract class BaseItemViewModel : ExtraPropertiesForCollectionsViewModelBase
    {
        protected BaseItemViewModel(ObservableCollection<BaseItemViewModel> editableItems)
        {
            _collection = editableItems;
        }

        protected BaseItemViewModel()
        {

        }


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
                    //var enumerable = _collection.Select(item => item.SourceValue == value);
                    var existingItems = new ObservableCollection<BaseItemViewModel>(Collection.Where(item => item.SourceValue == value));
                    if (existingItems.Count==0)
                    {
                        DefaultChangeFactory.OnChanging(this, "SourceValue", _sourceValue, value);
                        Value = ProcessSourceValue(value, _sourceValue);
                        _sourceValue = value;
                        OnPropertyChanged("SourceValue");
                        
                        
                    }
                   
                    else
                    {
                        MessageBox.Show("Item with the same name already exists");
                    }
                }
            }
        }

        //updating source value and getting new reevaluated 
        public abstract string ProcessSourceValue(string newValue, string oldValue);


        private ObservableCollection<BaseItemViewModel> _collection = new ObservableCollection<BaseItemViewModel>();
        public ObservableCollection<BaseItemViewModel> Collection
        {
            get { return _collection; }
            set
            {
                _collection = value;
                OnPropertyChanged("Collection");
            }
        }




        private string _parameter = "";
        public string Parameter
        {
            get { return _parameter; }
            set
            {
                if (MainWindowViewModel.Instance != null)
                {
                    MainWindowViewModel.Instance.RemoveError(_parameter);
                }

                DefaultChangeFactory.OnChanging(this, "Parameter", _parameter, value);
                _parameter = value;
                OnPropertyChanged("Parameter");
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
       
    }
    
}
