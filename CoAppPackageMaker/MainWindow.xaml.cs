using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
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
            this.DataContext = MainWindowViewModel.Instance;
           
        }


        #region View Model Accessor
        //to avoid calls to getInstance for Undo/Redo canExecute
      
        private  MainWindowViewModel VM
        {
            //get { return _vM ?? (_vM = MainWindowViewModel.Instance); }
            get { return DataContext as MainWindowViewModel; } 
        }

        #endregion

        /// <summary>
        ///Monitored Undo Framework
        /// </summary>
        /// <from> http://muf.codeplex.com/ </from>
        #region Event Handlers


        //// The following 4 event handlers support the "CommandBindings" in the window.
        // These hook to the Undo and Redo commands.

        private void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Tell the UI whether Undo is available.
            var catalog = VM;
            if (catalog != null)
            e.CanExecute = UndoService.Current[catalog].CanUndo;
         
        }

        private void Redo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Tell the UI whether Redo is available.
            var catalog = VM;
            if(catalog!=null)
            e.CanExecute = UndoService.Current[catalog].CanRedo;
          
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

            var root = VM;

           if (null != root)
                UndoService.Current[root].Undo();
           
        }

        private void Redo_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            var root = VM;
            if (null != root)
                UndoService.Current[root].Redo();
        }


        #endregion

        //private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        //{

        //}

        private void filterError_Click(object sender, RoutedEventArgs e)
        {
            var collectionView = CollectionViewSource.GetDefaultView(MainWindowViewModel.Instance.ErrorsCollection);
            if ((bool)filterError.IsChecked && (bool)filterWarning.IsChecked)
            {
                collectionView.Filter = o =>  true;
            }
            else if (!(bool)filterError.IsChecked && !(bool)filterWarning.IsChecked)
            {
                collectionView.Filter = o => false;
            }
            else if ((bool)filterWarning.IsChecked)
            {
                collectionView.Filter = o => (o is Warning) ;

            }
            else if ((bool)filterError.IsChecked)
            {
                collectionView.Filter = o => !(o is Warning);
            }
            
            collectionView.Refresh();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".autopkg";
            dlg.Filter = "Text documents (.autopkg)|*.autopkg";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                //txtFileName.Text = filename;
                VM.PathToFile = filename;
                VM.LoadData();
                UndoService.Current[this.VM].Clear();

            }
        }

       
    }
}
