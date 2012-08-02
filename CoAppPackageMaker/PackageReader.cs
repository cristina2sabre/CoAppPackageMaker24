using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using CoApp.Developer.Toolkit.Scripting.Languages.PropertySheet;
using CoApp.Packaging;
using CoAppPackageMaker.ViewModels;
using CoAppPackageMaker.ViewModels.RuleViewModels;

namespace CoAppPackageMaker
{
    public class PackageReader
    {
        public bool SusscesfullRead = true;
        private PackageSource _packageSource;

        public void Read(string pathToSourceFile)
        {
            try
            {
                _packageSource = new PackageSource(pathToSourceFile, new Dictionary<string, string>());
            }

            catch (Exception exception)
            {
                SusscesfullRead = false;
                MessageBox.Show(exception.Message);
            }

        }

        public void Save(string destinationFilename)
        {
            _packageSource.SavePackageFile(destinationFilename);
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

        public ObservableCollection<BaseItemViewModel> GetDefineRules()
        {
            var result = new ObservableCollection<BaseItemViewModel>();
            foreach (Rule rule in _packageSource.DefineRules)
            {
                foreach (string propertyName in rule.PropertyNames)
                {
                    var model = new DefineItem();
                    PropertyRule propertyRule = rule[propertyName];
                    model.Label = propertyName;
                    var propertyValue = propertyRule.PropertyValues.FirstOrDefault();
                    if (propertyValue != null)
                    {
                        //model.Reader = this;
                        model.SourceValue = propertyValue.SourceValues.FirstOrDefault();
                    }
                    result.Add(model);
                }
            }
            return result;
        }

      public ObservableCollection<BaseItemViewModel> GetRulesSourceValuesByNameForEditableCollections(string ruleName, string propertyName)
        {
            var result = new ObservableCollection<BaseItemViewModel>();
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
            if (propertyRule != null)
            {
                foreach (PropertyValue propertyValue in propertyRule.PropertyValues)
                {
                    int i = -1;
                    foreach (string sourceValue in propertyValue.SourceValues)
                    {
                        i ++;
                        if(ruleName=="signing")
                        {
                          var model  = new SigningViewModel.SigningIncludeItem();
                          //model.Index = i;
                          //model.Reader = this;
                          model.SourceValue = sourceValue;
                          // model.Value = no need to set-is evaluated in the sourcevalue;
                          result.Add(model);
                        }
                        else
                        {
                          var  model2 = new RequiresViewModel.RequireItem();
                          //model2.Index = i;
                          //model2.Reader = this;
                          model2.SourceValue = sourceValue;
                          // model.Value = no need to set-is evaluated in the sourcevalue;
                          result.Add(model2);
                        }
                       
                    }
                }
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


        //public string SetSourceRequireSigningRules(string ruleName, string propertyName, string oldValue, string newValue)
        //{
        //    string result = String.Empty;
        //    PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
        //    if (propertyRule != null)
        //    {
        //        PropertyValue propertyValue = propertyRule.PropertyValues.ToList()[0];
        //        List<string> tempList = propertyValue.SourceValues.ToList();
        //        int index = tempList.Count();

        //        if (tempList.Contains(oldValue))
        //        {
        //            index = tempList.IndexOf(oldValue);
        //            tempList[index] = newValue;
        //        }
        //        else if (tempList.Contains(newValue))
        //        {
        //            index = tempList.IndexOf(newValue);
        //        }
        //        else if (!tempList.Contains(newValue))
        //        {
        //            tempList.Add(newValue);
        //            index = tempList.Count() - 1;
        //        }


        //        propertyValue.SourceValues = tempList;
        //        PropertyRule p = _packageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
        //        result = p.Values.ToList()[index];
        //    }
        //    return result;
        //}

        public string SetRulesWithParameters(string ruleName, string parameter, string colectionName,string oldValue, string newValue)
        {
            string result = String.Empty;
            IEnumerable<Rule> rules = _packageSource.AllRules.GetRulesByName(ruleName);
            Rule rule = rules.FirstOrDefault(item => item.Parameter == parameter);
            if (rule != null)
            {
                PropertyRule propertyRule = rule[colectionName];
                //e la fel ca in restul
                   if (propertyRule != null)
                   {
                       
                       PropertyValue propertyValue = propertyRule.PropertyValues.ToList()[0];
                       List<string> tempList = propertyValue.SourceValues.ToList();
                       int index = tempList.Count();

                       if (tempList.Contains(oldValue))
                       {
                           index = tempList.IndexOf(oldValue);
                           tempList[index] = newValue;
                       }
                       else if (tempList.Contains(newValue))
                       {
                           index = tempList.IndexOf(newValue);
                       }
                       else if (!tempList.Contains(newValue))
                       {
                           tempList.Add(newValue);
                           index = tempList.Count() - 1;
                       }


                       propertyValue.SourceValues = tempList;
                       //PropertyRule p = _packageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
                       result = propertyRule.Values.ToList()[index];
               }
            }

            return result;

        }

        public ObservableCollection<BaseItemViewModel> GetRulesByParamater(string parameter, string propertyName, string ruleName,Type itemType)
        {
            var result = new ObservableCollection<BaseItemViewModel>();
            IEnumerable<Rule> rules = _packageSource.AllRules.GetRulesByName(ruleName);
            //Rule rulew = rules.FirstOrDefault().Parent.GetRule(null,parameter,null,null);

            Rule rule = rules.FirstOrDefault(item => item.Parameter == parameter);

            if (rule != null)
            {
                PropertyRule propertyRule = rule[propertyName];
                if(propertyRule!=null)
                {

                    //sa iau doar primul??
                    PropertyValue propertyValue = propertyRule.PropertyValues.FirstOrDefault();
                    if (propertyValue != null)
                    {
                        //int i = -1;
                        foreach (string sourceValue in propertyValue.SourceValues)
                        {
                            //i++;
                            var newItem = Activator.CreateInstance(itemType);
                            //((BaseItemViewModel)newItem).Index = i;
                            ((BaseItemViewModel)newItem).Parameter = parameter;
                            ((BaseItemViewModel)newItem).CollectionName = propertyName;
                            ((BaseItemViewModel)newItem).RuleNameToDisplay = ruleName;
                            ((BaseItemViewModel)newItem).SourceValue = sourceValue;
                           
                            ((BaseItemViewModel)newItem).upper = result;

                            result.Add((BaseItemViewModel)newItem);

                        }
                    }
                }
               
           
            }
           

            return result;
        }


        public string SetSourceRequireSigningRules(string ruleName,string propertyName, string oldValue, string newValue)
        {
            string result = String.Empty;
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
            if (propertyRule != null)
            {
                PropertyValue propertyValue = propertyRule.PropertyValues.ToList()[0];
                List<string> tempList = propertyValue.SourceValues.ToList();
                int index = tempList.Count();

                if (tempList.Contains(oldValue))
                {
                    index = tempList.IndexOf(oldValue);
                    tempList[index] = newValue;
                }
                else if (tempList.Contains(newValue))
                {
                    index = tempList.IndexOf(newValue);
                }
                else if (!tempList.Contains(newValue))
                {
                    tempList.Add(newValue);
                    index = tempList.Count() - 1;
                }


                propertyValue.SourceValues = tempList;
                PropertyRule p = _packageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
                result = p.Values.ToList()[index];
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

        public string SetNewParameter(string ruleName, string oldValue, string newValue)
        {
            var rule = _packageSource.AllRules.GetRulesByName(ruleName).FirstOrDefault(item => item.Parameter == oldValue);
            if (rule!=null)
            {
                return rule.Parameter = newValue;
            }
            return newValue;
        }

        public string GetFilesRulesPropertyValueByParameterAndName(string parameter, string propertyName)
        {
           return _packageSource.FileRules.GetRulesByParameter(parameter).GetPropertyValue(propertyName);
        }

        public string SetFiles(string parameter,string propertyName,string newValue)
        {
            IEnumerable<Rule> rules = _packageSource.AllRules.GetRulesByName("files");
            Rule rule = rules.FirstOrDefault(item => item.Parameter == parameter);
            //if rule does not exist to add
            if (rule != null)
            {
                PropertyRule propertyRule = rule[propertyName];
                if (propertyRule != null)
                {
                   propertyRule.PropertyValues.FirstOrDefault().Value = newValue;
                }
                return _packageSource.FileRules.GetRulesByParameter(parameter).GetPropertyValue(propertyName);
            }
            //Temporary solution - if rule[parameter] doesnt exist(usually for new added files[]) just return the same value
            return newValue;
           
        }


       

       
    }
}
