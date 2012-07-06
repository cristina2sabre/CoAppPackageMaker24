using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Threading;

namespace CoAppPackageMaker.Temp
{
    /// <summary>
    /// Implemented by a ViewModel that needs to control
    /// where input focus is in a View.
    /// </summary>
    public interface IFocusMover
    {
        /// <summary>
        /// Raised when the input focus should move to 
        /// a control whose 'active' dependency property 
        /// is bound to the specified property.
        /// </summary>
        event EventHandler<MoveFocusEventArgs> MoveFocus;
    }

    public class MoveFocusEventArgs : EventArgs
    {
        public MoveFocusEventArgs(string focusedProperty)
        {
            this.FocusedProperty = focusedProperty;
        }

        public string FocusedProperty { get; private set; }
    }

    // <summary>
    /// A base class for custom markup extension which provides properties
    /// that can be found on regular <see cref="Binding"/> markup extension.
    /// </summary>
    [MarkupExtensionReturnType(typeof(object))]
    public abstract class BindingDecoratorBase : MarkupExtension
    {
        /// <summary>
        /// The decorated binding class.
        /// </summary>
        private Binding binding = new Binding();


        //check documentation of the Binding class for property information

        #region properties

        /// <summary>
        /// The decorated binding class.
        /// </summary>
        [Browsable(false)]
        public Binding Binding
        {
            get { return binding; }
            set { binding = value; }
        }


        [DefaultValue(null)]
        public object AsyncState
        {
            get { return binding.AsyncState; }
            set { binding.AsyncState = value; }
        }

        [DefaultValue(false)]
        public bool BindsDirectlyToSource
        {
            get { return binding.BindsDirectlyToSource; }
            set { binding.BindsDirectlyToSource = value; }
        }

        [DefaultValue(null)]
        public IValueConverter Converter
        {
            get { return binding.Converter; }
            set { binding.Converter = value; }
        }

        [TypeConverter(typeof(CultureInfoIetfLanguageTagConverter)), DefaultValue(null)]
        public CultureInfo ConverterCulture
        {
            get { return binding.ConverterCulture; }
            set { binding.ConverterCulture = value; }
        }

        [DefaultValue(null)]
        public object ConverterParameter
        {
            get { return binding.ConverterParameter; }
            set { binding.ConverterParameter = value; }
        }

        [DefaultValue(null)]
        public string ElementName
        {
            get { return binding.ElementName; }
            set { binding.ElementName = value; }
        }

        [DefaultValue(null)]
        public object FallbackValue
        {
            get { return binding.FallbackValue; }
            set { binding.FallbackValue = value; }
        }

        [DefaultValue(false)]
        public bool IsAsync
        {
            get { return binding.IsAsync; }
            set { binding.IsAsync = value; }
        }

        [DefaultValue(BindingMode.Default)]
        public BindingMode Mode
        {
            get { return binding.Mode; }
            set { binding.Mode = value; }
        }

        [DefaultValue(false)]
        public bool NotifyOnSourceUpdated
        {
            get { return binding.NotifyOnSourceUpdated; }
            set { binding.NotifyOnSourceUpdated = value; }
        }

        [DefaultValue(false)]
        public bool NotifyOnTargetUpdated
        {
            get { return binding.NotifyOnTargetUpdated; }
            set { binding.NotifyOnTargetUpdated = value; }
        }

        [DefaultValue(false)]
        public bool NotifyOnValidationError
        {
            get { return binding.NotifyOnValidationError; }
            set { binding.NotifyOnValidationError = value; }
        }

        [DefaultValue(null)]
        public PropertyPath Path
        {
            get { return binding.Path; }
            set { binding.Path = value; }
        }

        [DefaultValue(null)]
        public RelativeSource RelativeSource
        {
            get { return binding.RelativeSource; }
            set { binding.RelativeSource = value; }
        }

        [DefaultValue(null)]
        public object Source
        {
            get { return binding.Source; }
            set { binding.Source = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter
        {
            get { return binding.UpdateSourceExceptionFilter; }
            set { binding.UpdateSourceExceptionFilter = value; }
        }

        [DefaultValue(UpdateSourceTrigger.Default)]
        public UpdateSourceTrigger UpdateSourceTrigger
        {
            get { return binding.UpdateSourceTrigger; }
            set { binding.UpdateSourceTrigger = value; }
        }

        [DefaultValue(false)]
        public bool ValidatesOnDataErrors
        {
            get { return binding.ValidatesOnDataErrors; }
            set { binding.ValidatesOnDataErrors = value; }
        }

        [DefaultValue(false)]
        public bool ValidatesOnExceptions
        {
            get { return binding.ValidatesOnExceptions; }
            set { binding.ValidatesOnExceptions = value; }
        }

        [DefaultValue(null)]
        public string XPath
        {
            get { return binding.XPath; }
            set { binding.XPath = value; }
        }

        [DefaultValue(null)]
        public Collection<ValidationRule> ValidationRules
        {
            get { return binding.ValidationRules; }
        }

        #endregion



        /// <summary>
        /// This basic implementation just sets a binding on the targeted
        /// <see cref="DependencyObject"/> and returns the appropriate
        /// <see cref="BindingExpressionBase"/> instance.<br/>
        /// All this work is delegated to the decorated <see cref="Binding"/>
        /// instance.
        /// </summary>
        /// <returns>
        /// The object value to set on the property where the extension is applied. 
        /// In case of a valid binding expression, this is a <see cref="BindingExpressionBase"/>
        /// instance.
        /// </returns>
        /// <param name="provider">Object that can provide services for the markup
        /// extension.</param>
        public override object ProvideValue(IServiceProvider provider)
        {
            //create a binding and associate it with the target
            return binding.ProvideValue(provider);
        }



        /// <summary>
        /// Validates a service provider that was submitted to the <see cref="ProvideValue"/>
        /// method. This method checks whether the provider is null (happens at design time),
        /// whether it provides an <see cref="IProvideValueTarget"/> service, and whether
        /// the service's <see cref="IProvideValueTarget.TargetObject"/> and
        /// <see cref="IProvideValueTarget.TargetProperty"/> properties are valid
        /// <see cref="DependencyObject"/> and <see cref="DependencyProperty"/>
        /// instances.
        /// </summary>
        /// <param name="provider">The provider to be validated.</param>
        /// <param name="target">The binding target of the binding.</param>
        /// <param name="dp">The target property of the binding.</param>
        /// <returns>True if the provider supports all that's needed.</returns>
        protected virtual bool TryGetTargetItems(IServiceProvider provider, out DependencyObject target, out DependencyProperty dp)
        {
            target = null;
            dp = null;
            if (provider == null) return false;

            //create a binding and assign it to the target
            IProvideValueTarget service = (IProvideValueTarget)provider.GetService(typeof(IProvideValueTarget));
            if (service == null) return false;

            //we need dependency objects / properties
            target = service.TargetObject as DependencyObject;
            dp = service.TargetProperty as DependencyProperty;
            return target != null && dp != null;
        }

    }
    public class FocusBinding : BindingDecoratorBase
    {
        public override object ProvideValue(IServiceProvider provider)
        {
            DependencyObject elem;
            DependencyProperty prop;
            if (base.TryGetTargetItems(provider, out elem, out prop))
            {
                FocusController.SetFocusableProperty(elem, prop);
            }

            return base.ProvideValue(provider);
        }
    }


    internal static class FocusController
    {
        #region FocusableProperty

        internal static DependencyProperty GetFocusableProperty(DependencyObject obj)
        {
            return (DependencyProperty)obj.GetValue(FocusablePropertyProperty);
        }

        internal static void SetFocusableProperty(DependencyObject obj, DependencyProperty value)
        {
            obj.SetValue(FocusablePropertyProperty, value);
        }

        internal static readonly DependencyProperty FocusablePropertyProperty =
            DependencyProperty.RegisterAttached(
            "FocusableProperty",
            typeof(DependencyProperty),
            typeof(FocusController),
            new UIPropertyMetadata(null, OnFocusablePropertyChanged));

        static void OnFocusablePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null)
                return;

            var property = e.NewValue as DependencyProperty;
            if (property == null)
                return;

            element.DataContextChanged += delegate
            {
                CreateHandler(element, property);
            };

            if (element.DataContext != null)
            {
                CreateHandler(element, property);
            }
        }

        static void CreateHandler(DependencyObject element, DependencyProperty property)
        {
            var focusMover = element.GetValue(FrameworkElement.DataContextProperty) as IFocusMover;
            if (focusMover == null)
            {
                var handler = element.GetValue(MoveFocusSinkProperty) as MoveFocusSink;
                if (handler != null)
                {
                    handler.ReleaseReferences();
                    element.ClearValue(MoveFocusSinkProperty);
                }
            }
            else
            {
                var handler = new MoveFocusSink(element as UIElement, property);
                focusMover.MoveFocus += handler.HandleMoveFocus;
                element.SetValue(MoveFocusSinkProperty, handler);
            }
        }

        #endregion // FocusableProperty

        #region MoveFocusSink

        static readonly DependencyProperty MoveFocusSinkProperty = DependencyProperty.RegisterAttached(
            "MoveFocusSink",
            typeof(MoveFocusSink),
            typeof(FocusController),
            new UIPropertyMetadata(null));

        private class MoveFocusSink
        {
            public MoveFocusSink(UIElement element, DependencyProperty property)
            {
                _element = element;
                _property = property;
            }

            internal void HandleMoveFocus(object sender, MoveFocusEventArgs e)
            {
                if (_element == null || _property == null)
                    return;

                var binding = BindingOperations.GetBinding(_element, _property);
                if (binding == null)
                    return;

                if (e.FocusedProperty != binding.Path.Path)
                    return;

                // Delay the call to allow the current batch
                // of processing to finish before we shift focus.
                _element.Dispatcher.BeginInvoke((Action)delegate
                {
                    _element.Focus();
                },
                DispatcherPriority.Background);
            }

            internal void ReleaseReferences()
            {
                _element = null;
                _property = null;
            }

            UIElement _element;
            DependencyProperty _property;
        }

        #endregion // MoveFocusSink
    }
}
