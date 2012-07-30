using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using CoAppPackageMaker.ViewModels.Base;
using CoAppPackageMaker.ViewModels.RuleViewModels;
using CoAppPackageMaker.Views;

namespace CoAppPackageMaker.ValidationRules
{
    public class DefineValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var validationResult = new ValidationResult(true, null);
            var dataItem = (value as BindingExpression).DataItem;
            if (dataItem != null)
            {
                var label = (dataItem as DefineItem).Label;
                var sourceValue = (dataItem as DefineItem).SourceValue;
                string errorHeader =label;
               
                if (sourceValue.Contains("${" + label + "}"))
                {
                    //check if the error exist in the list
                    var existingErros =
                        MainWindowViewModel.Instance.ErrorsCollection.Where(item => item.ErrorHeader == errorHeader);
                    if (!existingErros.Any())
                    {
                        MainWindowViewModel.Instance.ErrorsCollection.Add(new Error()
                                                                              {
                                                                                  ErrorHeader = label,
                                                                                  ErrorDetails ="Never set a variable by self refernce! Doing so will cause autopackage to loop forever trying to resolve it.",
                                                                                  ErrorRule = "Define"

                                                                              });
                        MainWindowViewModel.Instance.DefineViewModel.StatusColor = Colors.Red;
                    }
                    return new ValidationResult(false, null);
                }

                var errorStatus =
                    MainWindowViewModel.Instance.ErrorsCollection.Where(item => item.ErrorRule == "Define");
                if (errorStatus.Any())
                {
                    MainWindowViewModel.Instance.DefineViewModel.StatusColor = Colors.Green;
                    
                }
                MainWindowViewModel.Instance.RemoveError(errorHeader);
            }
            return validationResult;
        }
    }
}
