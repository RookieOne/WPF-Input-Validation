using WpfApplication.Foundation;

namespace WpfApplication.BindingValidation
{
    public class BindingViewModel : ViewModel
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged(this, (m) => m.Text);
            }
        }
    }
}