using System;
using System.Windows;
using System.Windows.Controls;

namespace CoAppPackageMaker.Views
{
    /// <summary>
    /// Interaction logic for SigningView.xaml
    /// </summary>
    public partial class SigningView : UserControl
    {

        public SigningView()
        {
            InitializeComponent();
            Loaded += MyLoadedRoutedEventHandler;
        }
        
        void MyLoadedRoutedEventHandler(Object sender, RoutedEventArgs e)
        {
            //used for selecting template based on IsEnabled property - Source or Value
          this.SingningInclude.ListTemplate = this.IsEnabled ? EditCollectionsUC.SelectTemplate.SourceValueTemplate : EditCollectionsUC.SelectTemplate.ValueTemplate;
          Loaded -= MyLoadedRoutedEventHandler;
        }
       
    }
}
