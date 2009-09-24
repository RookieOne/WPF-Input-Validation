using System.Windows.Input;
using CommonLibrary.Extensions;
using CommonLibrary.Wpf;
using ValidationRules.ShowPartyInvitees;

namespace MainWpfApplication
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            ShowValidationRulesExample = Show.InWindow<ShowPartyInviteesViewModel>().AsCommand();
            ShowExceptionExample = Show.InWindow<Exceptions.ShowPartyInvitees.ShowPartyInviteesViewModel>().AsCommand();
            
        }

        public ICommand ShowValidationRulesExample { get; set; }
        public ICommand ShowExceptionExample { get; set; }
    }
}