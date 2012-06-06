using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
   public class CompatibilityPolicy:ExtraPropertiesViewModelBase
    {
       public CompatibilityPolicy(PackageReader reader)
       {
           Minimum = reader.GetRulesPropertyValueByName("compatability-policy", "minimum");
           Maximum = reader.GetRulesPropertyValueByName("compatability-policy", "maximum");
           Versions = reader.GetRulesPropertyValueByName("compatability-policy", "versions");

          
       }

       private string _minimum;
       public string Minimum
       {
           get { return _minimum; }
           set
           {
               _minimum = value;
               OnPropertyChanged("Minimum");
           }
       }

       private string _maximum;

       public string Maximum
       {
           get { return _maximum; }
           set
           {
               _maximum = value;
               OnPropertyChanged("Maximum");
           }
       }

       private string _versions;

       public string Versions
       {
           get { return _versions; }
           set
           {
               _versions = value;
               OnPropertyChanged("Versions");
           }
       }

       

       

       
    }
}
