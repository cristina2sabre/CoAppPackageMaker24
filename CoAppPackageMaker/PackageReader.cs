using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using CoApp.Developer.Toolkit.Scripting.Languages.PropertySheet;
using CoApp.Packaging;
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
                   
                    PropertyRule propertyRule = rule[propertyName];
                    //propertyRule.HasValues
                    if (propertyRule!=null)
                    {
                        var model = new DefineItem("define","");
                        model.Label = propertyName;
                        var propertyValue = propertyRule.PropertyValues.FirstOrDefault();
                        if (propertyValue != null)
                        {
                            //model.RuleNameToDisplay = "define";
                            model.SourceValue = propertyValue.SourceValues.FirstOrDefault();
                        }
                        result.Add(model);
                        
                    }
                   
                }
            }
            return result;
        }

        public ObservableCollection<BaseItemViewModel> GetRulesSourceValuesByNameForEditableCollections(string ruleName, string propertyName, Type type)
        {
            var result = new ObservableCollection<BaseItemViewModel>();
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName,
                                                                                                     propertyName);
            if (propertyRule != null)
            {
                foreach (PropertyValue propertyValue in propertyRule.PropertyValues)
                {

                    foreach (string sourceValue in propertyValue.SourceValues)
                    {
                       
                        
                           
                            object newItem = Activator.CreateInstance(type, ruleName, propertyName);
                           ((BaseItemViewModel) newItem).SourceValue=sourceValue;
                            result.Add((BaseItemViewModel)newItem);
                   

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


        //used
        public string GetRulesByNameForPackageComposition(string ruleName, string propertyName, string attributeName, bool isSource)
        {
            IEnumerable<string> result = new string[0];
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
            if (propertyRule != null)
            {
                PropertyValue propertyValue = propertyRule.PropertyValues.FirstOrDefault();
                result = isSource ? propertyValue.SourceValues : propertyValue.Values;
            }

            return result.FirstOrDefault();
        }


        public void RemoveRulesWithParameters(string ruleName, string parameter, string colectionName, string toRemove)
        {
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
                    tempList.Remove(toRemove);
                    propertyValue.SourceValues = tempList;
                }
            }
        }

        public string SetRulesWithParameters(string ruleName, string parameter, string colectionName, string oldValue, string newValue)
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
                    //update
                    if (tempList.Contains(oldValue))
                    {
                        index = tempList.IndexOf(oldValue);
                        tempList[index] = newValue;
                    }
                    //is used when is loaded 
                    else if (tempList.Contains(newValue))
                    {
                        index = tempList.IndexOf(newValue);
                    }
                        //add
                    else if (!tempList.Contains(newValue))
                    {
                        tempList.Add(newValue);
                        index = tempList.Count() - 1;
                    }


                    propertyValue.SourceValues = tempList;
                    result = propertyRule.Values.ToList()[index];
                }
            }

            return result;

        }

        public ObservableCollection<BaseItemViewModel> GetRulesByParamater(string parameter, string propertyName, string ruleName,Type itemType)
        {
            var result = new ObservableCollection<BaseItemViewModel>();
            IEnumerable<Rule> rules = _packageSource.AllRules.GetRulesByName(ruleName);
            Rule rule = rules.FirstOrDefault(item => item.Parameter == parameter);

            if (rule != null)
            {
                PropertyRule propertyRule = rule[propertyName];
                if(propertyRule!=null)
                {
                   
                    PropertyValue propertyValue = propertyRule.PropertyValues.FirstOrDefault();
                    if (propertyValue != null)
                    {
                      
                        foreach (string sourceValue in propertyValue.SourceValues)
                        {
                            var newItem = Activator.CreateInstance(itemType, ruleName, propertyName);
                            ((BaseItemViewModel)newItem).Parameter = parameter;
                            //((BaseItemViewModel)newItem).CollectionName = propertyName;
                            //((BaseItemViewModel)newItem).RuleNameToDisplay = ruleName;
                            ((BaseItemViewModel)newItem).SourceValue = sourceValue;
                            ((BaseItemViewModel)newItem).Collection = result;
                            result.Add((BaseItemViewModel)newItem);
                        }
                    }
                }
            }

            return result;
        }

        public void RemoveFromList(string ruleName,string propertyName, string toRemove)
        {
           
            PropertyRule propertyRule = _packageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
            if (propertyRule != null)
            {
                PropertyValue propertyValue = propertyRule.PropertyValues.ToList()[0];
                List<string> tempList = propertyValue.SourceValues.ToList();
                if (tempList.Contains(toRemove))
                {

                    tempList.Remove(toRemove);
                }

                propertyValue.SourceValues = tempList;
            }
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

            var rul = _packageSource.GetRule(id:"define");
            var rule = rul[propertyName];
            var rule2s = _packageSource.AllRules.GetRulesByName(ruleName);
            PropertyRule propertyRulet = rule2s.GetProperty(ruleName, propertyName);

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
                result = propertyRule.PropertyValues.Single((item => item.Label == attributeName)).Values;
            }
           
            return result.FirstOrDefault();
        }


        public string GetRulesSourceStringPropertyValueByName(string ruleName)
        {
            IEnumerable<Rule> rules = _packageSource.AllRules.GetRulesByName(ruleName);
            var result = new StringBuilder();
            foreach (Rule rule in rules)
            {
                result.Append(rule.SourceString);
            }
            return result.ToString();
        }

        public List<string> ReadParameters(string ruleName)
        {
            var rules = _packageSource.AllRules.GetRulesByName(ruleName);

            return rules.Select(rule => rule.Parameter).ToList();
        }

        public string SetNewParameter(string ruleName, string oldValue, string newValue)
        {
            if(oldValue=="[]")
            {
                oldValue = null;
            }
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
