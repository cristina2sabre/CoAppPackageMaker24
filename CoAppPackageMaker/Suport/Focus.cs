﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CoAppPackageMaker.Support
{
    public static class Focus
    {

        /// <summary>
        /// A dummy variable. This is used to notify the element that an undo occurred and fires an <see cref="UndoOccurredEvent"/> if the variable is greater than 0;
        /// </summary>
        public static readonly DependencyProperty UndoOccurredIntProperty =
            DependencyProperty.RegisterAttached("UndoOccurredInt", typeof(int), typeof(Focus), new PropertyMetadata(default(int), UndoOccurredIntChanged));

        private static void UndoOccurredIntChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var elem = dependencyObject as UIElement;

            if (elem != null && dependencyPropertyChangedEventArgs.NewValue is int && (int)dependencyPropertyChangedEventArgs.NewValue > 0)
            {
                var args = new RoutedEventArgs(UndoOccurredEvent, elem);
                elem.RaiseEvent(args);
            }

        }

        public static void SetUndoOccurredInt(UIElement element, int value)
        {
            element.SetValue(UndoOccurredIntProperty, value);
        }

        public static int GetUndoOccurred(UIElement element)
        {
            return (int)element.GetValue(UndoOccurredIntProperty);
        }



        /// <summary>
        /// Event fired when an undo has occurred
        /// </summary>
        public static readonly RoutedEvent UndoOccurredEvent =
            EventManager.RegisterRoutedEvent("UndoOccurred", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Focus));


        public static void AddUndoOccurredHandler(DependencyObject d, RoutedEventHandler handler)
        {
            var uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(UndoOccurredEvent, handler);
            }
        }


        public static void RemoveUndoOccurredHandler(DependencyObject d, RoutedEventHandler handler)
        {
            var uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(UndoOccurredEvent, handler);
            }
        }
    }
}