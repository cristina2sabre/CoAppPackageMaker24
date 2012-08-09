using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Media;
using System.Windows.Shapes;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;
using CoAppPackageMaker.ViewModels.RuleViewModels;

namespace CoAppPackageMaker.ViewModels
{
    public abstract class ExtraPropertiesForCollectionsViewModelBase : ViewModelBase, ISupportsUndo, ISupportUndoNotification
    {

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



        protected void UpdateParameterForEveryItemInTheCollection(string parameter, ObservableCollection<BaseItemViewModel> collection)
        {
            foreach (var item in collection)
            {
                item.Parameter = parameter;
            }
        }

        private string _ruleNameToDisplay="Rule Name to Display--TEMP";
        public string RuleNameToDisplay
        {
            get { return _ruleNameToDisplay; }
            set
            {
                _ruleNameToDisplay = value;
                OnPropertyChanged("RuleNameToDisplay");
            }
        }

        private SolidColorBrush _statusColor = Brushes.Green;
        public SolidColorBrush StatusColor
        {
            get { return _statusColor; }
            set
            {
                _statusColor = value;
                OnPropertyChanged("StatusColor");
            }
        }

        private string _sourceString=String.Empty;
        public string SourceString
        {
            get { return _sourceString; }
            set
            {
                _sourceString = value;
                OnPropertyChanged("SourceString");
            }
        }

       
        
        public bool IsReadOnly { get; set; }


        virtual public object GetUndoRoot()
        {

            return MainWindowViewModel.Instance;

        }





        public List<Tuple<string, string>> Search(string toSearch)
        {
            string name = RuleNameToDisplay;
            var result = new List<Tuple<string, string>>();
            foreach (var prop in this.GetType().GetProperties())
            {
                if (prop.Name.Equals("ManifestCollection") )
                {
                    result.AddRange(SearchInCollections(toSearch, prop));
                }


                if (prop.Name != "SelectedFile" && prop.Name != "SourceString" && prop.Name != "EditCollectionViewModel")
                {
                    var tempString = prop.GetValue(this, null).ToString();
                    if (tempString.Contains(toSearch))
                    {
                        result.Add(new Tuple<string, string>(name, prop.Name));
                    }
                }


            }
            //for search in collections of editable items

            var method = this.GetType().GetProperty("EditCollectionViewModel");
            if (method != null)
            {
                var value = method.GetValue(this, null);
                if (value != null)
                {
                    foreach (BaseItemViewModel item in ((EditCollectionViewModel) value).EditableItems)
                    {
                        if (item.SourceValue.Contains(toSearch))
                        {
                            result.Add(new Tuple<string, string>(name, String.Format("Collection  {0}", item.Parameter)));
                            break;
                        }
                    }
                }

            }
            return result;
        }

        private List<Tuple<string, string>> SearchInCollections(string toSearch, PropertyInfo prop)
        {
            var result = new List<Tuple<string, string>>();
              var propertyInfo = this.GetType().GetProperty(prop.Name);
                  if (propertyInfo != null)
                  {
                      var value = propertyInfo.GetValue(this, null);
                      if (value != null)
                      {
                          foreach (var item in (ObservableCollection<ManifestItemViewModel>)value)
                          {
                              foreach (var property in item.GetType().GetProperties())
                              {
                                  var name = property.Name;
                                  if (name.Contains("Collection"))
                                  {
                                      var propertyCollection = item.GetType().GetProperty(name);
                                      if (propertyCollection != null)
                                      {
                                          var itemValue = propertyCollection.GetValue(item, null);

                                          if (itemValue != null)
                                          {
                                              foreach (
                                                  BaseItemViewModel model in
                                                      ((EditCollectionViewModel) itemValue).EditableItems)
                                              {
                                                  if (model.SourceValue.Contains(toSearch))
                                                  {
                                                    
                                                      result.Add(new Tuple<string, string>(model.RuleNameToDisplay,
                                                                                           String.Format("[{0}] {1}",
                                                                                                         model.Parameter,
                                                                                                         model.
                                                                                                             CollectionName)));
                                                      break;
                                                  }
                                              }
                                          }

                                      }
                                  }

                              }
                          }
                      }
                  }

          return result;
        }
        
        private int _undoCounter;
        public int UndoCounter
        {
            get { return _undoCounter; }
        }

        public void RedoHappened(Change change)
        {
            Interlocked.Increment(ref _undoCounter);
            OnPropertyChanged("UndoCounter");
        }

        public void UndoHappened(Change change)
        {
            Interlocked.Increment(ref _undoCounter);
            OnPropertyChanged("UndoCounter");
        }
    }

    public abstract class ExtraPropertiesViewModelBase : ExtraPropertiesForCollectionsViewModelBase
    {
        override public object GetUndoRoot()
        {
            if (this.IsSource == true)
            {
                return MainWindowViewModel.Instance;
                
            }
            return null;
        }

        private bool _isSource = false;
        public bool IsSource
        {
            get { return _isSource; }
            set
            {
                _isSource = value;
                OnPropertyChanged("IsSource");
            }
        }


     
       
        
    }
}
