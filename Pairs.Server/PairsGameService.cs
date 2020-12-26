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

        public int GetNewPlayerId()
        {
            return _playersManager.GetNewPlayerId();
        }

        public bool StartNewGame(int playerId, int withPlayerId)
        {
            // TODO: discover whether withPlayerId player is available (logged in and is not playing)
            // start new game 
            return true;
        }

        public int GetColumnCount()
        {
            Console.WriteLine("sending 555.....");
            return 555555;
        }

        public bool GetMoveWasCompleted()
        {
            throw new NotImplementedException();
        }

        public int GetPlayerOnTurn()
        {
            throw new NotImplementedException();
        }

        public int GetRowCount()
        {
            throw new NotImplementedException();
        }

        public int[] GetScores()
        {
            throw new NotImplementedException();
        }

        public bool GetWasSuccessfulMove()
        {
            throw new NotImplementedException();
        }

        public int GetWinner()
        {
            throw new NotImplementedException();
        }

        public bool IsEndOfGame()
        {
            throw new NotImplementedException();
        }

        public bool IsPlayerOnTheTurn(int playerNumber)
        {
            throw new NotImplementedException();
        }

        public int NextMove(int row, int column)
        {
            throw new NotImplementedException();
        }
    }
}
