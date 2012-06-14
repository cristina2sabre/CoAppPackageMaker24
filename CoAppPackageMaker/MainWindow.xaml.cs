using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;


namespace CoAppPackageMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           // DataContext = new MainWindowViewModel();
           // LayoutRoot_Loaded();
        }


        #region View Model Accessor

        private MainWindowViewModel VM
        {
            get
            {
                return DataContext as MainWindowViewModel;
            }
        }
        #endregion


        #region Event Handlers

        private void LayoutRoot_Loaded()
        {
            // The undo / redo stack collections are not "Observable", so we 
            // need to manually refresh the UI when they change.
            var root = UndoService.Current[this];
            root.UndoStackChanged += new EventHandler(OnUndoStackChanged);
            root.RedoStackChanged += new EventHandler(OnRedoStackChanged);
             

        }

        // Refresh the UI when the undo stack changes.
        void OnUndoStackChanged(object sender, EventArgs e)
        {
            RefreshUndoStackList();
        }

        // Refresh the UI when the redo stack changes.
        void OnRedoStackChanged(object sender, EventArgs e)
        {
            RefreshUndoStackList();
        }

        // Refresh the UI when the undo / redo stacks change.
        private void RefreshUndoStackList()
        {
            // Calling refresh on the CollectionView will tell the UI to rebind the list.
            // If the list were an ObservableCollection, or implemented INotifyCollectionChanged, this would not be needed.
            var cv = CollectionViewSource.GetDefaultView(UndoStack);
            cv.Refresh();

            cv = CollectionViewSource.GetDefaultView(RedoStack);
            cv.Refresh();
        }
        // The following 4 event handlers support the "CommandBindings" in the window.
        // These hook to the Undo and Redo commands.

        private void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Tell the UI whether Undo is available.
            var catalog = VM;
            e.CanExecute = UndoService.Current[catalog].CanUndo;
          //  e.CanExecute = true;
        }

        private void Redo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Tell the UI whether Redo is available.
            var catalog = VM;
            e.CanExecute = UndoService.Current[catalog].CanRedo;
           // e.CanExecute = true;
        }

        private void Undo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Get the document root. In this case, we pass in "this", which 
            // implements ISupportsUndo. The ISupportsUndo interface is used
            // by the UndoService to locate the appropriate root node of an 
            // undoable document.
            // In this case, we are treating the window as the root of the undoable
            // document, but in a larger system the root would probably be your
            // domain model.

            //var undoRoot = UndoService.Current[this];
            //undoRoot.Undo();


         var catalog = VM;

           if (null != catalog)
                UndoService.Current[VM].Undo();
        }

        private void Redo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // A shorthand version of the above call to Undo, except 
            // that this calls Redo.
          //UndoService.Current[].Redo();
            var catalog = VM;

            if (null != catalog)
                UndoService.Current[catalog].Redo();
        }



        public IEnumerable<ChangeSet> UndoStack
        {
            get
            {
                return UndoService.Current[this].UndoStack;

            }
        }

        public IEnumerable<ChangeSet> RedoStack
        {
            get
            {
                return UndoService.Current[this].RedoStack;

            }
        }


        #endregion
    }
}
