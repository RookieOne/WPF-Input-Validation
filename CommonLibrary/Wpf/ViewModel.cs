using System;
using System.ComponentModel;
using System.Linq.Expressions;
using CommonLibrary.Extensions;

namespace CommonLibrary.Wpf
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged<T>(T viewModel, Expression<Func<T, object>> e)
        {
            string propertyName = e.GetPropertyName();
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}