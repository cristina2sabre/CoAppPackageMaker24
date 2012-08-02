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
           

            ApplicationCollection = new ObservableCollection<RoleItemViewModel>();

            foreach (string parameter in reader.ReadParameters("assembly"))
            {
                var includeCollection = new ObservableCollection<BaseItemViewModel>(reader.GetRulesByParamater(parameter, "include", "assembly", typeof(ApplicationItem)));
                var model = new RoleItemViewModel()
                {
                    //RoleName = "Assembly",
                     RuleNameToDisplay = "assembly",
                    EditCollectionViewModel =
                        new EditCollectionViewModel( includeCollection, typeof(ApplicationItem)),
                    Parameter = parameter,
                   
                };
                ApplicationCollection.Add(model);
            }
            

            SourceString = reader.GetRulesSourceStringPropertyValueByName("assembly");
            ApplicationCollection.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(base.RolesCollectionChanged);
        }

        public override void Add()
        {

            this.ApplicationCollection.Add(new RoleItemViewModel() {RuleNameToDisplay = "assembly",EditCollectionViewModel = new EditCollectionViewModel(new ObservableCollection<BaseItemViewModel>(),typeof(ApplicationItem) ) });
        }
    }
}