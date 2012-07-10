using System.Collections.Generic;
using System.ComponentModel;
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
            ReplaceSignature = _reader.GetRulesPropertyValueByName(Signing, "replace-signature") == "true";
            CompanyAttribute = reader.GetRulesByNameForSigning(Signing, "attributes", "company",false);
            DescriptionAttribute = reader.GetRulesByNameForSigning(Signing, "attributes", "description",false);
            ProductNameAttribute = reader.GetRulesByNameForSigning(Signing, "attributes", "product-name",false);
            ProductVersion = reader.GetRulesByNameForSigning(Signing, "attributes", "product-version",false);
            FileVersionAttribute = reader.GetRulesByNameForSigning(Signing, "attributes", "file-version",false);
            EditCollectionViewModel = new EditCollectionViewModel(reader, reader.GetRulesSourceValuesByNameForEditableCollections(Signing, "include"));
            IsEditable = false;
            IsReadOnly = true;

            SourceSigningViewModel = new SigningViewModel()
                                         {
                                             ReplaceSignature =
                                                 _reader.GetRulesSourcePropertyValueByName(Signing, "replace-signature") ==
                                                 "true",
                                             CompanyAttribute =
                                                 reader.GetRulesByNameForSigning(Signing,
                                                                                 "attributes",
                                                                                 "company", true),
                                             DescriptionAttribute =
                                                 reader.GetRulesByNameForSigning(Signing,
                                                                                 "attributes",
                                                                                 "description", true),
                                             ProductNameAttribute =
                                                 reader.GetRulesByNameForSigning(Signing,
                                                                                 "attributes",
                                                                                 "product-name", true),
                                             ProductVersion =
                                                 reader.GetRulesByNameForSigning(Signing,
                                                                                 "attributes",
                                                                                 "product-version", true),
                                             FileVersionAttribute =
                                                 reader.GetRulesByNameForSigning(Signing,
                                                                                 "attributes",
                                                                                 "file-version", true),
                                             EditCollectionViewModel = this.EditCollectionViewModel,
                                             IsEditable = true,
                                             IsReadOnly = false,
                                             IsSource = true,

                                         };

            SourceString = reader.GetRulesSourceStringPropertyValueByName(Signing);
            SourceSigningViewModel.PropertyChanged += EvaluatedChanged;
        }

        #region Properties

        private SigningViewModel _sourceSigningViewModel;
        public SigningViewModel SourceSigningViewModel
        {
            get { return _sourceSigningViewModel; }
            set
            {
                _sourceSigningViewModel = value;
                OnPropertyChanged("SourceSigningViewModel");
            }
        }

        private string _companyAttribute;
        public string CompanyAttribute
        {
            get { return _companyAttribute; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "CompanyAttribute", _companyAttribute, value);
                _companyAttribute = value;
                OnPropertyChanged("CompanyAttribute");
            }
        }


        private string _descriptionAttribute;
        public string DescriptionAttribute
        {
            get { return _descriptionAttribute; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "DescriptionAttribute", _descriptionAttribute, value);
                _descriptionAttribute = value;
                OnPropertyChanged("DescriptionAttribute");
            }
        }


        private string _productNameAttribute;
        public string ProductNameAttribute
        {
            get { return _productNameAttribute; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "ProductNameAttribute", _productNameAttribute, value);
                _productNameAttribute = value;
                OnPropertyChanged("ProductNameAttribute");
            }
        }

        private string _productVersionAttribute;
        public string ProductVersion
        {
            get { return _productVersionAttribute; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "ProductVersion", _productVersionAttribute, value);
                _productVersionAttribute = value;
                OnPropertyChanged("ProductVersion");
            }
        }

        private string _fileVersionAttribute;
        public string FileVersionAttribute
        {
            get { return _fileVersionAttribute; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "FileVersionAttribute", _fileVersionAttribute, value);
                _fileVersionAttribute = value;
                OnPropertyChanged("FileVersionAttribute");
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

        private EditCollectionViewModel _editCollectionViewModel;
        public EditCollectionViewModel EditCollectionViewModel
        {
            get { return _editCollectionViewModel; }
            set
            {
                _editCollectionViewModel = value;
                OnPropertyChanged("EditCollectionViewModel");
            }
        }

     

        
        public void EvaluatedChanged(object sender, PropertyChangedEventArgs args)
        {
            IEnumerable<string> newValues;
            switch (args.PropertyName)
            {
                case "CompanyAttribute":
                    //reevaluate
                    newValues = new[] {((SigningViewModel) sender).CompanyAttribute};
                    CompanyAttribute = _reader.SetNewSourceValueSigning(Signing, "attributes", "company", newValues);
                    break;
                case "DescriptionAttribute":
                    newValues = new[] {((SigningViewModel) sender).DescriptionAttribute};
                    DescriptionAttribute = _reader.SetNewSourceValueSigning(Signing, "attributes", "description", newValues);
                    break;
                case "ProductNameAttribute":
                    newValues = new[] { ((SigningViewModel)sender).ProductNameAttribute };
                    ProductNameAttribute = _reader.SetNewSourceValueSigning(Signing, "attributes", "product-name", newValues);
                    break;
                case "ProductVersion":
                    newValues = new[] { ((SigningViewModel)sender).ProductVersion };
                    ProductVersion = _reader.SetNewSourceValueSigning(Signing, "attributes", "product-version", newValues);
                    break;
                case "FileVersionAttribute":
                    newValues = new[] { ((SigningViewModel)sender).FileVersionAttribute };
                    FileVersionAttribute = _reader.SetNewSourceValueSigning(Signing, "attributes", "file-version", newValues);
                    break;
                case "ReplaceSignature":
                    ReplaceSignature= ((SigningViewModel)sender).ReplaceSignature;
                    break;
            }

            SourceString = _reader.GetRulesSourceStringPropertyValueByName(Signing);
        }
    }
}
