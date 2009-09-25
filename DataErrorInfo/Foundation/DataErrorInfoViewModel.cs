using System.ComponentModel;
using CommonLibrary.Wpf;

namespace DataErrorInfo.Foundation
{
    public class DataErrorInfoViewModel : ViewModel, IDataErrorInfo
    {
        private string _error;

        public string this[string columnName]
        {
            get { return GetErrorFor(columnName); }
        }

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged(this, v => v.Error);
            }
        }

        protected virtual string GetErrorFor(string columnName)
        {
            return string.Empty;
        }
    }
}