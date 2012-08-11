using System.Collections.Generic;
using System.ComponentModel;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels
{
    public class CompatibilityPolicyViewModel : ExtraPropertiesViewModelBase
    {
       private readonly PackageReader _reader;
       private const string CompatibilityPolicy = "compatability-policy";
       public CompatibilityPolicyViewModel()
       {
       }

       public CompatibilityPolicyViewModel(PackageReader reader)
       {
           _reader = reader;
           RuleNameToDisplay = "Compatibility Policy";

           Minimum = reader.GetRulesSourcePropertyValueByName(CompatibilityPolicy, "minimum");
           Maximum = reader.GetRulesSourcePropertyValueByName(CompatibilityPolicy, "maximum");
           Versions = reader.GetRulesSourcePropertyValueByName(CompatibilityPolicy, "versions");
           IsSource = true;

           ValueCompatibilityPolicyViewModel = new CompatibilityPolicyViewModel()
                                                   {
                                                       Minimum =
                                                           reader.GetRulesPropertyValueByName(CompatibilityPolicy,
                                                                                              "minimum"),
                                                       Maximum =
                                                           reader.GetRulesPropertyValueByName(CompatibilityPolicy,
                                                                                              "maximum"),
                                                       Versions =
                                                           reader.GetRulesPropertyValueByName(CompatibilityPolicy,
                                                                                              "versions"),
                                                       IsReadOnly = true,
                                                        SourceString = reader.GetRulesSourceStringPropertyValueByName(CompatibilityPolicy),
                                                   };

          
           this.PropertyChanged += EvaluatedChanged;


       }

        #region Properties

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

       #endregion

       private CompatibilityPolicyViewModel _valueCompatibilityPolicyViewModel;
       public CompatibilityPolicyViewModel ValueCompatibilityPolicyViewModel
       {
           get { return _valueCompatibilityPolicyViewModel; }
           set
           {
               _valueCompatibilityPolicyViewModel = value;
               OnPropertyChanged("ValueCompatibilityPolicyViewModel");
           }
       }

       public void EvaluatedChanged(object sender, PropertyChangedEventArgs args)
       {
         
          
               switch (args.PropertyName)
               {
                   case "Minimum":
                        ValueCompatibilityPolicyViewModel.Minimum = _reader.SetNewSourceValue(CompatibilityPolicy, "minimum", Minimum);
                        break;
                   case "Maximum":
                       ValueCompatibilityPolicyViewModel.Maximum = _reader.SetNewSourceValue(CompatibilityPolicy, "maximum", Maximum);
                       break;
                   case "Versions":
                       ValueCompatibilityPolicyViewModel.Versions = _reader.SetNewSourceValue(CompatibilityPolicy, "versions", Versions);
                       break;

               }

               ValueCompatibilityPolicyViewModel.SourceString = _reader.GetRulesSourceStringPropertyValueByName(CompatibilityPolicy);
           }

       
     
       
    }
}
