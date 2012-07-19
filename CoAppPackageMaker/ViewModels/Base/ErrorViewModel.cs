using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoAppPackageMaker.ViewModels.Base
{
    public class ErrorViewModel:ViewModelBase

{
    private string _errorHeader;

    public string ErrorHeader
    {
        get { return _errorHeader; }
        set
        {
            _errorHeader = value;
            OnPropertyChanged("ErrorHeader");
        }
    }

    public string ErrorDetails { get; set; }
}
}
