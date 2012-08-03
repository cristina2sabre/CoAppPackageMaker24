﻿using System.Collections.ObjectModel;
using CoAppPackageMaker.ViewModels.RuleViewModels;

namespace CoAppPackageMaker.ViewModels
{
    //This role specifies shared libraries which are dynamically linked to by other assemblies and applications.
    public class AssemblyRoleViewModel : ApplicationRoleViewModel
    {

       
        public AssemblyRoleViewModel(PackageReader reader)
            : base()
        {
            RoleCollection = new ObservableCollection<RoleItemViewModel>();
            foreach (string parameter in reader.ReadParameters("assembly"))
            {
                var includeCollection = new ObservableCollection<BaseItemViewModel>(reader.GetRulesByParamater(parameter, "include", "assembly", typeof(ApplicationItem)));
                var model = new RoleItemViewModel()
                {
                    RuleNameToDisplay = "assembly",
                    EditCollectionViewModel =
                        new EditCollectionViewModel( includeCollection, "include", "assembly", typeof(ApplicationItem)),
                    Parameter = parameter,
                   
                };
                RoleCollection.Add(model);
            }
            

            SourceString = reader.GetRulesSourceStringPropertyValueByName("assembly");
            RoleCollection.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(base.RolesCollectionChanged);
        }

        public override void Add()
        {

            this.RoleCollection.Add(new RoleItemViewModel() { RuleNameToDisplay = "assembly", EditCollectionViewModel = new EditCollectionViewModel(new ObservableCollection<BaseItemViewModel>(), "include", "assembly", typeof(ApplicationItem)) });
        }
    }
}