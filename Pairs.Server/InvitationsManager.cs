using Pairs.InterfaceLibrary;
using System.Collections.Generic;
using System.Linq;

namespace Pairs.Server
{
    class InvitationsManager
    {
        private readonly PlayersManager _playersManager;

        private readonly List<Invitation> _invitations = new List<Invitation>();

        private readonly List<InvitationReply> _invitationReplies = new List<InvitationReply>();

        public InvitationsManager(PlayersManager playersManager)
        {
            _playersManager = playersManager;
        }

        internal bool AddInvitation(int playerId, string toPlayerNick, GameLayout gameLayout)
        {
            Player fromPlayer = _playersManager.GetPlayer(playerId);
            fromPlayer.IsPlaying = true;
            Player toPlayer = _playersManager.GetPlayer(toPlayerNick);
            if (toPlayer.IsOnline && !toPlayer.IsPlaying)
            {
                _invitations.Add(new Invitation(fromPlayer, toPlayer, gameLayout));
                return true;
            }
            return false;
        }

        internal Invitation GetInvitationFor(int playerId)
        {
            return _invitations.FirstOrDefault(i => i.ToPlayer.Id == playerId);
        }

        internal Invitation PopInvitationFor(int toPlayerId)
        {
            var foundInvitation = _invitations.FirstOrDefault(i => i.ToPlayer.Id == toPlayerId);
            if (foundInvitation != null)
            {
                _invitations.Remove(foundInvitation);
            }
            return foundInvitation;
        }

        internal void AddInvitationReply(Invitation invitation, bool isAccepted)
        {
            _invitationReplies.Add(new InvitationReply(invitation, isAccepted));
        }

        internal bool? GetInvitationReply(int fromPlayerId)
        {
            var reply = _invitationReplies.FirstOrDefault(x => x.FromPlayer.Id == fromPlayerId);
            if (reply != null)
            {
                _invitationReplies.Remove(reply);
                return reply.IsAccepted;
            }
            return null;
        }

    }
}
