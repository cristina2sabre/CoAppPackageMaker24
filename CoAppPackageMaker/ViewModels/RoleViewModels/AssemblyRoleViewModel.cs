using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CoAppPackageMaker.ViewModels.Base;
using CoAppPackageMaker.ViewModels.RuleViewModels;

namespace CoAppPackageMaker.ViewModels
{
    //This role specifies shared libraries which are dynamically linked to by other assemblies and applications.
    public class AssemblyRoleViewModel:ApplicationRoleViewModel
    {
      
        //withparam name?
        public AssemblyRoleViewModel(PackageReader reader, MainWindowViewModel root) : base()
        {
            Root = root;
           
            ApplicationCollection = new ObservableCollection<ApplicationItemViewModel>();

            foreach (string parameter in reader.ReadParameters("assembly"))
            {
                ObservableCollection<ItemViewModel> includeCollection = new ObservableCollection<ItemViewModel>(reader.ApplicationIncludeList("assembly", parameter, "include", root));

                ApplicationItemViewModel model = new ApplicationItemViewModel()
                {
                   Label = "Assembly",
                    EditCollectionViewModel = new EditCollectionViewModel(reader, root, includeCollection),
                    Name = parameter,
                    Root = root,

                };
                ApplicationCollection.Add(model);
            }

            SourceString = reader.GetRulesSourceStringPropertyValueByName("assembly");
           ApplicationCollection.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(base.FilesCollectionCollectionChanged);
        }

        public override void Add()
        {

            this.ApplicationCollection.Add(new ApplicationRoleViewModel.ApplicationItemViewModel() { Label = "Assembly", Root = this.Root, EditCollectionViewModel = new EditCollectionViewModel(null, Root, new ObservableCollection<ItemViewModel>()) });
        }
    }
}
