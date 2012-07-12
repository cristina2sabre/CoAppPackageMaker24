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
using CoAppPackageMaker.ViewModels.Base;
using CoAppPackageMaker.ViewModels.RuleViewModels;

namespace CoAppPackageMaker
{
    public class PackageReader
    {

        public void Read(string pathToSourceFile)
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

        public void Save(string destinationFilename)
        {
            _packageSource.SavePackageFile(destinationFilename);
        }

        private PackageSource _packageSource;
        //public PackageSource PackageSource
        //{
        //    get { return _packageSource; }

        //}


        public string SetMMM(string parameter, IEnumerable<string> value)
        {
            //IEnumerable<Rule> rules = _packageSource.Rules.GetRulesByName("manifest");
            //IEnumerable<string> r = (rules != null) ? rules.GetRulesByParameter(parameter).GetPropertyValues("include") : new string[0];

            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName("manifest").GetProperty("manifest", "assembly");
            if (propertyRule != null)
            {
                foreach (PropertyValue propertyValue in propertyRule.PropertyValues)
                {

                    foreach (string s in propertyValue.SourceValues)
                    {
                        propertyValue.SourceValues = value;
                        PropertyRule p = _packageSource.AllRules.GetRulesByName("requires").GetProperty("requires", "assembly");
                        // result = p.Values.First();
                    }
                }
            }

            //foreach (string str in r)

            {

                //if (propertyRule != null)
                //{
                //    PropertyValue propertyValue = propertyRule.PropertyValues.FirstOrDefault();

                //    if (propertyValue != null)
                //    {
                //        propertyValue.SourceValues = value;
                //        // SearchAll(propertyName); from MainViewModel
                //        return propertyRule.Value;
                //    }
                //}
                //PropertyRule propertyRule = rule["include"];

                //if (propertyRule != null)
                //{
                //   // IEnumerable<string> r = (rules != null) ? rules.GetRulesByParameter(parameter).GetPropertyValues(propertyName) : new string[0];
                //    PropertyValue propertyValue = propertyRule.PropertyValues.FirstOrDefault();

                //    if (propertyValue != null)
                //    {
                //        propertyValue.SourceValues = value;
                //        // SearchAll(propertyName); from MainViewModel
                //        return propertyRule.Value;
                //    }
                //}
            }
            return String.Empty;
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
                        // SearchAll(propertyName); from MainViewModel
                        return propertyRule.Value;
                    }
                }
            }
            return String.Empty;
        }

        public ObservableCollection<ItemViewModel> GetDefineRules()
        {
            var result = new ObservableCollection<ItemViewModel>();
            foreach (Rule rule in _packageSource.DefineRules)
            {
                foreach (string propertyName in rule.PropertyNames)
                {
                    var model = new ItemViewModel();
                    PropertyRule propertyRule = rule[propertyName];
                    model.Label = propertyName;
                    var firstOrDefault = propertyRule.PropertyValues.FirstOrDefault();
                    if (firstOrDefault != null)
                    {
                        model.Reader = this;
                        model.UpdateSource = SetSourceDefineRules;
                        model.SourceValue = firstOrDefault.SourceValues.FirstOrDefault();
                        //model.Value = propertyRule.Value;//no need to set-is evaluated in the sourcevalue;
                    }

                    result.Add(model);
                }

            }
            return result;
        }

        public ObservableCollection<ItemViewModel> GetRulesSourceValuesByParameterForEditableCollections(string ruleName, string parameter, string propertyName)
        {
            var result = new ObservableCollection<ItemViewModel>();
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
            if (propertyRule != null)
            {
                foreach (PropertyValue propertyValue in propertyRule.PropertyValues)
                {
                    int count = propertyValue.SourceValues.Count();
                    for (int i = 0; i < count; i++)
                    {
                        var model = new ItemViewModel();
                        model.Reader = this;
                        model.Label = parameter;
                        model.Index = i;
                        //  model.UpdateSource = SetMiFINAL;
                        model.SourceValue = propertyValue.SourceValues.ToList()[i];
                        //  model.Value = propertyValue.Values.ToList()[i];
                        result.Add(model);


                    }

                }
            }


            return result;
        }

        public ObservableCollection<ItemViewModel> GetRulesSourceValuesByNameForEditableCollections(string ruleName, string propertyName)
        {
            var result = new ObservableCollection<ItemViewModel>();
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
            if (propertyRule != null)
            {
                foreach (PropertyValue propertyValue in propertyRule.PropertyValues)
                {

                    foreach (string sourceValue in propertyValue.SourceValues)
                    {
                        var model = new ItemViewModel();
                        model.Reader = this;
                        model.UpdateSource = SelectUpdateMethod(ruleName);
                        model.SourceValue = sourceValue;
                        // model.Value = no need to set-is evaluated in the sourcevalue;
                        result.Add(model);
                    }
                }
            }

            return result;
        }


        private ItemViewModel.Process SelectUpdateMethod(string ruleName)
        {
            ItemViewModel.Process result = null;
            switch (ruleName)
            {
                case "requires":
                    result = SetSourceRequireRules;
                    break;
                case "signing":
                    result = SetSourceSingingRules;
                    break;
            }

            return result;
        }

        //used
        public string GetRulesByNameForSigning(string ruleName, string propertyName, string attributeName, bool isSource)
        {
            IEnumerable<string> result = new string[0];
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
            if (propertyRule != null)
            {
                PropertyValue propertyValue = propertyRule.PropertyValues.Single((item => item.Label == attributeName));
                result = isSource ? propertyValue.SourceValues : propertyValue.Values;
            }

            return result.FirstOrDefault();
        }

        public string SetMiFINAL(string parameter, IEnumerable<string> value)
        {

            string result = String.Empty;
            IEnumerable<Rule> hRules = _packageSource.AllRules.GetRulesByName("manifest");
            PropertyRule propertyRule = null;
            foreach (Rule hRule in hRules)
            {
                if (hRule.Parameter == parameter)
                {
                    propertyRule = hRule["assembly"];
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
            }




            return result;




        }


        public ObservableCollection<ItemViewModel> GETMiFINAL(string parameter, IEnumerable<string> value)
        {
            var result = new ObservableCollection<ItemViewModel>();

            IEnumerable<Rule> hRules = _packageSource.AllRules.GetRulesByName("manifest");
            PropertyRule propertyRule = null;
            foreach (Rule hRule in hRules)
            {
                if (parameter == hRule.Parameter)
                {
                    propertyRule = hRule["assembly"];
                    if (propertyRule != null)
                    {
                        PropertyValue propertyValue = propertyRule.PropertyValues.FirstOrDefault();

                        if (propertyValue != null)
                        {
                            foreach (string sourceValue in propertyValue.SourceValues)
                            {
                                var model = new ItemViewModel();
                                model.Reader = this;
                                model.Label = parameter;
                                model.UpdateSource = SetMiFINAL;
                                model.SourceValue = sourceValue;
                                // model.Value = no need to set-is evaluated in the sourcevalue;
                                result.Add(model);
                            }

                        }
                    }

                }
            }




            return result;




        }

        public string SetSourceRequireRules(string parameter, IEnumerable<string> value)
        {
            string result = String.Empty;
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName("requires").GetProperty("requires", "package");
            if (propertyRule != null)
            {
                foreach (PropertyValue propertyValue in propertyRule.PropertyValues)
                {

                    foreach (string s in propertyValue.SourceValues)
                    {
                        propertyValue.SourceValues = value;
                        PropertyRule p = _packageSource.AllRules.GetRulesByName("requires").GetProperty("requires", "package");
                        result = p.Values.First();
                    }
                }
            }


            return result;
        }

        public string SetSourceSingingRules(string parameter, IEnumerable<string> value)
        {
            string result = String.Empty;
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName("signing").GetProperty("signing", "include");
            if (propertyRule != null)
            {
                foreach (PropertyValue propertyValue in propertyRule.PropertyValues)
                {

                    foreach (string s in propertyValue.SourceValues)
                    {
                        propertyValue.SourceValues = value;
                        PropertyRule p = _packageSource.AllRules.GetRulesByName("signing").GetProperty("signing", "include");
                        result = p.Values.First();
                    }
                }
            }


            return result;
        }


        public string SetSourceMAnifestIncludeRules(string parameter, IEnumerable<string> value)
        {
            string result = String.Empty;
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName("manifest").GetProperty("manifest", "include");
            if (propertyRule != null)
            {
                foreach (PropertyValue propertyValue in propertyRule.PropertyValues)
                {

                    foreach (string s in propertyValue.SourceValues)
                    {
                        propertyValue.SourceValues = value;
                        PropertyRule p = _packageSource.AllRules.GetRulesByName("manifest").GetProperty("manifest", "include");
                        result = p.Values.First();
                    }
                }
            }


            return result;
        }

        public string SetSourceManifestRules(string ruleName, IEnumerable<string> value)
        {
            //to mofify !!!!!!!!!!!!is for required
            string result = String.Empty;
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName("manifest").GetProperty("manifest", "assembly");
            if (propertyRule != null)
            {
                foreach (PropertyValue propertyValue in propertyRule.PropertyValues)
                {

                    foreach (string s in propertyValue.SourceValues)
                    {
                        propertyValue.SourceValues = value;
                        PropertyRule p = _packageSource.AllRules.GetRulesByName("manifest").GetProperty("manifest", "assembly");
                        result = p.Values.First();
                    }
                }
            }


            return result;
        }

        //used
        public string GetRulesSourcePropertyValueByName(string ruleName, string propertyName)
        {
            string result = String.Empty;
            var rules = _packageSource.AllRules.GetRulesByName(ruleName);
            PropertyRule propertyRule = rules.GetProperty(ruleName, propertyName);
            if (propertyRule != null)
            {
                PropertyValue propertyValue = propertyRule.PropertyValues.FirstOrDefault();
                result = (propertyValue != null) ? propertyValue.SourceValues.FirstOrDefault() : String.Empty;

            }
            return result;
        }

        //used
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

        //used
        public string SetNewSourceValueSigning(string ruleName, string propertyName, string attributeName, IEnumerable<string> value)
        {
            IEnumerable<string> result = new string[0];
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
            if (propertyRule != null)
            {
                propertyRule.PropertyValues.First((item => item.Label == attributeName)).SourceValues = value;

            }
            result = propertyRule.PropertyValues.Single((item => item.Label == attributeName)).Values;
            return result.FirstOrDefault();
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

        public List<string> ReadParameters(string ruleName)
        {
            var result = new List<string>();
            var rules = _packageSource.AllRules.GetRulesByName(ruleName);
            foreach (Rule rule in rules)
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


        public ObservableCollection<ItemViewModel> ApplicationIncludeList(string ruleName, string parameter, string propertyName)
        {
            var result = new ObservableCollection<ItemViewModel>();
            IEnumerable<Rule> rules = _packageSource.AllRules.GetRulesByName(ruleName);
            IEnumerable<string> r = (rules != null) ? rules.GetRulesByParameter(parameter).GetPropertyValues(propertyName) : new string[0];

            {
                foreach (string value in r)
                {
                    var model = new ItemViewModel();
                    model.Reader = this;

                    //  model.UpdateSource = SetSourceRequireRules;
                    model.SourceValue = value;
                    // model.Root = root;
                    result.Add(model);

                }
            }

            return result;
        }

        public ObservableCollection<String> Rules
        {
            get
            {
                var rules = new ObservableCollection<string>();
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
                var roles = new ObservableCollection<string>();
                foreach (var rule in _packageSource.AllRoles)
                {
                    roles.Add(rule.Name);
                }
                return roles;
            }
        }

    }
}
