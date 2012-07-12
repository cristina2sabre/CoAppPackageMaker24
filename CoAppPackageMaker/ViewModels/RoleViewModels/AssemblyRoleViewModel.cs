﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CoAppPackageMaker.ViewModels.Base;
using CoAppPackageMaker.ViewModels.RuleViewModels;

namespace CoAppPackageMaker.ViewModels
{
    //This role specifies shared libraries which are dynamically linked to by other assemblies and applications.
    public class AssemblyRoleViewModel : ApplicationRoleViewModel
    {

        //withparam name?
        public AssemblyRoleViewModel(PackageReader reader)
            : base()
        {
            // Root = root;

            ApplicationCollection = new ObservableCollection<RoleItemViewModel>();

            foreach (string parameter in reader.ReadParameters("assembly"))
            {
                ObservableCollection<ItemViewModel> includeCollection = new ObservableCollection<ItemViewModel>(reader.ApplicationIncludeList("assembly", parameter, "include"));

                RoleItemViewModel model = new RoleItemViewModel()
                {
                    Label = "Assembly",
                    EditCollectionViewModel = new EditCollectionViewModel(reader, includeCollection),
                    Name = parameter,
                };

                ApplicationCollection.Add(model);
            }

            SourceString = reader.GetRulesSourceStringPropertyValueByName("assembly");
            ApplicationCollection.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(base.FilesCollectionCollectionChanged);
        }

        public override void Add()
        {

            this.ApplicationCollection.Add(new ApplicationRoleViewModel.RoleItemViewModel() { Label = "Assembly", EditCollectionViewModel = new EditCollectionViewModel(null, new ObservableCollection<ItemViewModel>()) });
        }
    }
}