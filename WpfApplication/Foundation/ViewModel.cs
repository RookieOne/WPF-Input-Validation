using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace WpfApplication.Foundation
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel">The view model.</param>
        /// <param name="e">The e.</param>
        protected void OnPropertyChanged<T>(T viewModel, Expression<Func<T, object>> e)
        {
            var propertyName = e.GetPropertyName();
            var handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}