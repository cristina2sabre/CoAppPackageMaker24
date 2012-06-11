using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
    public class LicenseViewModel:ExtraPropertiesViewModelBase
{
        public LicenseViewModel ()
        {

        }

        public LicenseViewModel(PackageReader reader)
    {
        string license = "license";

        License = reader.GetRulesPropertyValueByName(license, "license");
        LicenseType = reader.GetRulesPropertyValueByName(license,"license-type");
        LicenseUrl = reader.GetRulesPropertyValueByName(license, "license-url");
        _sourceValueLicenseViewModel=new SourceLicenseViewModel(reader);
        SourceString = reader.GetRulesSourceStringPropertyValueByName(license);

    }

    private string _license;
    public string License
    {
        get { return _license; }
        set
        {
            _license = value;
            OnPropertyChanged("License");
        }
    }

        private string _licenseUrl;

        public string LicenseUrl
        {
            get { return _licenseUrl; }
            set
            {
                _licenseUrl = value;
                OnPropertyChanged("LicenseUrl");
            }
        }


        private string _licenseType;

        public string LicenseType
        {
            get { return _licenseType; }
            set
            {
                _licenseType = value;
                OnPropertyChanged("LicenseType");
            }
        }

        private SourceLicenseViewModel _sourceValueLicenseViewModel;

        public SourceLicenseViewModel SourceValueLicenseViewModel
        {
            get { return _sourceValueLicenseViewModel; }
            set
            {
                _sourceValueLicenseViewModel = value;
                OnPropertyChanged("SourceValueLicenseViewModel");
            }
        }

        public class SourceLicenseViewModel:LicenseViewModel
        {
             public SourceLicenseViewModel(PackageReader reader)
             {
                 string license = "license";
                 License = reader.GetRulesSourcePropertyValueByName(license, "license");
                 LicenseType = reader.GetRulesSourcePropertyValueByName(license, "license-type");
                 LicenseUrl = reader.GetRulesSourcePropertyValueByName(license, "license-url");
             }
        }

     

}
}
