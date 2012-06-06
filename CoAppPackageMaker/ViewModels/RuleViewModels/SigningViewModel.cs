using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
    class SigningViewModel:ExtraPropertiesViewModelBase
    {

        public SigningViewModel(PackageReader reader)
        {
          string signing = "signing";
          ReplaceSignature=reader.GetRulesPropertyValueByName(signing, "replace-signature")=="true";
          Include = new ObservableCollection<string>(reader.GetRulesPropertyValues(signing, "include"));
          ObservableCollection<string> values = new ObservableCollection<string>(reader.GetRulesPropertyValues(signing, "attributes"));
        
          reader.ReadSigning();
        }

        #region Properties
        private string _companyAttribute;

        public string CompanyAttribute
        {
            get { return _companyAttribute; }
            set
            {
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
                _fileVersionAttribute = value;
                OnPropertyChanged("VersionAttribute");
            }
        }

        private bool _replaceSignature;

        public bool ReplaceSignature
        {
            get { return _replaceSignature; }
            set
            {
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



    }
}
