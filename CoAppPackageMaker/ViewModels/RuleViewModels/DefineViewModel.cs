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
    public class DefineViewModel : ExtraPropertiesViewModelBase
    {

        //private PackageReader _reader;

        //private ObservableCollection<DefineItemViewModel> _defineCollection;
        //public ObservableCollection<DefineItemViewModel> DefineCollection
        //{
        //    get { return _defineCollection; }
        //    set
        //    {
        //        _defineCollection = value;
        //        OnPropertyChanged("DefineCollection");
        //    }
        //}


        public DefineViewModel(PackageReader reader, MainWindowViewModel mainWindowViewModel)
        {

            EditCollectionViewModel = new EditCollectionViewModel(reader, mainWindowViewModel, reader.GetDefineRules(mainWindowViewModel));
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

        public class DefineItemViewModel : ExtraPropertiesViewModelBase
        {

            //private string _label;
            //public string Label
            //{
            //    get { return _label; }
            //    set
            //    {
            //        DefaultChangeFactory.OnChanging(this, "Label", _label, value);
            //        _label = value;
            //        OnPropertyChanged("Label");
            //    }
            //}

            //private string _value;
            //public string Value
            //{
            //    get { return _value; }
            //    set
            //    {
            //        _value = value;
            //        OnPropertyChanged("Value");
            //    }
            //}

            //private string _sourceValue;
            //public string SourceValue
            //{
            //    get { return _sourceValue; }
            //    set
            //    {
            //        DefaultChangeFactory.OnChanging(this, "SourceValue", _sourceValue, value);
            //        _sourceValue = value;
            //        OnPropertyChanged("SourceValue");
            //        if (_reader != null)
            //        {

            //            Value = _reader.SetSourceDefineRules(this.Label, new[] { (_sourceValue) });

            //        }


            //    }
            //}

        //    private PackageReader _reader;
        //    public PackageReader Reader
        //    {
        //        get { return _reader; }
        //        set
        //        {
        //            _reader = value;
        //            OnPropertyChanged("Reader");
        //        }
        //    }

        }

    }
}
