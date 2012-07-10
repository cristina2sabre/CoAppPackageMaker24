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
            this.DataContext = MainWindowViewModel.Instance;
        }


        #region View Model Accessor
        //to avoid calls to getInstance for Undo/Redo canExecute
        private static MainWindowViewModel _vM;
        private static MainWindowViewModel VM
        {
            get { return _vM ?? (_vM = MainWindowViewModel.Instance); }
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
    }
}
