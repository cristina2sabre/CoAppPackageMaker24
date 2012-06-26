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
        public delegate string Process(IEnumerable<string> sEnumerable);

        //property
        public  Process UpdateSource { get; set; }

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
                      if(UpdateSource!=null){ Value = UpdateSource(new[] { (_sourceValue) });}
                       
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
