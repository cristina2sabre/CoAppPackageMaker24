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

namespace CoAppPackageMaker.Views
{
    /// <summary>
    /// Interaction logic for DeveloperLibrary.xaml
    /// </summary>
    public partial class DeveloperLibrary : UserControl
    {
        public DeveloperLibrary()
        {
            InitializeComponent();
            Loaded += MyLoadedRoutedEventHandler;
           
        }

        void MyLoadedRoutedEventHandler(Object sender, RoutedEventArgs e)
        {
            //used for selecting template based on IsEnabled property - Source or Value
            this.ListBoxDeveloperLibraryCollection.ItemTemplate = (DataTemplate)(this.IsEnabled ? FindResource("DeveloperLibraryTemplate") : FindResource("DeveloperLibraryTemplateValue"));
            Loaded -= MyLoadedRoutedEventHandler;
        }
    }
}
