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




        private PackageReader _reader;

        //string define = "*";
        //Flavor = reader.GetRulesPropertyValueByName(define, "flavor");
        //Architecture = reader.GetRulesPropertyValueByName(define, "arch");
        //SourceString = reader.GetRulesSourceStringPropertyValueByName("*");
        //IsEditable =false;
        //_sourceValueDefineViewModel = new DefineViewModel()
        //                                  {
        //                                      Architecture = reader.GetRulesSourcePropertyValueByName("*", "arch" ),
        //                                      Flavor = reader.GetRulesSourcePropertyValueByName("*", "flavor"),
        //                                      IsEditable = true
        //                                  };





        private ObservableCollection<DefineItemViewModel> _defineCollection;

        public ObservableCollection<DefineItemViewModel> DefineCollection
        {
            get { return _defineCollection; }
            set
            {
                _defineCollection = value;
                OnPropertyChanged("DefineCollection");
            }
        }


        public DefineViewModel()
        {
        }

        public DefineViewModel(PackageReader reader, MainWindowViewModel root)
        {
            _reader = reader;
            _defineCollection = reader.GetDefineRules("value", this);
            SourceDefineViewModel = new DefineViewModel {Root = root};
            SourceDefineViewModel._defineCollection = reader.GetDefineRules("source", SourceDefineViewModel);
            SourceString = reader.GetRulesSourceStringPropertyValueByName("*");
            SourceDefineViewModel.DefineCollection.CollectionChanged += ContentCollectionChanged;


        }



        private DefineViewModel _sourceManifestViewModel;

        public DefineViewModel SourceDefineViewModel
        {
            get { return _sourceManifestViewModel; }
            set
            {
                _sourceManifestViewModel = value;
                OnPropertyChanged("SourceDefineViewModel");
            }
        }

        public class DefineItemViewModel : ExtraPropertiesViewModelBase
        {

            private string _label;

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


            private string _value;

            public string Value
            {
                get { return _value; }
                set
                {
                    DefaultChangeFactory.OnChanging(this, "Value", _value, value);
                    _value = value;
                    OnPropertyChanged("Value");
                }
            }



        }

       
        public void ContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
 
        {
        }
    }
}
