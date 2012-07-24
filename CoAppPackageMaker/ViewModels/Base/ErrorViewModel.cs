using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoAppPackageMaker.ViewModels.Base
{
    public class Error : ViewModelBase
    {
        //private string _errorHeader;
        //public string ErrorHeader
        //{
        //    get { return _errorHeader; }
        //    set
        //    {
        //        _errorHeader = value;
        //        OnPropertyChanged("ErrorHeader");
        //    }
        //}
        public string ErrorHeader { get; set; }
        public string ErrorDetails { get; set; }

        public string Icon
        {
            get { return "Icons\\eventlogWarn.ico"; }
        }
    }

    public class Warning : Error
    {
       new  public string Icon
        {
            get { return "Icons\\eventlogError.ico"; }
        }

    }
}
