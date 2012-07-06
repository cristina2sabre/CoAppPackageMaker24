using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CoAppPackageMaker.Temp
{

   
    public static class FocusExtension
    {

        public static readonly DependencyProperty HighlightTextOnFocusProperty =
            DependencyProperty.RegisterAttached(
                "HighlightTextOnFocus", typeof(bool), typeof(FocusExtension),
                new PropertyMetadata(true, HighlightTextOnFocusPropertyChanged));


        [AttachedPropertyBrowsableForChildrenAttribute(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetHighlightTextOnFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(HighlightTextOnFocusProperty);
        }

        public static void SetHighlightTextOnFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(HighlightTextOnFocusProperty, value);
        }

        private static void HighlightTextOnFocusPropertyChanged(DependencyObject obj,
                                                                DependencyPropertyChangedEventArgs e)
        {
            var sender = obj as UIElement;
            if (obj != null)
            {
                if ((bool)e.NewValue)
                {
                    sender.GotKeyboardFocus += OnKeyboardFocusSelectText;
                   // sender.PreviewMouseLeftButtonDown += OnMouseLeftButtonDownSetFocus;
                }
                else
                {
                    sender.GotKeyboardFocus -= OnKeyboardFocusSelectText;
                   // sender.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDownSetFocus;
                }
            }
        }

        private static void OnKeyboardFocusSelectText(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
            {
                textBox.SelectAll();
            }
        }

        //private static void OnMouseLeftButtonDownSetFocus(object sender, MouseButtonEventArgs e)
        //{
        //    DependencyObject a = e.OriginalSource as DependencyObject;
        //// VisualTreeHelpers.FindAncestor((DependencyObject)e.OriginalSource);

        //   VisualTreeHelpers.FindAncestor<DependencyObject>((DependencyObject)e.OriginalSource);

        //  //  <Signatur, bool>(new Signatur());


        //    //if (tb == null)
        //    //    return;

        //    //if (!tb.IsKeyboardFocusWithin)
        //    //{
        //    //    tb.Focus();
        //    //    e.Handled = true;
        //    //}
        //}


        ////////////////////////////////////////
        
        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.RegisterAttached("IsFocused", typeof(bool?), typeof(FocusExtension), new FrameworkPropertyMetadata(IsFocusedChanged));

        public static bool? GetIsFocused(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return (bool?)element.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject element, bool? value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(IsFocusedProperty, value);
        }

        private static void IsFocusedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = (FrameworkElement)d;

            if (e.OldValue == null)
            {
                fe.GotFocus += FrameworkElement_GotFocus;
                fe.LostFocus += FrameworkElement_LostFocus;
            }

            if (!fe.IsVisible)
            {
                fe.IsVisibleChanged += new DependencyPropertyChangedEventHandler(fe_IsVisibleChanged);
            }

            if ((bool)e.NewValue)
            {
                fe.Focus();
            }
        }

        private static void fe_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var fe = (FrameworkElement)sender;
            if (fe.IsVisible && (bool)((FrameworkElement)sender).GetValue(IsFocusedProperty))
            {
                fe.IsVisibleChanged -= fe_IsVisibleChanged;
                fe.Focus();
            }
        }

        private static void FrameworkElement_GotFocus(object sender, RoutedEventArgs e)
        {
            ((FrameworkElement)sender).SetValue(IsFocusedProperty, true);
        }

        private static void FrameworkElement_LostFocus(object sender, RoutedEventArgs e)
        {
            ((FrameworkElement)sender).SetValue(IsFocusedProperty, false);
        }
    }

  
 
}


