using System.Collections.Generic;
using System.Windows.Input;
using CommonLibrary.Domain;
using CommonLibrary.Wpf;
using DataErrorInfo.AddInvitee;
using DataErrorInfo.BetterAddInvitee;

namespace DataErrorInfo.ShowPartyInvitees
{
    public class ShowPartyInviteesViewModel : ViewModel
    {
        private readonly IPartyInviteeRepository _repository;
        private IAddInviteeViewModel _addInviteeViewModel;
        private IBetterAddInviteeViewModel _betterAddInviteeViewModel;
        private IEnumerable<PartyInvitee> _partyInvitees;

        public ShowPartyInviteesViewModel(IPartyInviteeRepository repository)
        {
            _repository = repository;
            Add = new ActionCommand(OnAdd);
            BetterAdd = new ActionCommand(OnBetterAdd);
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
        public ICommand BetterAdd { get; set; }

        public IAddInviteeViewModel AddInviteeViewModel
        {
            get { return _addInviteeViewModel; }
            set
            {
                _addInviteeViewModel = value;
                OnPropertyChanged(this, v => v.AddInviteeViewModel);
            }
        }

        public IBetterAddInviteeViewModel BetterAddInviteeViewModel
        {
            get { return _betterAddInviteeViewModel; }
            set
            {
                _betterAddInviteeViewModel = value;
                OnPropertyChanged(this, v => v.BetterAddInviteeViewModel);
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

        private void OnBetterAdd()
        {
            BetterAddInviteeViewModel = new BetterAddInviteeViewModel(_repository);
            BetterAddInviteeViewModel.Close += (sender, e) =>
                                             {
                                                 BetterAddInviteeViewModel = null;
                                                 RefreshList();
                                             };
        }

        private void RefreshList()
        {
            PartyInvitees = _repository.GetAllInvitees();
        }
    }
}