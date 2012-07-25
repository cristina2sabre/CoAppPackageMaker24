using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using CoAppPackageMaker.ViewModels.Base;
using System.Text.RegularExpressions;

namespace CoAppPackageMaker.Views
{
    //http://www.codeproject.com/KB/WPF/AttachingVirtualBranches.aspx -about thhe bridge
    //http://karlshifflett.wordpress.com/mvvm/input-validation-ui-exceptions-model-validation-errors/ -about validation
    public class PackageValidationRule : ValidationRule
    {
       static Dictionary<string,string> _errorsMessages =new Dictionary<string, string>();
       static PackageValidationRule()
       {
           _errorsMessages["Version"] = "Must be a 4-part version string:  a.b.c.d ";
           _errorsMessages["Architecture"] = "arch: [x86 | x64 | any];    /* Any particular package should be for exactly ONE of these architectures.";
           
       }

        /// <summary>
        /// Evaluate first if Binding expression and DataForBinding is not null
        /// Check if it present in the collection-set on false or true
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        ///     
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var validationResult = new ValidationResult(true, null);
            var bindingExpression = (value as BindingExpression);
            if (bindingExpression != null)
            {
                string errorHeader = bindingExpression.ParentBinding.Path.Path;
                MethodInfo method = typeof(PackageValidationRule).GetMethod(errorHeader);
                if ((bool)method.Invoke(this,null))
                {
                    //check if the error exist in the list
                    var existingErros =
                        MainWindowViewModel.Instance.ErrorsCollection.Where(item => item.ErrorHeader == errorHeader);
                    if (!existingErros.Any())
                    {
                        MainWindowViewModel.Instance.ErrorsCollection.Add(new Error()
                                                                              {
                                                                                  ErrorHeader =
                                                                                      bindingExpression.
                                                                                      ParentBinding.Path.Path,
                                                                                  ErrorDetails =_errorsMessages[errorHeader]
                                                                                      
                                                                              });
                        MainWindowViewModel.Instance.PackageViewModel.StatusColor = Colors.Red;

                    }
                    //MainWindowViewModel.Instance.PackageViewModel.StatusColor = Colors.Green;
                    return new ValidationResult(false, null);

                }
                MainWindowViewModel.Instance.RemoveError(errorHeader);
            }
            return validationResult;
        }

     

        public bool Version()
        {
            var sd = MainWindowViewModel.Instance.PackageViewModel.ValuePackageViewModel.GetType().GetProperty("Version").GetValue(MainWindowViewModel.Instance.PackageViewModel.ValuePackageViewModel, null).ToString();
            return (sd.Split('.').Count() != 4 ||
                    sd.Split('.').ToList().Any(item => item.Length == 0));


        }

        public bool Architecture()
        {
           string sd = MainWindowViewModel.Instance.PackageViewModel.ValuePackageViewModel.GetType().GetProperty("Architecture").GetValue(MainWindowViewModel.Instance.PackageViewModel.ValuePackageViewModel, null).ToString();
           return !(sd.Equals("x86") | sd.Equals("x64") | sd.Equals("any"));
                


        }
       
    }
}