using Pairs.Core;
using Pairs.InterfaceLibrary;
using System.Collections.Generic;
using System.Linq;

namespace Pairs.Server
{
    class GamesManager
    {
        private readonly List<GameOfPlayers> _games = new List<GameOfPlayers>();

        internal void Add(Player firstPlayer, Player SecondPlayer, GameLayout gameLayout)
        {
            _games.Add(new GameOfPlayers(firstPlayer, SecondPlayer, gameLayout));
        }

        internal PairsGame GetGame(int playerId)
        {
            return GetGameOfPlayers(playerId).Game;
        }

        internal GameOfPlayers GetGameOfPlayers(int playerId)
        {
            return _games.FirstOrDefault(x => x.FirstPlayer.Id == playerId || x.SecondPlayer.Id == playerId);
        }

    }
}
