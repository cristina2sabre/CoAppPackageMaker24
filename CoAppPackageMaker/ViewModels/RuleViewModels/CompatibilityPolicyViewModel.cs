using System.ComponentModel;

namespace CoAppPackageMaker.ViewModels
{
   public class CompatibilityPolicyViewModel:ExtraPropertiesViewModelBase
    {
       public CompatibilityPolicyViewModel()
       {
       }

       public CompatibilityPolicyViewModel(PackageReader reader)
       {
           string compatibilityPolicy = "compatability-policy";
           Minimum = reader.GetRulesPropertyValueByName(compatibilityPolicy, "minimum");
           Maximum = reader.GetRulesPropertyValueByName(compatibilityPolicy, "maximum");
           Versions = reader.GetRulesPropertyValueByName(compatibilityPolicy, "versions");
           IsEditable = false;
           SourceValueCompatibilityPolicyViewModel = new CompatibilityPolicyViewModel()
                                                       {
                                                            Minimum = reader.GetRulesSourcePropertyValueByName(compatibilityPolicy, "minimum"),
                Maximum = reader.GetRulesSourcePropertyValueByName(compatibilityPolicy, "maximum"),
                Versions = reader.GetRulesSourcePropertyValueByName(compatibilityPolicy, "versions"),
                IsEditable = true
                                                       };

           SourceString = reader.GetRulesSourceStringPropertyValueByName(compatibilityPolicy);

           SourceValueCompatibilityPolicyViewModel.PropertyChanged += EvaluatedChanged;

           
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
           {
               switch (args.PropertyName)
               {
                   case "Minimum":
                       Minimum = ((CompatibilityPolicyViewModel)sender).Minimum;
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
