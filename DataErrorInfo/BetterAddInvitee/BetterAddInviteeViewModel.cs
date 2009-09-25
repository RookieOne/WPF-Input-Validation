using System;
using System.Text.RegularExpressions;
using System.Windows.Input;
using CommonLibrary.Consts;
using CommonLibrary.Domain;
using CommonLibrary.Extensions;
using CommonLibrary.Wpf;
using DataErrorInfo.Foundation;

namespace DataErrorInfo.BetterAddInvitee
{
    public class BetterAddInviteeViewModel : DataErrorInfoViewModel, IBetterAddInviteeViewModel
    {
        private readonly IPartyInviteeRepository _repository;
        private string _age;
        private string _email;
        private string _name;

        public BetterAddInviteeViewModel(IPartyInviteeRepository repository)
        {
            _repository = repository;
            Add = new ActionCommand(OnAdd);
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(this, v => v.Name);
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(this, v => v.Email);
            }
        }

        public string Age
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
            var invitee = new PartyInvitee(Name, Email, Int32.Parse(Age));
            _repository.Add(invitee);
            Close(this, null);
        }

        protected override string GetErrorFor(string columnName)
        {
            if (columnName == "Name")
                if (string.IsNullOrEmpty(Name))
                    return "Name is required";

            if (columnName == "Age")
            {
                int age;
                if (!Int32.TryParse(Age, out age))
                    return "Age must be a number";

                if (!(age > 18))
                {                    
                    return "Age must be greater than 18";
                }
            }                

            if (columnName == "Email")
            {
                if (string.IsNullOrEmpty(Email))
                    return "Email is required";

                Match match = Regex.Match(Email, RegularExpressions.Email);
                if (!match.Success)
                    return "{0} must match {1}".FormatString(Email, RegularExpressions.Email);
            }

            return base.GetErrorFor(columnName);
        }
    }
}