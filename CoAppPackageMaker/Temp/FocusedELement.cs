using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CoAppPackageMaker.ViewModels;

namespace CoAppPackageMaker.Temp
{
    //http://blogs.msdn.com/b/luc/archive/2010/08/05/silverlight-wpf-debugging-determining-the-focused-element.aspx
    public class DebugFocusedElementProxy : FrameworkElement, INotifyPropertyChanged
    {
        private string _FocusedElementDescription;
        public string FocusedElementDescription
        {
            get
            {
                if (this.FocusedElement == null)
                    return "None";
                else
                {
                    if (!(this.FocusedElement is FrameworkElement) || string.IsNullOrEmpty(((FrameworkElement)this.FocusedElement).Name))
                        return this.FocusedElement.GetType().Name;
                    else
                        return ((FrameworkElement)this.FocusedElement).Name;
                }

            }
        }

        private UIElement _FocusedElement;
        public UIElement FocusedElement
        {
            get { return _FocusedElement; }
            set
            {
                _FocusedElement = value;
                OnNotifyPropertyChanged("FocusedElement");
                OnNotifyPropertyChanged("FocusedElementDescription");
                UpdateHepTip();
            }
        }

        private void UpdateHepTip()
        {
            string newHelptip = Properties.Resources.ResourceManager.GetString(FocusedElementDescription);
            if (!string.IsNullOrEmpty(newHelptip))
            {
                HelpForFocusedElement = newHelptip;
            }
        }

        private string _helpForFocusedElement;
        public string HelpForFocusedElement
        {
            get { return _helpForFocusedElement; }
            set
            {
               
                    _helpForFocusedElement = value;
                    OnNotifyPropertyChanged("HelpForFocusedElement");
               
                
            }
        }

        

        public DebugFocusedElementProxy()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += (o, ea) =>
            {
                this.FocusedElement = Keyboard.FocusedElement as UIElement;
            };
            timer.Start();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        void OnNotifyPropertyChanged(string nomPropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nomPropriete));
        }
        #endregion
    }
}
