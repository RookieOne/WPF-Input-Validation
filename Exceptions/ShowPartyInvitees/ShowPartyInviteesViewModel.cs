using System.Collections.Generic;
using System.Windows.Input;
using CommonLibrary.Domain;
using CommonLibrary.Wpf;
using Exceptions.AddInvitee;

namespace Exceptions.ShowPartyInvitees
{
    public class ShowPartyInviteesViewModel : ViewModel
    {
        private readonly IPartyInviteeRepository _repository;
        private IAddInviteeViewModel _addInviteeViewModel;
        private IEnumerable<PartyInvitee> _partyInvitees;

        public ShowPartyInviteesViewModel(IPartyInviteeRepository repository)
        {
            _repository = repository;
            Add = new ActionCommand(OnAdd);
            RefreshList();
        }

        public IEnumerable<PartyInvitee> PartyInvitees
        {
            get { return _partyInvitees; }
            set
            {
                _partyInvitees = value;
                OnPropertyChanged(this, v => v.PartyInvitees);
            }
        }

        public ICommand Add { get; set; }

        public IAddInviteeViewModel AddInviteeViewModel
        {
            get { return _addInviteeViewModel; }
            set
            {
                _addInviteeViewModel = value;
                OnPropertyChanged(this, v => v.AddInviteeViewModel);
            }
        }

        private void OnAdd()
        {
            AddInviteeViewModel = new AddInviteeViewModel(_repository);
            AddInviteeViewModel.Close += (sender, e) =>
                                             {
                                                 AddInviteeViewModel = null;
                                                 RefreshList();
                                             };
        }

        private void RefreshList()
        {
            PartyInvitees = _repository.GetAllInvitees();
        }
    }
}