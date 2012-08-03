using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoAppPackageMaker.ViewModels.Base
{
    public class Error : ViewModelBase
    {
        public string ErrorHeader { get; set; }
        public string ErrorDetails { get; set; }
        public string ErrorRule { get; set; }

        public string Icon
        {
            get { return "Icons\\eventlogError.ico"; }
        }
    }

    public class Warning : Error
    {
        public new string Icon
        {
            get { return "Icons\\eventlogWarn.ico"; }
        }

    }
}
