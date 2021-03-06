﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoAppPackageMaker.ViewModels
{


    public class PackageCompositionViewModel : ExtraPropertiesForCollectionsViewModelBase
    {
        public PackageCompositionViewModel(PackageReader reader)
        {
          //  List<string> parameters = reader.ReadPackageCompositionParameters();
         //   string a = reader.GetRulesPropertyValuesByNameForSigning("package-composition", "symlinks", "exes");
    //        Symlinks = reader.GetRulesPropertyValuesByNameForSigning("package-composition", "symlinks", "exes");
            SourceString = reader.GetRulesSourceStringPropertyValueByName("package-composition");
        }

        private string _symlinks;
         private string _fileCopy;

        public string Symlinks
        {
            get { return _symlinks; }
            set
            {
                _symlinks = value;
                OnPropertyChanged("Symlinks");
            }
        }

        public string FileCopy
        {
            get { return _fileCopy; }
            set
            {
               _fileCopy = value;
                OnPropertyChanged("FileCopy");
            }
        }
    }
}
