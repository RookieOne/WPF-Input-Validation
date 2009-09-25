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
            ShowDataErrorInfoExample =
                Show.InWindow<DataErrorInfo.ShowPartyInvitees.ShowPartyInviteesViewModel>().AsCommand();
            ShowEnterpriseLibraryExample =
                Show.InWindow<EnterpriseLibraryExample.ShowPartyInvitees.ShowPartyInviteesViewModel>().AsCommand();
            ShowCustomMarkupExtensionExample =
                Show.InWindow<CustomMarkupExtension.ShowPartyInvitees.ShowPartyInviteesViewModel>().AsCommand();
        }

        public ICommand ShowValidationRulesExample { get; set; }
        public ICommand ShowExceptionExample { get; set; }
        public ICommand ShowDataErrorInfoExample { get; set; }
        public ICommand ShowEnterpriseLibraryExample { get; set; }
        public ICommand ShowCustomMarkupExtensionExample { get; set; }
    }
}