using System;
using System.Collections.Generic;
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

        private Color _statusColor = Colors.Green;
        public Color StatusColor
        {
            get { return _statusColor; }
            set
            {
                _statusColor = value;
                OnPropertyChanged("StatusColor");
            }
        }

        private string _helpTip="test";
        private bool _isRequired=false;

        public string HelpTip
        {
            get { return _helpTip; }
            set
            {
                _helpTip = value;
                OnPropertyChanged("HelpTip");
            }
        }

        public bool IsRequired
        {
            get { return _isRequired; }
            set
            {
                _isRequired = value;
                OnPropertyChanged("IsRequired");
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

        private bool _isEditable;

        public bool IsEditable
        {
            get { return _isEditable; }
            set
            {
                _isEditable = value;
                OnPropertyChanged("IsEditable");
            }
        }

        public bool IsReadOnly { get; set; }

      
      virtual  public object GetUndoRoot()
        {
            if (MainWindowViewModel.Instance != null) { return MainWindowViewModel.Instance; }
            else
            {
                return null;
            }


        }

    
       
        public List<Tuple<string,string>> Search(string toSearch)
        {
            string name = RuleNameToDisplay;
            var result =new  List<Tuple<string, string>>(4);
            foreach (var prop in this.GetType().GetProperties())
            {
              
                   
                   if(prop.Name!="SelectedFile")
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
                var s = method.GetValue(this, null);
                foreach (ItemViewModel item in (s as EditCollectionViewModel).EditableItems)
                {
                    if (item.SourceValue.Contains(toSearch))
                    {
                        result.Add(new Tuple<string, string>(name, String.Format("Collection {0}",item.Label)));
                        break;
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
                if (MainWindowViewModel.Instance != null) { return MainWindowViewModel.Instance; }
                return null;
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
