﻿using System.Collections.ObjectModel;
﻿using System.Linq;
﻿using System.Windows.Forms;
﻿using CoAppPackageMaker.ViewModels.RuleViewModels;

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
                        new EditCollectionViewModel( includeCollection, "include", "assembly", typeof(ApplicationItem),parameter),
                    Parameter = parameter,
                   
                };
                RoleCollection.Add(model);
            }
            

            SourceString = reader.GetRulesSourceStringPropertyValueByName("assembly");
            RoleCollection.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(base.RolesCollectionChanged);
        }

        public override void Add()
        {
            var col = this.RoleCollection.Where(item => item.Parameter == "[]");
            if (!col.Any())
            {
                var newItem =
                    new RoleItemViewModel()
                        {
                            RuleNameToDisplay = "assembly",
                            EditCollectionViewModel =
                                new EditCollectionViewModel(new ObservableCollection<BaseItemViewModel>(), "include",
                                                            "assembly", typeof (ApplicationItem))
                        };
                this.RoleCollection.Add(newItem);
            }
            else
            {
                MessageBox.Show("An item with the same parameters exist aready in the collection");
            }
           
        }
    }
}