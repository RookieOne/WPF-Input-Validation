using System.Windows;
using WpfApplication.Domain;
using WpfApplication.ShowPartyInvitees;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            content.Content = new ShowPartyInviteesViewModel(new InMemoryPartyInviteeRepository());
        }
    }
}