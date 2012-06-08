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

namespace CoAppPackageMaker
{
   public  class PackageReader
    {
        private PackageSource _packageSource;
        public  void Read(string pathToSourceFile)
        {
            _packageSource = new PackageSource(pathToSourceFile,new Dictionary<string, string>());
            _packageSource.SourceFile =pathToSourceFile;
      
            try
            {
               // IEnumerable<string> result = _packageSource.AllRoles.GetRulesByName("").GetPropertyValues("");
                List<string> result = new List<string>();
                var fileRules = _packageSource.PackageRules;
                foreach (Rule rule in fileRules)
                {
                 IEnumerable<string> resulet=   rule.GetDynamicMemberNames();
                    foreach (string fileRule in rule.PropertyNames)
                    {
                        
                    }

                   
                    //foreach (PropertyValue value in rule.PropertyValues)
                    //{
                    //   string gf= value.Label;
                    //    IEnumerable<string> wdw=  value.SourceValues;
                    //}

                }

              
              // _packageSource.LoadPackageSourceData(_packageSource.SourceFile);
              PropertyRule a= _packageSource.AllRoles.GetRulesByName("package").GetProperty("location");
              
                Console.WriteLine(a);
            }
            catch (Exception exception)
            {

               MessageBox.Show(exception.ToString());
            }

        
        }
       
        public string GetRulesPropertyValueByName(string ruleName,string propertyName)
        {

            string result = _packageSource.AllRules.GetRulesByName(ruleName).GetPropertyValue(propertyName);
            return result;
        }

        public IEnumerable<string> GetRulesSourcePropertyValuesByName(string ruleName, string propertyName)
        {
            IEnumerable<string> result = null;
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);

            foreach (PropertyValue propertyValue in propertyRule.PropertyValues)
            {
                result = propertyValue.SourceValues;
            }
            return result;
        }

        public string GetRulesSourcePropertyValueByName(string ruleName, string propertyName)
        {
            
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
         PropertyValue propertyValue=  propertyRule.PropertyValues.FirstOrDefault();
         //IEnumerable<string> items = new string[3];

         //items.ToList().Add(("msg2"));
         //propertyValue.SourceValues = items;

            return (propertyValue != null) ? propertyValue.SourceValues.FirstOrDefault() : String.Empty;
           
        }

        public string GetRulesSourceStringPropertyValueByName(string ruleName, string propertyName)
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

            IEnumerable<string> result = _packageSource.AllRules.GetRulesByName(ruleName).GetPropertyValues(propertyName);
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
          
            IEnumerable<string> result = _packageSource.ManifestRules.GetRulesByParameter(parameter).GetPropertyValues(propertyName);
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
