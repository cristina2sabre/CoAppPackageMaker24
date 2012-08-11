using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
    public class EditCollectionViewModel:ExtraPropertiesForCollectionsViewModelBase
    {
        public string Parameter;
        private Type _typeForNewItems;
        private string _ruleNameToDisplay;
        private string _collectionName;
        public EditCollectionViewModel(ObservableCollection<BaseItemViewModel> collection, string collectionName, string ruleNameToDisplay, Type typeForNewItems, string parameter=null)
        {
             Parameter = parameter;
            _editableItems = collection;
            _ruleNameToDisplay = ruleNameToDisplay;
            _collectionName = collectionName;
            _typeForNewItems = typeForNewItems;
            _editableItems.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(FilesCollectionCollectionChanged);
        }

        void FilesCollectionCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DefaultChangeFactory.OnCollectionChanged(this, "EditableItems", EditableItems, e);
            OnPropertyChanged("EditableItems");
        }

        //public ObservableCollection<T> Images
        //{
        //    get { return (ObservableCollection<T>)GetValue(ImagesProperty); }
        //    set { SetValue(ImagesProperty, value); }
        //}

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

         
            if (this.SelectedItem.Parameter!=null)
            {
                MainWindowViewModel.Instance.Reader.RemoveRulesWithParameters(this.SelectedItem.RuleNameToDisplay, this.SelectedItem.Parameter,
                                                                           this.SelectedItem.CollectionName,
                                                                           this.SelectedItem.SourceValue);
            }

           
            MainWindowViewModel.Instance.Reader.RemoveFromList(this.SelectedItem.RuleNameToDisplay,
                                                                             this.SelectedItem.CollectionName,
                                                                             this.SelectedItem.SourceValue);
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
               
               object newItem = Activator.CreateInstance( _typeForNewItems, _ruleNameToDisplay,
                                                         _collectionName);


               ((BaseItemViewModel) newItem).Parameter = (EditableItems.Count != 0)
                                                             ? EditableItems[0].Parameter
                                                             : Parameter;
             
               ((BaseItemViewModel) newItem).Collection = _editableItems;
               this.EditableItems.Add(((BaseItemViewModel) newItem));
           }

        #endregion


    }
}
