﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoAppPackageMaker.ViewModels
{
 public abstract class ExtraPropertiesViewModelBase:ViewModelBase
    {
        private string _helpTip;
        private bool _isRequired;

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
    }
}
