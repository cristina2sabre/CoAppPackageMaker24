using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
    public class EditCollectionViewModel:ExtraPropertiesViewModelBase
    {
        private ObservableCollection<ItemViewModel> _filesCollection;
        public ObservableCollection<ItemViewModel> FilesCollection
        {
            get { return _filesCollection; }
            set
            {
               //DefaultChangeFactory.OnChanging(this, "FilesCollection", _filesCollection, value);
                _filesCollection = value;
                OnPropertyChanged("FilesCollection");
            }
        }

        public EditCollectionViewModel(PackageReader reader, MainWindowViewModel root, ExtraPropertiesViewModelBase f)
        {
           // reader.GetRulesSourcePropertyValuesByNameForRequired("requires", "package");
            Root = root;
           // _filesCollection = new ObservableCollection<ItemViewModel>();
            //foreach (string parameter in reader.GetRulesPropertyValues("requires", "package"))
            //{
            //   ItemViewModel itemViewModel = new ItemViewModel()
            //    {
            //      SourceValue = parameter,
            //      Root = root,
            //    };
            //    _filesCollection.Add(itemViewModel);
            //}
            _filesCollection = reader.GetRulesSourcePropertyValuesByNameForRequired("requires", "package",root);
            //SourceString = reader.GetRulesSourceStringPropertyValueByName("files");
            _filesCollection.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(FilesCollectionCollectionChanged);
        }

        void FilesCollectionCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
          //  FilesCollection = sender as ObservableCollection<FilesItemViewModel>;
           // DefaultChangeFactory.OnChanging(this, "FilesCollection", _filesCollection, sender as FilesItemViewModel);
          
            DefaultChangeFactory.OnCollectionChanged(this, "FilesCollection", FilesCollection, e);
            OnPropertyChanged("FilesCollection");
        }

       

       
        private ItemViewModel _selectedItem;
        public ItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        
        #region Event Handlers



        public ICommand RemoveCommand
        {
            get { return new RelayCommand(Remove, CanRemove); }
        }

        public void Remove()
        {
            this.FilesCollection.Remove(this.SelectedItem);
           
        }
      
           bool CanRemove()
         {
             return this.SelectedItem != null;
            
         }
          

           public ICommand AddCommand
           {
               get { return new RelayCommand(Add, CanAdd); }
           }
           bool CanAdd()
           {
               return this.FilesCollection!= null;

           }
           public void Add()
           {

               this.FilesCollection.Add(new ItemViewModel() { Root = this.Root });
           }
    
        #endregion
    }
}
