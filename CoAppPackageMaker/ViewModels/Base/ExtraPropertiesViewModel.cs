using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels
{
    public abstract class ExtraPropertiesViewModelBase : ViewModelBase, ISupportsUndo
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

        private MainWindowViewModel _root;
        public MainWindowViewModel Root
        {
            get { return _root; }
            set
            {
                if (value == _root)
                    return;

                // This line will log the property change with the undo framework.
                DefaultChangeFactory.OnChanging(this, "Root", _root, value);

                _root = value;
                OnPropertyChanged("Root");
            }
        }

        #region ISupportsUndo Members

        public object GetUndoRoot()
        {
            return this.Root;
        }

        #endregion

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
    }
}
