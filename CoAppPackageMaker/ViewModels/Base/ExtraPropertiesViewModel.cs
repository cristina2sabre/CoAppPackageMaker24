using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels
{

    public abstract class ExtraPropertiesViewModelBase : ViewModelBase, ISupportsUndo, ISupportUndoNotification

    {
        private string _helpTip;
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

        private string _sourceString;
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

      
     


        private int _undoCounter;

        public int UndoCounter
        {
            get { return _undoCounter; }
        }

        

      

        virtual public object GetUndoRoot()
        {
            if (this.IsSource == true)
            {
                if (MainWindowViewModel.Instance != null) { return MainWindowViewModel.Instance; }
                return null;
            }
            return null;


        }

    
       
        public List<string> Search(string toSearch)

        {
            List<string> result = new List<string>(4);
            foreach (var prop in this.GetType().GetProperties())
            {
                if (prop.GetValue(this, null).ToString().Contains(toSearch))
                {
                    result.Add(prop.Name);
                }
            }
            return result;
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
           public void UndoHappened(Change change)
        {
            Interlocked.Increment(ref _undoCounter);
            OnPropertyChanged("UndoCounter");
        }

           public void RedoHappened(Change change)
           {
               Interlocked.Increment(ref _undoCounter);
               OnPropertyChanged("UndoCounter");
           }
    }
   
}
