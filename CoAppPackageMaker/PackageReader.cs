using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
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
           _packageSource = new PackageSource();
            _packageSource.SourceFile =pathToSourceFile;
      
            try
            {
                
               _packageSource.LoadPackageSourceData(_packageSource.SourceFile, "#");
              PropertyRule a= _packageSource.AllRoles.GetRulesByName("faux-pax").GetProperty("downloads");
           
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

                IEnumerable<string> resdult =
                    _packageSource.FileRules.GetRulesByParameter(rule.Parameter).GetPropertyValues("include");
                List<string> sfs = resdult.ToList();
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
