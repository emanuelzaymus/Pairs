﻿using Pairs.Core;
using Pairs.InterfaceLibrary;
using System.Collections.Generic;
using System.Linq;

namespace Pairs.Server
{
    class GamesManager
    {
        private readonly List<GameOfPlayers> _games = new List<GameOfPlayers>();

        internal void Add(Player firstPlayer, Player secondPlayer, GameLayout gameLayout)
        {
            _games.Add(new GameOfPlayers(firstPlayer, secondPlayer, gameLayout));
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
