using System.Collections.Generic;
using System.ComponentModel;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
    public class LicenseViewModel : ExtraPropertiesViewModelBase
{
        private readonly PackageReader _reader;
        private const string LicenseString = "license";
        public LicenseViewModel ()
        {

        }

        public LicenseViewModel(PackageReader reader)
        {

            _reader = reader;
            License =
                reader.GetRulesSourcePropertyValueByName(LicenseString, "license");
            LicenseType =
                reader.GetRulesSourcePropertyValueByName(LicenseString, "license-type");
            LicenseUrl =
                reader.GetRulesSourcePropertyValueByName(LicenseString, "license-url");
            IsSource = true;
            RuleNameToDisplay = "License";
          

            _valueLicenseViewModel = new LicenseViewModel()
                                               {
                                                    License = reader.GetRulesPropertyValueByName(LicenseString, "license"),
            LicenseType = reader.GetRulesPropertyValueByName(LicenseString, "license-type"),
            LicenseUrl = reader.GetRulesPropertyValueByName(LicenseString, "license-url"),
            SourceString = reader.GetRulesSourceStringPropertyValueByName(LicenseString),
            IsReadOnly = true,
                                               };

            
            this.PropertyChanged += EvaluatedChanged;

        }

        #region Properties

        private string _license;
        public string License
        {
            get { return _license; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "License", _license, value);
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
                DefaultChangeFactory.OnChanging(this, "LicenseUrl", _licenseUrl, value);
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
                DefaultChangeFactory.OnChanging(this, "LicenseType", _licenseType, value);
                _licenseType = value;
                OnPropertyChanged("LicenseType");
            }
        }

        #endregion

        private LicenseViewModel _valueLicenseViewModel;
        public LicenseViewModel ValueLicenseViewModel
        {
            get { return _valueLicenseViewModel; }
            set
            {
                _valueLicenseViewModel = value;
                OnPropertyChanged("ValueLicenseViewModel");
            }
        }


        public void EvaluatedChanged(object sender, PropertyChangedEventArgs args)
        {
            
           IEnumerable<string> newValues;
                switch (args.PropertyName)
                {
                   case "License":
                        //reevaluate
                        newValues= new[]{License};
                        this.ValueLicenseViewModel.License = _reader.SetNewSourceValue(LicenseString, "license", newValues);
                        break;
                    case "LicenseType":
                        newValues = new[] {LicenseType};
                        this.ValueLicenseViewModel.LicenseType = _reader.SetNewSourceValue(LicenseString, "license-type", newValues);
                        break;
                    case "LicenseUrl":
                        newValues = new[] { LicenseUrl };
                        this.ValueLicenseViewModel.LicenseUrl = _reader.SetNewSourceValue(LicenseString, "license-url", newValues);
                        break;
                 
                }

                this.ValueLicenseViewModel.SourceString = _reader.GetRulesSourceStringPropertyValueByName(LicenseString);
            }
     
        }
}

