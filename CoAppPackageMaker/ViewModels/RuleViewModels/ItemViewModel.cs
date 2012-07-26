﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
﻿using CoAppPackageMaker.ViewModels.Base;
﻿using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
    public class ItemViewModel : ExtraPropertiesForCollectionsViewModelBase
    {

        //type
        public delegate string Process(string parameter, IEnumerable<string> sEnumerable);

        //property
        public Process UpdateSource { get; set; }

        private string _label = "NewLabel";
        public string Label
        {
            get { return _label; }
            set
            {
                if(MainWindowViewModel.Instance!=null)
                {
                    MainWindowViewModel.Instance.RemoveError(_label);
                }
               
                DefaultChangeFactory.OnChanging(this, "Label", _label, value);
                _label = value;
                OnPropertyChanged("Label");
            }
        }

        private string _parameter;
        public string Parameter
        {
            get { return _parameter; }
            set
            {
                _parameter = value;
                OnPropertyChanged("Parameter");
            }
        }


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


        private string _value = "new";
        public string Value
        {
            get { return _value; }
            set
            {

                _value = value;
                OnPropertyChanged("Value");
                //if (MainWindowViewModel.Instance != null)
                //{

                //    MainWindowViewModel.Instance.SearchAll(this.Label);

                //}

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
                        var s = new[] {(_sourceValue)};
                        //string aaa = s[0];
                        //string ddd ="${"+  this.Label+"}";
                        //var contains = aaa.Contains(ddd);
                        //bool a = s[0].ToString().Contains(String.Format("${{0}}", this.Label));
                        if (!s[0].Contains("${" + this.Label + "}"))
                        {
                            if (UpdateSource != null) { Value = UpdateSource(this.Label, new[] { (_sourceValue) }); }
                        }
                       

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