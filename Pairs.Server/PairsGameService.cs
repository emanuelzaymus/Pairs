using Pairs.Core;
using Pairs.InterfaceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pairs.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    class PairsGameService : IPairsGameService
    {
        private PlayersManager _playersManager = new PlayersManager();
        private GamesManager _gamesManager = new GamesManager();

        private PairsGame _game;

        //public int GetNewPlayerId()
        //{
        //    return _playersManager.GetNewPlayerId();
        //}

        public bool StartNewGame(/*int playerId, int withPlayerId*/)
        {
            // TODO: discover whether withPlayerId player is available (logged in and is not playing)
            // start new game 
            _game = new PairsGame(GameLayout.FourTimesThree);
            return true;
        }

        public int GetColumnCount()
        {
            return _game.ColumnCount;
        }

        public bool GetMoveWasCompleted()
        {
            return _game.MoveWasCompleted;
        }

        public int GetPlayerOnTurn()
        {
            return _game.PlayerOnTheTurn;
        }

        public int GetRowCount()
        {
            return _game.RowCount;
        }

        public int[] GetScores()
        {
            return _game.Scores;
        }

        public bool GetWasSuccessfulMove()
        {
            return _game.WasSuccessfulMove;
        }

        public int GetWinner()
        {
            return _game.Winner;
        }

        public bool IsEndOfGame()
        {
            return _game.IsEndOfGame();
        }

        public bool IsPlayerOnTheTurn(int playerNumber)
        {
            return _game.IsPlayerOnTheTurn(playerNumber);
        }

        public int NextMove(int row, int column)
        {
            return _game.NextMove(row, column);
        }
    }
}
