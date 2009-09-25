using System;
using System.Windows.Input;
using CommonLibrary.Consts;
using CommonLibrary.Domain;
using CommonLibrary.Wpf;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace EnterpriseLibraryExample.AddInvitee
{
    public class AddInviteeViewModel : ViewModel, IAddInviteeViewModel
    {
        private readonly IPartyInviteeRepository _repository;
        private int _age;
        private string _email;
        private string _name;

        public AddInviteeViewModel(IPartyInviteeRepository repository)
        {
            _repository = repository;
            Add = new ActionCommand(OnAdd);
        }

        [NotNullValidator]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(this, v => v.Name);
            }
        }

        [RegexValidator(RegularExpressions.Email, 
            MessageTemplate = "Email must be in the correct format")]
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(this, v => v.Email);
            }
        }

        [RangeValidator(18, RangeBoundaryType.Inclusive, Int32.MaxValue, RangeBoundaryType.Inclusive,
            MessageTemplate = "Age must be greater than 18")]
        public int Age
        {
            get { return _age; }
            set
            {
                _age = value;
                OnPropertyChanged(this, v => v.Age);
            }
        }

        public ICommand Add { get; set; }

        public event EventHandler Close;

        private void OnAdd()
        {
            var invitee = new PartyInvitee(Name, Email, Age);
            _repository.Add(invitee);
            Close(this, null);
        }
    }
}