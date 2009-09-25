using System.Windows;
using System.Windows.Controls;

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