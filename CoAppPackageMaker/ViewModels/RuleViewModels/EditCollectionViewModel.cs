using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
    public class EditCollectionViewModel:ExtraPropertiesForCollectionsViewModelBase
    {
        readonly PackageReader _reader;
        private Type _typeForNewItems;

        private ObservableCollection<BaseItemViewModel> _editableItems;
        public ObservableCollection<BaseItemViewModel> EditableItems
        {
            get { return _editableItems; }
            set
            {
               
                _editableItems = value;
                OnPropertyChanged("EditableItems");
            }
        }

        public EditCollectionViewModel(PackageReader reader, ObservableCollection<BaseItemViewModel> collection, Type typeForNewItems)
        {
            _reader = reader;
            _editableItems = collection;
            _typeForNewItems = typeForNewItems;
            _editableItems.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(FilesCollectionCollectionChanged);
        }

        void FilesCollectionCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DefaultChangeFactory.OnCollectionChanged(this, "EditableItems", EditableItems, e);
            OnPropertyChanged("EditableItems");
        }
      
        private BaseItemViewModel _selectedItem;
        public BaseItemViewModel SelectedItem
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

           
           
            this.EditableItems.Remove(this.SelectedItem);
           
           

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
               return this.EditableItems!= null;

           }
           public void Add()
           {
              //to add type
               Type type = _typeForNewItems;
               object newItem = Activator.CreateInstance(type);
               ((BaseItemViewModel) newItem).Reader = _reader;
               ((BaseItemViewModel)newItem).Index = EditableItems.Count;
               //if is a new collection ->fail!!!!!!!!!!!
               ((BaseItemViewModel)newItem).Label =
               EditableItems[0].Label;
               ((BaseItemViewModel)newItem).CollectionName =
              EditableItems[0].CollectionName;
               this.EditableItems.Add((BaseItemViewModel)newItem);
           }
    
        #endregion
    }
}
