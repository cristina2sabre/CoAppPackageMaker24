using System;
using System.Collections.ObjectModel;
using System.Linq;
using CoApp.Developer.Toolkit.Scripting.Languages.PropertySheet;
using CoApp.Packaging;
using CoAppPackageMaker;
using CoAppPackageMaker.ViewModels;
using CoAppPackageMaker.ViewModels.RuleViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{
    /// <summary>
    ///This is a test class for PackageReaderTest and is intended
    ///to contain all PackageReaderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PackageReaderTest
    {
        private static PackageReader _reader;

        #region Additional test attributes
      
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            _reader = new PackageReader {PackageSource = new PackageSource()};
            
        }
      
        #endregion


        /// <summary>
        ///A test for GetDefineRules
        ///</summary>
        [TestMethod()]
        public void GetDefineRulesTest()
        {
            var rule2 = _reader.PackageSource.GetRule("*", id: "define");
            PropertyRule propertyRule2 = rule2.GetRuleProperty("arch");
            var propertyValue2 = propertyRule2.GetPropertyValue("");
            propertyValue2.SourceValues = new string[] { "any" };

            ObservableCollection<BaseItemViewModel> actual = _reader.GetDefineRules();

            Assert.AreEqual("define", actual[0].RuleNameToDisplay);
            Assert.AreEqual("any", ((DefineItem) actual[0]).SourceValue);
            Assert.AreEqual("any", ((DefineItem) actual[0]).Value);
            Assert.AreEqual(1, actual.Count);
        }

        /// <summary>
        ///A test for GetFilesRulesPropertyValueByParameterAndName
        ///</summary>
        [TestMethod()]
        public void GetFilesRulesPropertyValueByParameterAndNameTest()
        {
            var rule = _reader.PackageSource.GetRule("files", "A");
            PropertyRule propertyRule = rule.GetRuleProperty("trim-path");
            var propertyValue = propertyRule.GetPropertyValue("");
            propertyValue.SourceValues = new string[] { "minimal" };

            string  actual = _reader.GetFilesRulesPropertyValueByParameterAndName("A", "trim-path");

            Assert.AreEqual("minimal", actual);
        }

        /// <summary>
        ///A test for GetRulesByNameForPackageComposition
        ///</summary>
        [TestMethod()]
        public void GetRulesByNameForPackageCompositionTest()
        {
            //NOT USED YET
            //PackageReader target = new PackageReader(); // TODO: Initialize to an appropriate value
            //string ruleName = string.Empty; // TODO: Initialize to an appropriate value
            //string propertyName = string.Empty; // TODO: Initialize to an appropriate value
            //string attributeName = string.Empty; // TODO: Initialize to an appropriate value
            //bool isSource = false; // TODO: Initialize to an appropriate value
            //string expected = string.Empty; // TODO: Initialize to an appropriate value
            //string actual;
            //actual = target.GetRulesByNameForPackageComposition(ruleName, propertyName, attributeName, isSource);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetRulesByNameForSigning
        ///</summary>
        [TestMethod()]
        public void GetRulesByNameForSigningTest()
        {
            //not reading from the define in the tests..like ${arch}=any
            var rule = _reader.PackageSource.GetRule("signing");
            PropertyRule propertyRule = rule.GetRuleProperty("attributes");
            var propertyValue = propertyRule.GetPropertyValue("company");
            string newValue = "CoApp";
            propertyValue.SourceValues = new string[] {newValue};
            
            string sourse = _reader.GetRulesByNameForSigning("signing", "attributes", "company", true);
            string value = _reader.GetRulesByNameForSigning("signing", "attributes", "company", false);
            Assert.AreEqual(sourse, newValue);
            Assert.AreEqual(value, newValue);
       
        }

        /// <summary>
        ///A test for GetRulesByParamater
        ///</summary>
        [TestMethod()]
        public void GetRulesByParameterTest()
        {
            var rule = _reader.PackageSource.GetRule("developer-library", "A");
            PropertyRule propertyRule = rule.GetRuleProperty("headers");
            var propertyValue = propertyRule.GetPropertyValue("");
            propertyValue.SourceValues = new string[] { "abc","2" };

            var rule2 = _reader.PackageSource.GetRule("developer-library", "B");
            PropertyRule propertyRule2 = rule2.GetRuleProperty("docs");
            var propertyValue2 = propertyRule2.GetPropertyValue("");
            propertyValue2.SourceValues = new string[] { "abcd" };

            ObservableCollection<BaseItemViewModel> actual = _reader.GetRulesByParameter("A", "headers", "developer-library", typeof(DeveloperLibraryItem));

            Assert.AreEqual(2, actual.Count);
            var item= (DeveloperLibraryItem)actual[0];
            Assert.AreEqual("A", item.Parameter);
            Assert.AreEqual("headers", item.CollectionName);
            Assert.AreEqual("abc", item.SourceValue);
            Assert.AreEqual("abc", item.Value);

            item = (DeveloperLibraryItem)actual[1];
            Assert.AreEqual("A", item.Parameter);
            Assert.AreEqual("headers", item.CollectionName);
            Assert.AreEqual("2", item.SourceValue);
            Assert.AreEqual("2", item.Value);
        }

        /// <summary>
        ///A test for GetRulesPropertyValueByName
        ///</summary>
        [TestMethod()]
        public void GetRulesPropertyValueByNameTest()
        {
            var rule = _reader.PackageSource.GetRule("package");
            PropertyRule propertyRule = rule.GetRuleProperty("name");
            var propertyValue = propertyRule.GetPropertyValue("");
            propertyValue.SourceValues = new string[] {"TestPackage"};

            string actual = _reader.GetRulesPropertyValueByName("package", "name");

            Assert.AreEqual("TestPackage", actual);
        }

        /// <summary>
        ///A test for GetRulesSourcePropertyValueByName
        ///</summary>
        [TestMethod()]
        public void GetRulesSourcePropertyValueByNameTest()
        {
            var rule = _reader.PackageSource.GetRule("package");
            PropertyRule propertyRule = rule.GetRuleProperty("name");
            var propertyValue = propertyRule.GetPropertyValue("");
            propertyValue.SourceValues = new string[] { "TestPackage" };
           
            string actual = _reader.GetRulesSourcePropertyValueByName("package", "name");

            Assert.AreEqual("TestPackage", actual);
        }

        /// <summary>
        ///A test for GetRulesSourceStringPropertyValueByName
        ///</summary>
        [TestMethod()]
        public void GetRulesSourceStringPropertyValueByNameTest()
        {
            var rule = _reader.PackageSource.GetRule("package");
            PropertyRule propertyRule = rule.GetRuleProperty("name");
            var propertyValue = propertyRule.GetPropertyValue("");
            propertyValue.SourceValues = new string[] {"TestPackage"};

            string actual = _reader.GetRulesSourceStringPropertyValueByName("package");
            string expected = "package {\r\n    name : TestPackage;\r\n};\r\n\r\n";

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetRulesSourceValuesByNameForEditableCollections
        ///</summary>
        [TestMethod()]
        public void GetRulesSourceValuesByNameForEditableCollectionsTest()
        {
            var rule = _reader.PackageSource.GetRule("requires");
            PropertyRule propertyRule = rule.GetRuleProperty("package");
            var propertyValue = propertyRule.GetPropertyValue("");
            propertyValue.SourceValues=new string[]{"item1","item2","item3"};

            var actual = _reader.GetRulesSourceValuesByNameForEditableCollections("requires", "package", typeof(RequireItem));

            Assert.AreEqual(3, actual.Count);

            var item = (RequireItem)actual[1];
            Assert.AreEqual(String.Empty, item.Parameter);
            Assert.AreEqual(actual, item.Collection);
            Assert.AreEqual("package", item.CollectionName);
            Assert.AreEqual("item2", item.SourceValue);
            Assert.AreEqual("item2", item.Value);
           
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [TestMethod()]
        public void ReadTest()
        {
            //MessageBox is shown..
            //string pathToSourceFile = string.Empty; 
            //_reader.Read(pathToSourceFile);
            ////sucesfull read flag should be false
            //Assert.IsFalse(_reader.SuccesfullRead);
        }

        /// <summary>
        ///A test for ReadParameters
        ///</summary>
        [TestMethod()]
        public void ReadParametersTest()
        {
            //create files rule with parameters A,B,C
            var  rule=_reader.PackageSource.GetRule("files", "A");
            _reader.PackageSource.GetRule("files", "B");
            _reader.PackageSource.GetRule("files", "C");
            PropertyRule propertyRule = rule.GetRuleProperty("trim-path");
            var propertyValue = propertyRule.GetPropertyValue("");
            propertyValue.SourceValues = new string[] { "minimal" };

     
            var actual = _reader.ReadParameters("files");
            Assert.AreEqual(3, actual.Count);
            Assert.IsTrue(actual.Contains("A"));
            Assert.IsTrue(actual.Contains("B"));
            Assert.IsTrue(actual.Contains("C"));
        }

        /// <summary>
        ///A test for RemoveFromList
        ///</summary>
        [TestMethod()]
        public void RemoveFromListTest()
        {
            var rule = _reader.PackageSource.GetRule("requires");
            PropertyRule propertyRule = rule.GetRuleProperty("package");
            var propertyValue = propertyRule.GetPropertyValue("");
            propertyValue.SourceValues = new string[] { "item1", "item2", "item3" };

           _reader.RemoveFromList("requires", "package", "item2");

           Assert.AreEqual(2, propertyValue.SourceValues.Count());
           Assert.IsTrue(propertyValue.SourceValues.Contains("item1"));
           Assert.IsTrue(propertyValue.SourceValues.Contains("item3"));
        }

        /// <summary>
        ///A test for RemoveRulesWithParameters
        ///</summary>
        [TestMethod()]
        public void RemoveRulesWithParametersTest()
        {
            var rule = _reader.PackageSource.GetRule("manifest", "glib");
            PropertyRule propertyRule = rule.GetRuleProperty("include");
            var propertyValue = propertyRule.GetPropertyValue("");
            propertyValue.SourceValues = new string[] { "test", "test2", "test3" };
            _reader.RemoveRulesWithParameters("manifest", "glib", "include", "test");

            Assert.AreEqual(2, propertyValue.SourceValues.Count());
            Assert.IsTrue(propertyValue.SourceValues.Contains("test2"));
            Assert.IsTrue(propertyValue.SourceValues.Contains("test3"));
           
        }

        /// <summary>
        ///A test for Save
        ///</summary>
        [TestMethod()]
        public void SaveTest()
        {
            //CANNOT TEST
            //PackageReader target = new PackageReader(); // TODO: Initialize to an appropriate value
            //string destinationFilename = string.Empty; // TODO: Initialize to an appropriate value
            //target.Save(destinationFilename);
            //Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetNewParameter
        ///</summary>
        [TestMethod()]
        public void SetNewParameterTest()
        {
            //create manifest[glib] with include
            var rule = _reader.PackageSource.GetRule("manifest", "glib");
            PropertyRule propertyRule = rule.GetRuleProperty("include");
            var propertyValue = propertyRule.GetPropertyValue("");
            propertyValue.SourceValues = new string[] { "test", "test2", "test3" };

            int countBefore = _reader.ReadParameters("manifest").Count;
            Assert.IsTrue(_reader.ReadParameters("manifest").Contains("glib"));
            _reader.SetNewParameter("manifest", "glib", "hello");
            int countAfter = _reader.ReadParameters("manifest").Count;
            Assert.AreEqual(countBefore, countAfter);
            Assert.IsTrue(_reader.ReadParameters("manifest").Contains("hello"));
        }

        /// <summary>
        ///A test for SetNewSourceValue
        ///</summary>
        [TestMethod()]
        public void SetNewSourceValueTest()
        {
           
            //Test1 - set new sourse value that doesn't exist and will be added when called GetRule()
            Assert.AreEqual(0, _reader.PackageSource.CompatabilityPolicyRules.Count()); 

            string actual = _reader.SetNewSourceValue("compatability-policy", "minimum", "newValue");

            Assert.AreEqual("newValue", actual);
            Assert.AreEqual(1, _reader.PackageSource.CompatabilityPolicyRules.Count());

            //Test2 - set new sourse value for exiting rule
            //create the rule
            var rule = _reader.PackageSource.GetRule("files", "A");
            PropertyRule propertyRule = rule.GetRuleProperty("trim-path");
            var propertyValue = propertyRule.GetPropertyValue("");
            propertyValue.SourceValues = new string[] { "minimal" };

            Assert.AreEqual(1, _reader.PackageSource.FileRules.Count());
            actual = _reader.SetNewSourceValue("files", "trim-path", "newValue","A");

            Assert.AreEqual("newValue", propertyValue.SourceValues.FirstOrDefault());
            Assert.AreEqual(1, _reader.PackageSource.FileRules.Count());
        }

        /// <summary>
        ///A test for SetRulesWithParameters
        ///</summary>
        [TestMethod()]
        public void SetRulesWithParametersTest()
        {
            _reader = new PackageReader { PackageSource = new PackageSource() };
            //Test1 - set new sourse value that doesn't exist and will be added when called GetRule()
            Assert.AreEqual(0, _reader.PackageSource.ApplicationRules.Count());

            string actual = _reader.SetRulesWithParameters("application", "include", null, "newValue", "A");

            Assert.AreEqual("newValue", actual);
            Assert.AreEqual(1, _reader.PackageSource.ApplicationRules.Count());
            Assert.AreEqual(1, _reader.PackageSource.AllRoles.Count());

            //Test2 - set new sourse value for exiting rule and updating  nonexistent item(so it will be generated and added to the collection)
            var rule = _reader.PackageSource.GetRule("files", "A");
            PropertyRule propertyRule = rule.GetRuleProperty("include");
            var propertyValue = propertyRule.GetPropertyValue("");
            propertyValue.SourceValues = new string[] {"1", "2", "3"};


            Assert.AreEqual(3, propertyValue.SourceValues.Count());
            _reader.SetRulesWithParameters("files", "include", null, "newValue", "A");
            Assert.AreEqual(4, propertyValue.SourceValues.Count());

            //Test3 - set new sourse value for exiting rule and updating  existent item
            _reader.SetRulesWithParameters("files", "include", "1", "newValue", "A");
            Assert.AreEqual("newValue", propertyValue.SourceValues.FirstOrDefault());
            Assert.AreEqual(4, propertyValue.SourceValues.Count());
            Assert.IsTrue(propertyValue.SourceValues.Contains("newValue"));
            Assert.IsTrue(propertyValue.SourceValues.Contains("2"));
            Assert.IsTrue(propertyValue.SourceValues.Contains("3"));
        }
    }


}
