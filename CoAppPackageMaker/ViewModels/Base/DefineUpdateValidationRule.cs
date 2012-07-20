using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using CoAppPackageMaker.ViewModels.Base;

namespace CoAppPackageMaker.Views
{
    //http://www.codeproject.com/KB/WPF/AttachingVirtualBranches.aspx -about thhe bridge
    //http://karlshifflett.wordpress.com/mvvm/input-validation-ui-exceptions-model-validation-errors/ -about validation
    public class DefineUpdateValidationRule : ValidationRule
    {

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
            var validationResult = new ValidationResult(false, null);
            //int searchListSize = MainWindowViewModel.Instance.SearchList.Count();
            //if (searchListSize > 0)
            //{
            //    BindingExpression bindingExpression = (value as BindingExpression);
            //    if (bindingExpression != null)
            //    {
            //        string bindingName = bindingExpression.ParentBinding.Path.Path;
            //        validationResult = MainWindowViewModel.Instance.SearchList.Contains(bindingName)
            //                               ? new ValidationResult(false, null)
            //                               : new ValidationResult(true, null);
            //    }
            //}

            return validationResult;
        }

    }
}