using Pairs.Core;
using Pairs.InterfaceLibrary;

namespace Pairs.Server
{
    internal class GameOfPlayers
    {
        public Player FirstPlayer { get; }

        public Player SecondPlayer { get; }

        public PairsGame Game { get; set; }

        public GameOfPlayers(Player firstPlayer, Player secondPlayer, GameLayout gameLayout)
        {
            FirstPlayer = firstPlayer;
            SecondPlayer = secondPlayer;
            Game = new PairsGame(gameLayout, firstPlayer.Id, secondPlayer.Id);
        }

    }
}