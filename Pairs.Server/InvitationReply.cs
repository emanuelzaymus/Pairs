namespace Pairs.Server
{
    internal class InvitationReply
    {
        public Player FromPlayer { get; }

        public Player ToPlayer { get; }

        public bool IsAccepted { get; }

        public InvitationReply(Invitation invitation, bool isAccepted)
        {
            FromPlayer = invitation.FromPlayer;
            ToPlayer = invitation.ToPlayer;
            IsAccepted = isAccepted;
        }
    }
}