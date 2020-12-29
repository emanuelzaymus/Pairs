using Pairs.Core;
using Pairs.InterfaceLibrary;
using Pairs.InterfaceLibrary.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private List<OpponentsMove> _moveBuffer = new List<OpponentsMove>();

        public Player TryToLogIn(string nick, string encryptedPassword)
        {
            return _playersManager.GetPlayer(nick, encryptedPassword);
        }

        public bool TryToSignIn(string nick, string encryptedPassword)
        {
            return _playersManager.AddPlayer(nick, encryptedPassword);
        }

        public bool StartNewGame(int playerId, int withPlayerId, GameLayout gameLayout)
        {
            // TODO: discover whether withPlayerId player is available (logged in and is not playing)
            // start new game 
            _game = new PairsGame(gameLayout, playerId, withPlayerId);
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

        public string GetPlayerOnTurn()
        {
            int playerOnTheTurn = _game.PlayerIdOnTurn;
            if (playerOnTheTurn == 1)
            {
                return "Adam";
            }
            else if (playerOnTheTurn == 2)
            {
                return "Zoro";
            }
            return null;
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

        public string GetWinner()
        {
            int winner = _game.Winner;
            if (winner == 1)
            {
                return "Adam";
            }
            else if (winner == 2)
            {
                return "Zoro";
            }
            return null;
        }

        public bool IsEndOfGame()
        {
            return _game.IsEndOfGame();
        }

        public bool IsPlayerOnTheTurn(int playerNumber)
        {
            return _game.IsPlayerOnTheTurn(playerNumber);
        }

        public int NextMove(int playerId, int row, int column)
        {
            try
            {
                int cardNumber = _game.NextMove(playerId, row, column);

                bool? isSuccessful = _game.MoveWasCompleted ? _game.WasSuccessfulMove : (bool?)null;
                _moveBuffer.Add(new OpponentsMove(new Card(row, column, cardNumber), isSuccessful));

                return cardNumber;
            }
            catch (Exception e)
            {
                Trace.WriteLine("Wrong PLAYER !!!");
                Trace.WriteLine(e);
                return -1000;
            }
        }

        public List<OpponentsMove> ReadFromService(int playerId)
        {
            Trace.WriteLine("Server - ReadFromServer with id: " + playerId);
            List<OpponentsMove> moves = _moveBuffer.Select(x => x).ToList();
            _moveBuffer.Clear();
            return moves;
        }

    }
}
