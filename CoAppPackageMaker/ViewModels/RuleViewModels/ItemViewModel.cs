using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
    public class ItemViewModel : ExtraPropertiesViewModelBase
    {
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
                    //if (_reader != null)
                    //{

                    //    Value = _reader.SetSourceDefineRules(this.Label, new[] { (_sourceValue) });

                    //}

                }
            }
        }
    }
        
}
