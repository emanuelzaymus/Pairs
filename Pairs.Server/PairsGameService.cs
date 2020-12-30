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
    class PairsGameService : IPairsGameService, IDisposable
    {
        private const string PlayerManagerFilePath = "PlayerManager.json";

        private PlayersManager _playersManager;
        private InvitationsManager _invitationsManager;
        private GamesManager _gamesManager = new GamesManager();

        private PairsGame _game;

        private List<OpponentsMove> _moveBuffer = new List<OpponentsMove>();

        public PairsGameService()
        {
            _playersManager = PlayersManager.FromJsonFile(PlayerManagerFilePath);
            _invitationsManager = new InvitationsManager(_playersManager);
        }

        public int? TryToLogIn(string nick, string encryptedPassword)
        {
            return _playersManager.LogInPlayer(nick, encryptedPassword);
        }

        public bool TryToSignIn(string nick, string encryptedPassword)
        {
            return _playersManager.AddPlayer(nick, encryptedPassword);
        }

        public bool LogOut(int playerId)
        {
            return _playersManager.LogOut(playerId);
        }

        public List<string> GetAvailablePlayers(int playerId)
        {
            return _playersManager.OnlinePlayers.Where(p => p.Id != playerId).Select(p => p.Nick).ToList();
        }

        public bool SendInvitation(int playerId, string toPlayer, GameLayout gameLayout)
        {
            return _invitationsManager.AddInvitation(playerId, toPlayer, gameLayout);
        }

        public string ReceiveInvitation(int playerId)
        {
            Invitation invitation = _invitationsManager.GetInvitationFor(playerId);
            if (invitation != null)
            {
                return invitation.FromPlayer.Nick;
            }
            return null;
        }

        public GameLayout AcceptInvitation(int playerId, string fromPlayer, bool isAccepted)
        {
            Invitation invitation = _invitationsManager.PopInvitationFor(playerId);
            if (isAccepted && invitation != null)
            {
                // Create new game
                _gamesManager.Add(_playersManager.GetPlayer(fromPlayer), _playersManager.GetPlayer(playerId), invitation.GameLayout);
                // Create invitation reply for invitation.FromPlayer
                _invitationsManager.AddInvitationReply(invitation, isAccepted);
                return invitation.GameLayout;
            }
            return null;
        }

        public bool? ReadInvitationReply(int playerId)
        {
            return _invitationsManager.GetInvitationReply(playerId);
        }

        public bool StartNewGame(int playerId, int withPlayerId, GameLayout gameLayout)
        {
            // TODO: discover whether withPlayerId player is available (logged in and is not playing)
            // start new game 
            // TODO: set Player.IsPlaying to true
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

        public void Dispose()
        {
            _playersManager.ToJsonFile(PlayerManagerFilePath);
        }

    }
}
