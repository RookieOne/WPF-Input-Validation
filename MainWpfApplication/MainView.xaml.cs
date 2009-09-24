using System.Windows;

namespace MainWpfApplication
{
    public partial class MainView : Window
    {
        public MainView()
        {
            DataContext = new MainViewModel();
            InitializeComponent();
        }
    }
}