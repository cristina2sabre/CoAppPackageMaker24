﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
    public class SigningViewModel : ExtraPropertiesViewModelBase
    {
        private PackageReader _reader;
        private const string Signing = "signing";

        public SigningViewModel()
        {

        }

        public SigningViewModel(PackageReader reader)
        {
            _reader = reader;


            // ObservableCollection<string> values = new ObservableCollection<string>();
            // ObservableCollection<string> values2 = new ObservableCollection<string>(reader.GetRulesPropertyValues("assembly", "include")); la fel pt application

            ReplaceSignature = _reader.GetRulesPropertyValueByName(Signing, "replace-signature") == "true";
            Include = new ObservableCollection<string>(_reader.GetRulesPropertyValues(Signing, "include"));

            CompanyAttribute = reader.GetRulesPropertyValuesByNameForSigning(Signing, "attributes", "company");
            DescriptionAttribute = reader.GetRulesPropertyValuesByNameForSigning(Signing, "attributes", "description");
            ProductNameAttribute = reader.GetRulesPropertyValuesByNameForSigning(Signing, "attributes", "product-name");
            ProductVersion = reader.GetRulesPropertyValuesByNameForSigning(Signing, "attributes", "product-version");
            FileVersionAttribute = reader.GetRulesPropertyValuesByNameForSigning(Signing, "attributes", "file-version");
            IsEditable = false;
            IsReadOnly = true;

            SourceSigningViewModel = new SigningViewModel()
                                         {

                                             ReplaceSignature =
                                                 _reader.GetRulesSourcePropertyValueByName(Signing, "replace-signature") ==
                                                 "true",
                                             Include =
                                                 new ObservableCollection<string>(
                                                 _reader.GetRulesSourcePropertyValuesByName(Signing, "include")),
                                             CompanyAttribute =
                                                 reader.GetRulesSourcePropertyValuesByNameForSigning(Signing,
                                                                                                     "attributes",
                                                                                                     "company"),
                                             DescriptionAttribute =
                                                 reader.GetRulesSourcePropertyValuesByNameForSigning(Signing,
                                                                                                     "attributes",
                                                                                                     "description"),
                                             ProductNameAttribute =
                                                 reader.GetRulesSourcePropertyValuesByNameForSigning(Signing,
                                                                                                     "attributes",
                                                                                                     "product-name"),
                                             ProductVersion =
                                                 reader.GetRulesSourcePropertyValuesByNameForSigning(Signing,
                                                                                                     "attributes",
                                                                                                     "product-version"),
                                             FileVersionAttribute =
                                                 reader.GetRulesSourcePropertyValuesByNameForSigning(Signing,
                                                                                                     "attributes",
                                                                                                     "file-version"),
                                             IsEditable = true,
                                             IsReadOnly = false,


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

        private ObservableCollection<string> _include;

        public ObservableCollection<string> Include
        {
            get { return _include; }
            set
            {
                _include = value;
                OnPropertyChanged("Include");
            }
        }

        #endregion

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


            }

        }
    }
}
