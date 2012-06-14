using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
   public class DefineViewModel:ExtraPropertiesViewModelBase
 
   {

       public DefineViewModel()
       {

       }


       public DefineViewModel(PackageReader reader)
       {
        string define = "*";
        Flavor = reader.GetRulesPropertyValueByName(define, "flavor");
        Architecture = reader.GetRulesPropertyValueByName(define, "arch");
        SourceString = reader.GetRulesSourceStringPropertyValueByName("*");
        IsEditable = true;
        _sourceValueDefineViewModel = new DefineViewModel();
        _sourceValueDefineViewModel.Architecture = reader.GetRulesSourcePropertyValueByName("*", "arch" );
        _sourceValueDefineViewModel.Flavor = reader.GetRulesSourcePropertyValueByName("*", "flavor");
           IsEditable = false;
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

       public string Architecture
       {
           get { return _architecture; }
           set
           {
               _architecture = value;
               OnPropertyChanged("Architecture");
           }
       }


       private DefineViewModel _sourceValueDefineViewModel;
       public DefineViewModel SourceValueDefineViewModel
       {
           get { return _sourceValueDefineViewModel; }
           set
           {
               _sourceValueDefineViewModel = value;
               OnPropertyChanged("SourceDefineViewModel");
           }
       }
       

}
}
