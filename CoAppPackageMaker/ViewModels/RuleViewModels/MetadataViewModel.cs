using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Security.Policy;
using CoAppPackageMaker.Temp;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels
{
    public class MetadataViewModel : ExtraPropertiesViewModelBase
    {

    
        private PackageReader _reader;
        private const string Metadata = "metadata";
        public  MetadataViewModel()
        {
        }

        public MetadataViewModel(PackageReader reader)
        {
            _reader = reader;
           
          
            {
                Summary = reader.GetRulesPropertyValueByName(Metadata, "summary");
                Description = reader.GetRulesPropertyValueByName(Metadata, "description");
                AuthorVersion = reader.GetRulesPropertyValueByName(Metadata, "author-version");
                BugTracker = reader.GetRulesPropertyValueByName(Metadata, "bug-tracker");
                Stability = reader.GetRulesPropertyValueByName(Metadata, "stability");
                Licenses = reader.GetRulesPropertyValueByName(Metadata, "licenses");
                IsEditable = false;
                SourceString = reader.GetRulesSourceStringPropertyValueByName(Metadata);
                IsFocused = true;

            };
            SourceMetadataViewModel = new MetadataViewModel()
                                          {
                                              Summary = reader.GetRulesSourcePropertyValueByName(Metadata, "summary"),
                                              Description = reader.GetRulesSourcePropertyValueByName(Metadata, "description"),
                                              AuthorVersion = reader.GetRulesSourcePropertyValueByName(Metadata, "author-version"),
                                              BugTracker = reader.GetRulesSourcePropertyValueByName(Metadata, "bug-tracker"),
                                              Stability = reader.GetRulesSourcePropertyValueByName(Metadata, "stability"),
                                              Licenses = reader.GetRulesSourcePropertyValueByName(Metadata, "licenses"),
                                              IsEditable = true,
                                              IsSource = true,
                                          };

            SourceMetadataViewModel.PropertyChanged += EvaluatedChanged;
        }

        private string _summary;
        public String Summary
        {
            get { return _summary; }
            set
            {
                DefaultChangeFactory.OnChanging(this, "Summary", _summary, value);
                _summary = value;
                OnPropertyChanged("Summary");
               // IsFocused = true;
              

            }

        }

        private bool _isFocused = false;
        public bool IsFocused
        {
            get
            {
                return _isFocused;
            }
            set
            {
                _isFocused = false;
                _isFocused = value;

                OnPropertyChanged("IsFocused");
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

        private MetadataViewModel _sourceMetadataViewModel;
        public MetadataViewModel SourceMetadataViewModel
        {
            get { return _sourceMetadataViewModel; }
            set
            {
                _sourceMetadataViewModel = value;
                OnPropertyChanged("SourceMetadataViewModel");
            }
        }

       
        public void EvaluatedChanged(object sender, PropertyChangedEventArgs args)
        {

            IEnumerable<string> newValues;
                switch (args.PropertyName)
                {
                    case "Summary":
                        //reevaluate
                        newValues = new[] { ((MetadataViewModel)sender).Summary };
                        Summary = _reader.SetNewSourceValue(Metadata, "summary", newValues);
                        break;
                    case "Description":
                        newValues = new [] { ((MetadataViewModel)sender).Description };
                        Description = _reader.SetNewSourceValue(Metadata, "description", newValues);
                        break;
                    case "AuthorVersion":
                        newValues = new [] { ((MetadataViewModel)sender).AuthorVersion };
                        AuthorVersion = _reader.SetNewSourceValue(Metadata, "author-version", newValues);
                        break;
                    case "BugTracker":
                        newValues = new [] { ((MetadataViewModel)sender).BugTracker };
                        BugTracker = _reader.SetNewSourceValue(Metadata, "bug-tracker", newValues);
                        break;
                    case "Stability":
                        newValues = new[] { ((MetadataViewModel)sender).Stability };
                        Stability = _reader.SetNewSourceValue(Metadata, "stability", newValues);
                        break;
                    case "Licenses":
                        newValues = new[] { ((MetadataViewModel)sender).Licenses };
                        Licenses = _reader.SetNewSourceValue(Metadata, "licenses", newValues);
                        break;

                }

      
    }

       

       

    }
}
