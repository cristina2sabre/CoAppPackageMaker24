using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
   public class DefineViewModel:ExtraPropertiesViewModelBase
 
   {
    public DefineViewModel(PackageReader reader)
    {
        string define = "define";

        Flavor = reader.GetRulesPropertyValueByName(define, "flavor");
        Architecure = reader.GetRulesPropertyValueByName(define, "arch");
    }

    private string _flavor;

    public string Flavor
    {
        get { return _flavor; }
        set
        {
            _flavor = value;
            OnPropertyChanged("Flavor");
        }
    }

       private string _architecture;

       public string Architecure
       {
           get { return _architecture; }
           set
           {
               _architecture = value;
               OnPropertyChanged("Architecure");
           }
       }

       

}
}
