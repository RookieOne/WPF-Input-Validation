using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Input;
using CommonLibrary.Consts;
using CommonLibrary.Domain;
using CommonLibrary.Wpf;

namespace Exceptions.AddInvitee
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
        
        public string Name
        {
            get { return _name; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new Exception("Name is required");

                _name = value;
                OnPropertyChanged(this, v => v.Name);
            }
        }
        
        public string Email
        {
            get { return _email; }
            set
            {
                if (!Regex.Match(value, RegularExpressions.Email).Success)
                    throw new Exception("Email is in incorrect format");

                _email = value;
                OnPropertyChanged(this, v => v.Email);
            }
        }
        
        public int Age
        {
            get { return _age; }
            set
            {
                if (value < 18)
                    throw new Exception("Age must be greater than 18");

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