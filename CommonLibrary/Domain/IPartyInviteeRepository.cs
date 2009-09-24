using System.Collections.Generic;

namespace CommonLibrary.Domain
{
    public interface IPartyInviteeRepository
    {
        IEnumerable<PartyInvitee> GetAllInvitees();
        void Add(PartyInvitee invitee);
    }
}