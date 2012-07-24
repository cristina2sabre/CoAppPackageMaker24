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
    /// Interaction logic for ManifestView.xaml
    /// </summary>
    public partial class ManifestView : UserControl
    {
        public ManifestView()
        {
            InitializeComponent();
            Loaded += MyLoadedRoutedEventHandler;
           
        }

        void MyLoadedRoutedEventHandler(Object sender, RoutedEventArgs e)
        {
            //used for selecting template based on IsEnabled property - Source or Value
            this.ListBoxManifestCollection.ItemTemplate = (DataTemplate)(this.IsEnabled ? FindResource("ManifestTemplate") : FindResource("ManifestTemplateValue"));
            Loaded -= MyLoadedRoutedEventHandler;
        }
    }
}
