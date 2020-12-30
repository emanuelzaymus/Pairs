using Pairs.InterfaceLibrary;

namespace Pairs.Server
{
    internal class Invitation
    {
        public Player FromPlayer { get; }

        public Player ToPlayer { get; }

        public GameLayout GameLayout { get; }

        public Invitation(Player fromPlayer, Player toPlayer, GameLayout gameLayout)
        {
            FromPlayer = fromPlayer;
            ToPlayer = toPlayer;
            GameLayout = gameLayout;
        }
    }
}