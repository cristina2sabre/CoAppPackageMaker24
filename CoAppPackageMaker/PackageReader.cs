using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CoApp.Developer.Toolkit.Scripting.Languages.PropertySheet;
using CoApp.Packaging;
using CoAppPackageMaker.ViewModels.RuleViewModels;

namespace CoAppPackageMaker
{
    public class PackageReader
    {
        public bool SuccesfullRead = true;
        public PackageSource PackageSource=new PackageSource();
       
        public void Read(string pathToSourceFile)
        {
            try
            {
                PackageSource = new PackageSource(pathToSourceFile, new Dictionary<string, string>());
            }

            catch (Exception exception)
            {
                SuccesfullRead = false;
                MessageBox.Show(exception.Message); 
              
            }

        }

        public void Save(string destinationFilename)
        {
            PackageSource.SavePackageFile(destinationFilename);
        }

        public ObservableCollection<BaseItemViewModel> GetDefineRules()
        {
            var result = new ObservableCollection<BaseItemViewModel>();
            foreach (Rule rule in PackageSource.DefineRules)
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
            PropertyRule propertyRule = PackageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName,
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
                            ((BaseItemViewModel)newItem).Collection = result;
                   

                    }
                }
            }

            return result;
        }

        //used
        public string GetRulesByNameForSigning(string ruleName, string propertyName, string attributeName, bool isSource)
        {
            IEnumerable<string> result = new string[0];
            PropertyRule propertyRule = PackageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
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
            PropertyRule propertyRule = PackageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
            if (propertyRule != null)
            {
                PropertyValue propertyValue = propertyRule.PropertyValues.FirstOrDefault();
                result = isSource ? propertyValue.SourceValues : propertyValue.Values;
            }

            return result.FirstOrDefault();
        }


        public void RemoveRulesWithParameters(string ruleName, string parameter, string colectionName, string toRemove)
        {
            IEnumerable<Rule> rules = PackageSource.AllRules.GetRulesByName(ruleName);
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
        

        public ObservableCollection<BaseItemViewModel> GetRulesByParameter(string parameter, string propertyName, string ruleName, Type itemType)
        {
            var result = new ObservableCollection<BaseItemViewModel>();
            IEnumerable<Rule> rules = PackageSource.AllRules.GetRulesByName(ruleName);
            Rule rule = rules.FirstOrDefault(item => item.Parameter == parameter);

            if (rule != null)
            {
                PropertyRule propertyRule = rule[propertyName];
                if (propertyRule != null)
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
        
        //used
        public string GetRulesSourcePropertyValueByName(string ruleName, string propertyName)
        {
            string result = String.Empty;
            var rules = PackageSource.AllRules.GetRulesByName(ruleName);

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
            var rules = PackageSource.AllRules.GetRulesByName(ruleName);
            return (rules != null) ? rules.GetPropertyValue(propertyName) : String.Empty;
        }

        public string GetRulesSourceStringPropertyValueByName(string ruleName)
        {
            IEnumerable<Rule> rules = PackageSource.AllRules.GetRulesByName(ruleName);
            var result = new StringBuilder();
            foreach (Rule rule in rules)
            {
                result.Append(rule.SourceString);
            }
            return result.ToString();
        }

        public List<string> ReadParameters(string ruleName)
        {
            var rules = PackageSource.AllRules.GetRulesByName(ruleName);

            return rules.Select(rule => rule.Parameter).ToList();
        }

        public string GetFilesRulesPropertyValueByParameterAndName(string parameter, string propertyName)
        {
         
           return PackageSource.FileRules.GetRulesByParameter(parameter).GetPropertyValue(propertyName);
        }


        public string SetRulesWithParameters(string ruleName, string propertyName, string oldValue, string newValue, string parameter = null)
        {
            string result = String.Empty;

           {
                var rule = PackageSource.GetRule(ruleName, parameter);
                PropertyRule propertyRule = rule.GetRuleProperty(propertyName);
                var propertyValue = propertyRule.GetPropertyValue("");

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


            return result;

        }

        public void SetNewParameter(string ruleName, string oldValue, string newValue)
        {
            var rule = PackageSource.GetRule(ruleName, oldValue);
            rule.Parameter = newValue;

        }

        public string SetNewSourceValue(string ruleName, string propertyName, string newValue, string parameter = null, string attributeName = "", string id = null)
        {
            var rule = PackageSource.GetRule(ruleName, parameter, id: id);
            PropertyRule propertyRule = rule.GetRuleProperty(propertyName);
            var propertyValue = propertyRule.GetPropertyValue(attributeName);
            propertyValue.SourceValues = new string[] { newValue };
            return propertyValue.Value;
        }

        //public void RemoveFromRulesWithParameters(string ruleName, string parameter)
        //{
        //    var rule = _packageSource.GetRule(ruleName, parameter);
        //   _packageSource
        //}

        public void RemoveFromList(string ruleName, string propertyName, string toRemove)
        {

            PropertyRule propertyRule = PackageSource.AllRules.GetRulesByName(ruleName).GetProperty(ruleName, propertyName);
            if (propertyRule != null)
            {
                PropertyValue propertyValue = propertyRule.PropertyValues.FirstOrDefault();
                List<string> tempList = propertyValue.SourceValues.ToList();
                if (tempList.Contains(toRemove))
                {

                    tempList.Remove(toRemove);
                }

                propertyValue.SourceValues = tempList;
            }
        }

       
    }
}
