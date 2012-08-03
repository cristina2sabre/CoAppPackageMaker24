using System;
using System.Collections.Generic;
using System.ComponentModel;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels
{
    public class MetadataViewModel : ExtraPropertiesViewModelBase
    {
        private readonly PackageReader _reader;
        private const string Metadata = "metadata";
        public  MetadataViewModel()
        {
        }

        public MetadataViewModel(PackageReader reader)
        {
            _reader = reader;
          
               
          
            ValueMetadataViewModel = new MetadataViewModel()
                                          { Summary = reader.GetRulesPropertyValueByName(Metadata, "summary"),
                Description = reader.GetRulesPropertyValueByName(Metadata, "description"),
                AuthorVersion = reader.GetRulesPropertyValueByName(Metadata, "author-version"),
                BugTracker = reader.GetRulesPropertyValueByName(Metadata, "bug-tracker"),
                Stability = reader.GetRulesPropertyValueByName(Metadata, "stability"),
                Licenses = reader.GetRulesPropertyValueByName(Metadata, "licenses"),
                IsEditable = false,
                SourceString = reader.GetRulesSourceStringPropertyValueByName(Metadata),                           
                                          };
            Summary = reader.GetRulesSourcePropertyValueByName(Metadata, "summary");
            Description = reader.GetRulesSourcePropertyValueByName(Metadata, "description");
            AuthorVersion = reader.GetRulesSourcePropertyValueByName(Metadata, "author-version");
            BugTracker = reader.GetRulesSourcePropertyValueByName(Metadata, "bug-tracker");
            Stability = reader.GetRulesSourcePropertyValueByName(Metadata, "stability");
            Licenses = reader.GetRulesSourcePropertyValueByName(Metadata, "licenses");
            IsEditable = true;
            IsSource = true;
            RuleNameToDisplay = "Metadata";
           
            this.PropertyChanged += EvaluatedChanged;
        }

        #region Properties

        private string _summary;
        public String Summary
        {
            get { return _summary; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "Summary", _summary, value);
                _summary = value;
                OnPropertyChanged("Summary");
            }
        }

      
        private string _description;
        public String Description
        {
            get { return _description; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "Description", _description, value);
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        private string _authorVersion;
        public String AuthorVersion
        {
            get { return _authorVersion; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "AuthorVersion", _authorVersion, value);
                _authorVersion = value;
                OnPropertyChanged("AuthorVersion");
            }
        }

        private string _bugTracker;
        public String BugTracker
        {
            get { return _bugTracker; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "BugTracker", _bugTracker, value);
                _bugTracker = value;
                OnPropertyChanged("BugTracker");
            }
        }

        private string _stability;
        public String Stability
        {
            get { return _stability; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "Stability", _stability, value);
                _stability = value;
                OnPropertyChanged("Stability");
            }
        }

        private string _licenses;
        public String Licenses
        {
            get { return _licenses; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "Licenses", _licenses, value);
                _licenses = value;
                OnPropertyChanged("Licenses");
            }
        }

        #endregion

        private MetadataViewModel _valueMetadataViewModel;
        public MetadataViewModel ValueMetadataViewModel
        {
            get { return _valueMetadataViewModel; }
            set
            {
                _valueMetadataViewModel = value;
                OnPropertyChanged("ValueMetadataViewModel");
            }
        }

       
        public void EvaluatedChanged(object sender, PropertyChangedEventArgs args)
        {

            IEnumerable<string> newValues;
                switch (args.PropertyName)
                {
                    case "Summary":
                        //reevaluate
                        newValues = new[] { Summary };
                        ValueMetadataViewModel.Summary = _reader.SetNewSourceValue(Metadata, "summary", newValues);
                        break;
                    case "Description":
                        newValues = new [] { Description };
                        ValueMetadataViewModel.Description = _reader.SetNewSourceValue(Metadata, "description", newValues);
                        break;
                    case "AuthorVersion":
                        newValues = new [] { AuthorVersion };
                        ValueMetadataViewModel.AuthorVersion = _reader.SetNewSourceValue(Metadata, "author-version", newValues);
                        break;
                    case "BugTracker":
                        newValues = new [] { BugTracker };
                        ValueMetadataViewModel.BugTracker = _reader.SetNewSourceValue(Metadata, "bug-tracker", newValues);
                        break;
                    case "Stability":
                        newValues = new[] { Stability };
                        ValueMetadataViewModel.Stability = _reader.SetNewSourceValue(Metadata, "stability", newValues);
                        break;
                    case "Licenses":
                        newValues = new[] { Licenses };
                        ValueMetadataViewModel.Licenses = _reader.SetNewSourceValue(Metadata, "licenses", newValues);
                        break;

                }
                ValueMetadataViewModel.SourceString = _reader.GetRulesSourceStringPropertyValueByName(Metadata); 
    }     

    }
}
