using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using CoApp.Developer.Toolkit.Scripting.Languages.PropertySheet;
using CoApp.Packaging;
using CoAppPackageMaker.ViewModels.RuleViewModels;

namespace CoAppPackageMaker
{
   public  class PackageReader
    {
        private PackageSource _packageSource;
        public  void Read(string pathToSourceFile)
        {
            try
            {
                _packageSource = new PackageSource(pathToSourceFile, new Dictionary<string, string>());
            }

            catch (Exception exception)
            {
               MessageBox.Show(exception.ToString());
            }

        }
       
      

        public IEnumerable<string> GetRulesSourcePropertyValuesByName(string ruleName, string propertyName)
        {
            IEnumerable<string> result = new string[0];
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
            if(propertyRule!=null)
            {
                foreach (PropertyValue propertyValue in propertyRule.PropertyValues)
                {
                    result = propertyValue.SourceValues;

                }
            }
           
            return result;
        }

        public string GetRulesSourcePropertyValuesByNameForSigning(string ruleName, string propertyName, string attributeName)
        {
            IEnumerable<string> result = new string[0];
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
            if (propertyRule != null)
            {
                result = propertyRule.PropertyValues.Single((item => item.Label == attributeName)).SourceValues;
               
            }
        
            return result.FirstOrDefault();
        }

        public ObservableCollection<DefineViewModel.DefineItemViewModel> GetDefineRules(DefineViewModel defineViewModel)
        {
            var  result=new ObservableCollection<DefineViewModel.DefineItemViewModel>();
           foreach (Rule rule in _packageSource.DefineRules)
           {
             
               foreach (string s in rule.PropertyNames)
               {
                   var model = new DefineViewModel.DefineItemViewModel();
                   PropertyRule propertyRule = rule[s];
                   model.Label = s;
                   var firstOrDefault = propertyRule.PropertyValues.FirstOrDefault();
                   if (firstOrDefault != null)
                   {
                       model.Value = propertyRule.Value;
                       model.SourceValue = firstOrDefault.SourceValues.FirstOrDefault();
                       model.Root = defineViewModel.Root;
                       model.Reader = this;
                     
                   }
                      
                   result.Add(model);
               }
              
           }
            return result;
            
       }

        public string SetSourceDefineRules(string propertyName, IEnumerable<string> value)
        {

            foreach (Rule rule in _packageSource.DefineRules)
            {
              
                    PropertyRule propertyRule = rule[propertyName];
                 
                    if (propertyRule != null)
                    {
                     PropertyValue propertyValue = propertyRule.PropertyValues.FirstOrDefault();

                        if (propertyValue != null)
                        {
                            propertyValue.SourceValues = value;
                            return propertyRule.Value;
                        }
                       
                    }
            }
            return String.Empty;
        }

       public string PropertyRuleValue(Rule rule,string name)
       {
           PropertyRule w = rule[name];
           string er = w.Value;
           return er;
       }


       public string SetNewSourceValueSigning(string ruleName, string propertyName,string attributeName, IEnumerable<string> value)
        {
            IEnumerable<string> result = new string[0];
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
            if (propertyRule != null)
            {
               propertyRule.PropertyValues.First((item => item.Label == attributeName)).SourceValues=value;

            }
            result = propertyRule.PropertyValues.Single((item => item.Label == attributeName)).Values; 
            return  result.FirstOrDefault();
        }
       
          
        public string GetRulesPropertyValuesByNameForSigning(string ruleName, string propertyName, string attributeName)
        {
            IEnumerable<string> result = new string[0];
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
            if (propertyRule != null)
            {
                result = propertyRule.PropertyValues.Single((item => item.Label == attributeName)).Values;

            }
            //  result = (propertyValue != null) ? propertyValue.SourceValues.FirstOrDefault() : String.Empty;
            return result.FirstOrDefault();
        }

        public string GetRulesSourcePropertyValueByName(string ruleName, string propertyName)
        {
            string result = String.Empty;
            var rules = _packageSource.AllRules.GetRulesByName(ruleName);
            PropertyRule propertyRule = rules.GetProperty(ruleName, propertyName);
            if(propertyRule!=null)
            {
                PropertyValue propertyValue = propertyRule.PropertyValues.FirstOrDefault();
                result=(propertyValue != null) ? propertyValue.SourceValues.FirstOrDefault() : String.Empty;

              //  IEnumerable<string> resudlt = new string[0];
              //  propertyValue.SourceValues =  resudlt;
            }
            return result;    
        }

        public string GetRulesPropertyValueByName(string ruleName, string propertyName)
        {
            var rules = _packageSource.AllRules.GetRulesByName(ruleName);
            return (rules != null) ? rules.GetPropertyValue(propertyName) : String.Empty;


        }

        public string SetNewSourceValue(string ruleName, string propertyName, IEnumerable<string> value)
       {
           var rules = _packageSource.AllRules.GetRulesByName(ruleName);
           PropertyRule propertyRule = rules.GetProperty(ruleName, propertyName);
           if (propertyRule != null)
           {
               PropertyValue propertyValue = propertyRule.PropertyValues.FirstOrDefault();

               if (propertyValue != null)
               {

                   propertyValue.SourceValues = value;
               }

           }
           return (rules != null) ? rules.GetPropertyValue(propertyName) : String.Empty;
       }

       public void SetRulesSourceProperty(string ruleName, string propertyName, IEnumerable<string> value)
        {
            
            var rules = _packageSource.AllRules.GetRulesByName(ruleName);
            PropertyRule propertyRule = rules.GetProperty(ruleName, propertyName);
            if (propertyRule != null)
            {
                PropertyValue propertyValue = propertyRule.PropertyValues.FirstOrDefault();

                if(propertyValue!=null)
                {
                   
                 propertyValue.SourceValues = value;
                }
                
            }
            
        }

    
        public string GetRulesSourceStringPropertyValueByName(string ruleName)
        {

            IEnumerable<Rule> rules = _packageSource.AllRules.GetRulesByName(ruleName);
            StringBuilder result = new StringBuilder();
            foreach (Rule rule in rules)
            {
               
                result.Append(rule.SourceString);
            }


            return result.ToString();

        }

        public string GetRolesPropertyValueByName(string roleName, string propertyName)
        {

            string result = _packageSource.AllRoles.GetRulesByName(roleName).GetPropertyValue(propertyName);
            
            return result;
        }


        public IEnumerable<string> GetRolesPropertyValues(string roleName, string propertyName)
        {

            IEnumerable<string> result = _packageSource.AllRoles.GetRulesByParameter(roleName).GetPropertyValues(propertyName);
            return result;
        }

        public IEnumerable<string> GetRulesPropertyValues(string ruleName, string propertyName)
        {
            IEnumerable<string> result;
            IEnumerable<Rule> rulesByName = _packageSource.AllRules.GetRulesByName(ruleName);
            result =(rulesByName!=null)? rulesByName.GetPropertyValues(propertyName):new string[0];
            return result;
        }

        public List<string> ReadFilesParameters()
        {
            List<string> result=new List<string>();
            var fileRules = _packageSource.FileRules;
            foreach (Rule rule in fileRules)
            {
                string ruleParameter = rule.Parameter;
                result.Add(ruleParameter);  
            }

            return result;
        }


        public List<string> ReadPackageCompositionParameters()
        {
            List<string> result = new List<string>();
            var packageComposition = _packageSource.PackageCompositionRules;
            foreach (Rule rule in packageComposition)
            {
                string ruleParameter = rule.Parameter;
                result.Add(ruleParameter);
            }

            return result;
        }
        public List<string> ReadManifestParameters()
        {
            List<string> result = new List<string>();
            var fileRules = _packageSource.ManifestRules;
            foreach (Rule rule in fileRules)
            {
                string ruleParameter = rule.Parameter;
                result.Add(ruleParameter);
            }

            return result;
        }

        public List<string> ReadSinging()
        {
            List<string> result = new List<string>();
            var fileRules = _packageSource.DefineRules;
            foreach (Rule rule in fileRules)
            {
                string ruleParameter = rule.Parameter;
                result.Add(ruleParameter);
            }

            return result;
        }

        public string GetFilesRulesPropertyValueByParameterAndName(string parameter, string propertyName)
        {
            return _packageSource.FileRules.GetRulesByParameter(parameter).GetPropertyValue(propertyName);
        }

        public IEnumerable<string> GetFilesIncludeList(string parameter)
        {
            IEnumerable<string> result = _packageSource.FileRules.GetRulesByParameter(parameter).GetPropertyValues("include");
            return result;
        }
        public IEnumerable<string> GetManifestIncludeList(string parameter, string propertyName)
        {
            IEnumerable<Rule> rules = _packageSource.ManifestRules;
            IEnumerable<string> result = (rules!=null) ?rules.GetRulesByParameter(parameter).GetPropertyValues(propertyName):null;
            return result;
        }

        public IEnumerable<string> GetManifestIncludeList2(string parameter, string propertyName)
        {

            IEnumerable<string> result = null;
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName(parameter).GetProperty(parameter, propertyName);

            foreach (PropertyValue propertyValue in propertyRule.PropertyValues)
            {
                result = propertyValue.SourceValues;

            }
            return result;
              
        }

        public ObservableCollection<String> Rules
        {
             
            get
            {
                ObservableCollection<string> rules=new ObservableCollection<string>();
                foreach (var rule in _packageSource.AllRules)
                {
                   rules.Add(rule.Name);
                }
                return rules;

            }
        
        }

        public ObservableCollection<String> Roles
        {

            get
            {
                ObservableCollection<string> roles = new ObservableCollection<string>();
                foreach (var rule in _packageSource.AllRoles)
                {
                    roles.Add(rule.Name);
                }
                return roles;

            }

        }


    }
}
