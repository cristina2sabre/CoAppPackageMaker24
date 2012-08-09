using System;
using System.Windows;
using System.Windows.Controls;

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
            this.ListBoxManifestCollection.ItemTemplate = (DataTemplate)(this.Uid=="Source" ? FindResource("ManifestTemplate") : FindResource("ManifestTemplateValue"));
            Loaded -= MyLoadedRoutedEventHandler;
        }
    }
}
