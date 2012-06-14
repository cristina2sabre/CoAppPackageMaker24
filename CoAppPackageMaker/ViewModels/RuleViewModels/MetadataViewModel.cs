using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Security.Policy;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels
{
    class MetadataViewModel : ExtraPropertiesViewModelBase,ISupportsUndo
    {
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
                IsEditable = false;
                SourceString = reader.GetRulesSourceStringPropertyValueByName(metadata);
                IsFocused = true;

            };
            SourceMetadataViewModel = new MetadataViewModel()
                                          {
                                              Summary = reader.GetRulesSourcePropertyValueByName(metadata, "summary"),
                                              Description = reader.GetRulesSourcePropertyValueByName(metadata, "description"),
                                              AuthorVersion = reader.GetRulesSourcePropertyValueByName(metadata, "author-version"),
                                              BugTracker = reader.GetRulesSourcePropertyValueByName(metadata, "bug-tracker"),
                                              Stability = reader.GetRulesSourcePropertyValueByName(metadata, "stability"),
                                              Licenses = reader.GetRulesSourcePropertyValueByName(metadata, "licenses"),
                                              IsEditable = true
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
                IsFocused = true;

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
            {
                switch (args.PropertyName)
                {
                    case "Summary":
                        Summary = ((MetadataViewModel)sender).Summary;
                        break;
                    case "Description":
                        Description = ((MetadataViewModel)sender).Description;
                        break;
                    case "AuthorVersion":
                        AuthorVersion = ((MetadataViewModel)sender).AuthorVersion;
                        break;
                    case "BugTracker":
                        BugTracker = ((MetadataViewModel)sender).BugTracker;
                        break;
                    case "Stability":
                        Stability = ((MetadataViewModel)sender).Stability;
                        break;
                    case "Licenses":
                        Licenses = ((MetadataViewModel)sender).Licenses;
                        break;

                }

            }
        }

        private MainWindowViewModel _Root;
        public MainWindowViewModel Root
        {
            get { return _Root; }
            set
            {
                if (value == _Root)
                    return;

                // This line will log the property change with the undo framework.
                DefaultChangeFactory.OnChanging(this, "Root", _Root, value);

                _Root = value;
                OnPropertyChanged("Root");
            }
        }

        #region ISupportsUndo Members

        public object GetUndoRoot()
        {
            return this.Root;
        }

        #endregion

    }
}
