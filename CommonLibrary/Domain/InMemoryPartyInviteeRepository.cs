using System.Collections.Generic;
using System.Linq;

namespace CommonLibrary.Domain
{
    public class InMemoryPartyInviteeRepository : IPartyInviteeRepository
    {
        private readonly List<PartyInvitee> _partyInvitees;

        public InMemoryPartyInviteeRepository()
        {
            _partyInvitees = new List<PartyInvitee>();
        }

        public IEnumerable<PartyInvitee> GetAllInvitees()
        {
            return _partyInvitees.ToList();
        }

        public void Add(PartyInvitee invitee)
        {
            _partyInvitees.Add(invitee);
        }
    }
}