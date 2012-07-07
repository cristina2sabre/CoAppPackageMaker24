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
using CoAppPackageMaker.ViewModels.RuleViewModels;

namespace CoAppPackageMaker.Views
{
    /// <summary>
    /// Interaction logic for EditCollectionsUC.xaml
    /// </summary>
    public partial class EditCollectionsUC : UserControl
    {
        public static DataTemplate ValueTemplate;
        public static DataTemplate SourceValueTemplate;
        public static DataTemplate DefineValueTemplate;
        public static DataTemplate DefineSourceValueTemplate;
       
        public EditCollectionsUC()
        {
            InitializeComponent();
            ValueTemplate = FindResource("ValueTemplate") as DataTemplate;
            SourceValueTemplate = FindResource("SourceValueTemplate") as DataTemplate;
            DefineValueTemplate = FindResource("DefineValueTemplate") as DataTemplate;
            DefineSourceValueTemplate = FindResource("DefineSourceValueTemplate") as DataTemplate;
        }

        public static readonly DependencyProperty IsSourceValueProperty =
DependencyProperty.RegisterAttached("ListTemplate", typeof(SelectTemplate), typeof(EditCollectionsUC), new UIPropertyMetadata(MyPropertyChangedHandler));

        public SelectTemplate ListTemplate
        {
            get
            {
                return (SelectTemplate)GetValue(IsSourceValueProperty);
            }
            set
            {
                SetValue(IsSourceValueProperty, value);
            }
        }

        public enum SelectTemplate
        {
            Template,//for raise change in enum -never used
            SourceValueTemplate,
            ValueTemplate,
            DefineSourceValueTemplate,
            DefineValueTemplate,
            
        }

       
        public static void MyPropertyChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
          
                switch (e.NewValue.ToString())
            {
                case "SourceValueTemplate":
                    ((EditCollectionsUC)sender).FilesCollections.ItemTemplate = SourceValueTemplate;
                    break;
                case "ValueTemplate":
                    ((EditCollectionsUC)sender).FilesCollections.ItemTemplate = ValueTemplate;
                    break;
                case "DefineSourceValueTemplate":
                    ((EditCollectionsUC)sender).FilesCollections.ItemTemplate = DefineSourceValueTemplate;
                  
                    break;
                case "DefineValueTemplate":
                    ((EditCollectionsUC)sender).FilesCollections.ItemTemplate = DefineValueTemplate;
                  
                    break;
         
            }
        }

        
    }

   
}
