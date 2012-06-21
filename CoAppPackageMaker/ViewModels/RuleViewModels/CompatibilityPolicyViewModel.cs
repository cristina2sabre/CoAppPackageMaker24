using System.Collections.Generic;
using System.ComponentModel;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels
{
   public class CompatibilityPolicyViewModel:ExtraPropertiesViewModelBase
    {
       private PackageReader _reader;
       private const string CompatibilityPolicy = "compatability-policy";
       public CompatibilityPolicyViewModel()
       {
       }

       public CompatibilityPolicyViewModel(PackageReader reader)
       {
           _reader = reader;
          
           Minimum = reader.GetRulesPropertyValueByName(CompatibilityPolicy, "minimum");
           Maximum = reader.GetRulesPropertyValueByName(CompatibilityPolicy, "maximum");
           Versions = reader.GetRulesPropertyValueByName(CompatibilityPolicy, "versions");
           IsEditable = false;
           SourceValueCompatibilityPolicyViewModel = new CompatibilityPolicyViewModel()
                                                       {
                                                            Minimum = reader.GetRulesSourcePropertyValueByName(CompatibilityPolicy, "minimum"),
                Maximum = reader.GetRulesSourcePropertyValueByName(CompatibilityPolicy, "maximum"),
                Versions = reader.GetRulesSourcePropertyValueByName(CompatibilityPolicy, "versions"),
                IsEditable = true
                                                       };

           SourceString = reader.GetRulesSourceStringPropertyValueByName(CompatibilityPolicy);

           SourceValueCompatibilityPolicyViewModel.PropertyChanged += EvaluatedChanged;

           
       }

       private string _minimum;
       public string Minimum
       {
           get { return _minimum; }
           set
           {
               DefaultChangeFactory.OnChanging(this, "Minimum", _minimum, value);
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
               DefaultChangeFactory.OnChanging(this, "Maximum", _maximum, value);
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
               DefaultChangeFactory.OnChanging(this, "Versions", _versions, value);
               _versions = value;
               OnPropertyChanged("Versions");
           }
       }

       private CompatibilityPolicyViewModel _sourceValueCompatibilityPolicyViewModel;
       public CompatibilityPolicyViewModel SourceValueCompatibilityPolicyViewModel
       {
           get { return _sourceValueCompatibilityPolicyViewModel; }
           set
           {
               _sourceValueCompatibilityPolicyViewModel = value;
               OnPropertyChanged("SourceCompatibilityPolicyViewModel");
           }
       }

       public void EvaluatedChanged(object sender, PropertyChangedEventArgs args)
       {
           IEnumerable<string> newValues;
           {
               switch (args.PropertyName)
               {
                   case "Minimum":
                        
                        newValues= new[]{((CompatibilityPolicyViewModel)sender).Minimum};
                        Minimum = _reader.SetNewSourceValue(CompatibilityPolicy, "minimum", newValues);
                       break;
                   case "Maximum":
                       Maximum = ((CompatibilityPolicyViewModel)sender).Maximum;
                       break;
                   case "Versions":
                       Versions = ((CompatibilityPolicyViewModel)sender).Versions;
                       break;

               }

           }

       }
     
       
    }
}
