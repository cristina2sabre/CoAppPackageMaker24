using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Policy;

namespace CoAppPackageMaker.ViewModels
{
    class MetadataViewModel : ExtraPropertiesViewModelBase
    {
        private string _summary;
        private string _description;
        private string _authorVersion;
        private string _bugTracker;
        private string _stability;
        private string _licenses;
       
        public  MetadataViewModel()
        {
        }

        public MetadataViewModel(PackageReader reader)
        {

            string metadata = "metadata";

            {
                Summary = reader.GetRulesPropertyValueByName(metadata, "summary");
                Description = reader.GetRulesPropertyValueByName(metadata, "description");
                AuthorVersion = reader.GetRulesPropertyValueByName(metadata, "author-version");
                BugTracker = reader.GetRulesPropertyValueByName(metadata, "bug-tracker");
                Stability = reader.GetRulesPropertyValueByName(metadata, "stability");
                Licenses = reader.GetRulesPropertyValueByName(metadata, "licenses");
            };
        }

        public String Summary
        {
            get { return _summary; }
            set
            {
                _summary = value;
                OnPropertyChanged("Summary");
            }
        }

        public String Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        public String AuthorVersion
        {
            get { return _authorVersion; }
            set
            {
                _authorVersion = value;
                OnPropertyChanged("AuthorVersion");
            }
        }

        public String BugTracker
        {
            get { return _bugTracker; }
            set
            {
                _bugTracker = value;
                OnPropertyChanged("BugTracker");
            }
        }

        public String Stability
        {
            get { return _stability; }
            set
            {
                _stability = value;
                OnPropertyChanged("Stability");
            }
        }

        public String Licenses
        {
            get { return _licenses; }
            set
            {
                _licenses = value;
                OnPropertyChanged("Licenses");
            }
        }
      

    }
}
