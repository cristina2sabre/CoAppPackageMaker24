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

        public EditCollectionViewModel( ObservableCollection<BaseItemViewModel> collection, Type typeForNewItems)
        {
            _reader = MainWindowViewModel.Instance.Reader;
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
             
               //if is a new collection ->fail!!!!!!!!!!!
               ((BaseItemViewModel)newItem).Parameter =(EditableItems.Count!=0)?
                   EditableItems[0].Parameter:String.Empty;
               //to add as a paramater
               ((BaseItemViewModel)newItem).CollectionName =(EditableItems.Count!=0)?
              EditableItems[0].CollectionName:String.Empty;
               ((BaseItemViewModel) newItem).upper = _editableItems;

               ((BaseItemViewModel)newItem).RuleNameToDisplay = _editableItems[0].RuleNameToDisplay;

               this.EditableItems.Add(((BaseItemViewModel)newItem));
           }
    
        #endregion


    }
}
