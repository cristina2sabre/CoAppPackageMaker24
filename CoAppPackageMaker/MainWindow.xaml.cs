using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using CoApp.Packaging;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;
using Warning = CoAppPackageMaker.ViewModels.Base.Warning;


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
            if (!SaveCancelled())
            {
                // Create OpenFileDialog
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                // Set filter for file extension and default file extension
                dlg.DefaultExt = ".autopkg";
                dlg.Filter = "Autopkg files (.autopkg)|*.autopkg";

                // Display OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = dlg.ShowDialog();

                // Get the selected file name and display in a TextBox
                if (result == true)
                {
                    // Open document
                    string filename = dlg.FileName;
                    VM.PathToFile = filename;
                    VM.LoadData();
                    UndoService.Current[this.VM].Clear();

                }
            }
          
        }

        private bool SaveCancelled()
        {
            bool cancelled = false;
            bool modificationExist = UndoService.Current[VM].CanUndo;
            if (modificationExist)
            {
                MessageBoxResult result2 = MessageBox.Show("Would you like to save the modifications?",
                                                           "CoApp Package Maker",
                                                           MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result2 == MessageBoxResult.Cancel)
                {
                    cancelled = true;
                }

                if (result2 == MessageBoxResult.Yes)
                {
                    //this.VM.PathToFile;
                    this.VM.Reader.Save("D:\\P\\COPKG\\testsave.autopkg");
                }
            }
            return cancelled;
        }



        private void New_Click(object sender, RoutedEventArgs e)
        {

            if (!SaveCancelled())
            {
                //in order to delete old changes
                this.VM.ResetExecute();
                var dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.Title = "Select the location for saving";
                dlg.FileName = "New"; // Default file name
                dlg.DefaultExt = ".autopkg"; // Default file extension
                dlg.Filter = "Autopkg files (.autopkg)|*.autopkg"; // Filter files by extension

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    string filename = dlg.FileName;
                    //TextWriter tw = new StreamWriter(filename);
                    //tw.WriteLine(String.Format("{0}{1}{2}{3}{4}", "@import", "\"", "outercurve.inc", "\"", ";"));
                    //tw.Close();
                    VM.PathToFile = filename;
                    VM.LoadData();
                }
            }

        }


    }
}
