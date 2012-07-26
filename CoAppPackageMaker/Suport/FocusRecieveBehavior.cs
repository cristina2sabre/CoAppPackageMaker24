﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace CoAppPackageMaker.Support
{
    public class FocusReceiveBehavior : Behavior<TabItem>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            Focus.AddUndoOccurredHandler(AssociatedObject, HandleUndoOccurred);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            Focus.RemoveUndoOccurredHandler(AssociatedObject, HandleUndoOccurred);
        }

        private void HandleUndoOccurred(object sender, RoutedEventArgs routedEventArgs)
        {
            if (routedEventArgs.RoutedEvent == Focus.UndoOccurredEvent)
            {
                AssociatedObject.IsSelected = true;
            }
        }
    }
}