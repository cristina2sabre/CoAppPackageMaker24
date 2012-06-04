using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{

    /// <summary>
    /// If PropertyChanged Event Raised, and check if  property name is correct
    /// </summary>
    /// <from>http://code.msdn.microsoft.com/How-to-implement-MVVM-71a65441</from>
    public class ViewModelTest
    {
        protected void CheckPropertyChangedRaised<T>(Action<T> action, string propertyName)
            where T : INotifyPropertyChanged, new()
        {
            var eventRaised = false;

            var viewModel = new T();

            viewModel
                .PropertyChanged +=
                (s, e) => CheckPropertyName(ref eventRaised, e, propertyName);

            action(viewModel);

            Assert.IsTrue(eventRaised, "Event not raised.");
        }

        protected void CheckPropertyName(ref bool eventRaised, PropertyChangedEventArgs e, string propertyName)
        {
            eventRaised = true;

            Assert.AreEqual(e.PropertyName, propertyName);
        }
    }
}
