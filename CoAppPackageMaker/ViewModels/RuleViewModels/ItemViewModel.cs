using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
    public class ItemViewModel : ExtraPropertiesViewModelBase
    {

        //type
        public delegate string Process(string parameter,IEnumerable<string> sEnumerable);

        //property
        public  Process UpdateSource { get; set; }
       
        private string _label = "NewLabel";
        public string Label
        {
            get { return _label; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "Label", _label, value);
                _label = value;
                OnPropertyChanged("Label");
            }
        }

        
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
                    
                  if (_reader != null)
                    {
                      //!!!!!!!!!!!!!!!!!WRONG-only for requires-ovveride needed
                      //  Value = _reader.SetSourceRequireRules( new[] { (_sourceValue) });
                      if(UpdateSource!=null){ Value = UpdateSource(this.Label,new[] { (_sourceValue) });}
                       
                 }

                }
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
