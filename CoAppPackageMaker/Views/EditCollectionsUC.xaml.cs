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
    /// Interaction logic for EditCollectionsUC.xaml
    /// </summary>
    public partial class EditCollectionsUC : UserControl
    {
        public static DataTemplate ItemTemplate;
        public static DataTemplate DisabledItemTemplate;
        public EditCollectionsUC()
        {
            InitializeComponent();
            DisabledItemTemplate = FindResource("ItemTemplate") as DataTemplate;
            ItemTemplate= FindResource("DisabledItemTemplate") as DataTemplate;
            IsSourceValue = true;
        }

        public static readonly DependencyProperty IsSourceValueProperty =
DependencyProperty.Register("IsSourceValue", typeof(bool), typeof(EditCollectionsUC), new UIPropertyMetadata(MyPropertyChangedHandler));

        public bool IsSourceValue
        {
            get
            {
                return (bool)GetValue(IsSourceValueProperty);
            }
            set
            {
                SetValue(IsSourceValueProperty, value);
            }
        }


        public static void MyPropertyChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
           
            ((EditCollectionsUC)sender).FilesCollections.ItemTemplate = e.NewValue.ToString()=="True"? DisabledItemTemplate :ItemTemplate;
           
        }
    }
}
