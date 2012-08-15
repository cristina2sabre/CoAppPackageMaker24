using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
    public class SigningViewModel : ExtraPropertiesViewModelBase
    {
        private readonly PackageReader _reader;
        private const string Signing = "signing";

        public SigningViewModel()
        {

        }



        public SigningViewModel(PackageReader reader)
        {
            _reader = reader;
            RuleNameToDisplay = "Signing";
            ValueSigningViewModel = new SigningViewModel()
                                        {
                                            ReplaceSignature =
                                                _reader.GetRulesPropertyValueByName(Signing, "replace-signature") ==
                                                "true",
                                            CompanyAttribute =
                                                reader.GetRulesByNameForSigning(Signing, "attributes", "company", false),
                                            DescriptionAttribute =
                                                reader.GetRulesByNameForSigning(Signing, "attributes", "description",
                                                                                false),
                                            ProductNameAttribute =
                                                reader.GetRulesByNameForSigning(Signing, "attributes", "product-name",
                                                                                false),
                                            ProductVersion =
                                                reader.GetRulesByNameForSigning(Signing, "attributes", "product-version",
                                                                                false),
                                            FileVersionAttribute =
                                                reader.GetRulesByNameForSigning(Signing, "attributes", "file-version",
                                                                                false),
                                            EditCollectionViewModel =
                                                new EditCollectionViewModel(
                                                                            reader.
                                                                                GetRulesSourceValuesByNameForEditableCollections
                                                                                (Signing, "include", typeof(SigningIncludeItem)), "include",Signing, typeof(SigningIncludeItem)),
                                          
                                            IsReadOnly = true,
                                            SourceString = reader.GetRulesSourceStringPropertyValueByName(Signing),
                                        };

            ReplaceSignature =
              _reader.GetRulesSourcePropertyValueByName(Signing, "replace-signature") ==
              "true";
            CompanyAttribute =
                reader.GetRulesByNameForSigning(Signing,
                                                "attributes",
                                                "company", true);
            DescriptionAttribute =
                reader.GetRulesByNameForSigning(Signing,
                                                "attributes",
                                                "description", true);
            ProductNameAttribute =
                reader.GetRulesByNameForSigning(Signing,
                                                "attributes",
                                                "product-name", true);
            ProductVersion =
                reader.GetRulesByNameForSigning(Signing,
                                                "attributes",
                                                "product-version", true);
            FileVersionAttribute =
                reader.GetRulesByNameForSigning(Signing,
                                                "attributes",
                                                "file-version", true);
            EditCollectionViewModel = ValueSigningViewModel.EditCollectionViewModel;
           
            IsSource = true;
           
            this.PropertyChanged += EvaluatedChanged;
        }

        #region Properties

        
        private SigningViewModel _valueSigningViewModel;
        public SigningViewModel ValueSigningViewModel
        {
            get { return _valueSigningViewModel; }
            set
            {
                _valueSigningViewModel = value;
                OnPropertyChanged("ValueSigningViewModel");
            }
        }

        private string _companyAttribute;
        public string CompanyAttribute
        {
            get { return _companyAttribute; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    DefaultChangeFactory.OnChanging(this, "CompanyAttribute", _companyAttribute, value);
                    _companyAttribute = value;
                    OnPropertyChanged("CompanyAttribute");
                }
            }
        }


        private string _descriptionAttribute;
        public string DescriptionAttribute
        {
            get { return _descriptionAttribute; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    DefaultChangeFactory.OnChanging(this, "DescriptionAttribute", _descriptionAttribute, value);
                    _descriptionAttribute = value;
                    OnPropertyChanged("DescriptionAttribute");
                }
            }
        }


        private string _productNameAttribute;
        public string ProductNameAttribute
        {
            get { return _productNameAttribute; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    DefaultChangeFactory.OnChanging(this, "ProductNameAttribute", _productNameAttribute, value);
                    _productNameAttribute = value;
                    OnPropertyChanged("ProductNameAttribute");
                }
            }
        }

        private string _productVersionAttribute;
        public string ProductVersion
        {
            get { return _productVersionAttribute; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    DefaultChangeFactory.OnChanging(this, "ProductVersion", _productVersionAttribute, value);
                    _productVersionAttribute = value;
                    OnPropertyChanged("ProductVersion");
                }
               
            }
        }

        private string _fileVersionAttribute;
        public string FileVersionAttribute
        {
            get { return _fileVersionAttribute; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    DefaultChangeFactory.OnChanging(this, "FileVersionAttribute", _fileVersionAttribute, value);
                    _fileVersionAttribute = value;
                    OnPropertyChanged("FileVersionAttribute");
                }
               
            }
        }

        private bool _replaceSignature;
        public bool ReplaceSignature
        {
            get { return _replaceSignature; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "ReplaceSignature", _replaceSignature, value);
                _replaceSignature = value;
                OnPropertyChanged("ReplaceSignature");
            }
        }
        
        #endregion
        
        public void EvaluatedChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case "CompanyAttribute":
                    //reevaluate
                    ValueSigningViewModel.CompanyAttribute = _reader.SetNewSourceValue(Signing, "attributes", CompanyAttribute,attributeName: "company");
                    break;
                case "DescriptionAttribute":
                    ValueSigningViewModel.DescriptionAttribute = _reader.SetNewSourceValue(Signing, "attributes", DescriptionAttribute, attributeName: "description");
                    break;
                case "ProductNameAttribute":
                    ValueSigningViewModel.ProductNameAttribute = _reader.SetNewSourceValue(Signing, "attributes", ProductNameAttribute, attributeName: "product-name");
                    break;
                case "ProductVersion":
                    ValueSigningViewModel.ProductVersion = _reader.SetNewSourceValue(Signing, "attributes", ProductVersion, attributeName: "product-version");
                    break;
                case "FileVersionAttribute":
                    ValueSigningViewModel.FileVersionAttribute = _reader.SetNewSourceValue(Signing, "attributes", FileVersionAttribute, attributeName: "file-version");
                    break;
                case "ReplaceSignature":
                    ValueSigningViewModel.ReplaceSignature =Convert.ToBoolean( _reader.SetNewSourceValue(Signing, "attributes", ReplaceSignature.ToString().ToLower(), attributeName: "replace-signature") );
                    break;
            }

            ValueSigningViewModel.SourceString = _reader.GetRulesSourceStringPropertyValueByName(Signing);
        }

       
    }

    public class SigningIncludeItem : BaseItemViewModel
    {
        public SigningIncludeItem(string ruleName, string collectionName)
        {
            RuleNameToDisplay = ruleName;
            CollectionName = collectionName;
            
        }

        public override string ProcessSourceValue(string newValue, string oldValue)
        {
            return MainWindowViewModel.Instance.Reader.SetRulesWithParameters("signing", "include", oldValue, newValue);
        }
    }
}
