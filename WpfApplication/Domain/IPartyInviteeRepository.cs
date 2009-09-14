using System.Collections.Generic;

namespace WpfApplication.Domain
{
    public interface IPartyInviteeRepository
    {
        IEnumerable<PartyInvitee> GetAllInvitees();
        void Add(PartyInvitee invitee);
    }
}