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

           SourceString = reader.GetRulesSourceStringPropertyValueByName(compatibilityPolicy);
           _sourceValueCompatibilityPolicyViewModel = new SourceCompatibilityPolicyViewModel(reader);
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

       private SourceCompatibilityPolicyViewModel _sourceValueCompatibilityPolicyViewModel;
       public SourceCompatibilityPolicyViewModel SourceValueCompatibilityPolicyViewModel
       {
           get { return _sourceValueCompatibilityPolicyViewModel; }
           set
           {
               _sourceValueCompatibilityPolicyViewModel = value;
               OnPropertyChanged("SourceCompatibilityPolicyViewModel");
           }
       }
       

       public class SourceCompatibilityPolicyViewModel:CompatibilityPolicyViewModel
       {
            public SourceCompatibilityPolicyViewModel(PackageReader reader)
            {
                string compatibilityPolicy = "compatability-policy";
                Minimum = reader.GetRulesSourcePropertyValueByName(compatibilityPolicy, "minimum");
                Maximum = reader.GetRulesSourcePropertyValueByName(compatibilityPolicy, "maximum");
                Versions = reader.GetRulesSourcePropertyValueByName(compatibilityPolicy, "versions");
            }
       }
       
    }
}
