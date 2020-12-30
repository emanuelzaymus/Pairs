using Pairs.InterfaceLibrary;
using Pairs.InterfaceLibrary.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;

namespace Pairs.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    class PairsGameService : IPairsGameService, IDisposable
    {
        private const string PlayerManagerFilePath = "PlayerManager.json";

        private readonly PlayersManager _playersManager;
        private readonly InvitationsManager _invitationsManager;
        private readonly GamesManager _gamesManager = new GamesManager();

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
            if (invitation != null)
            {
                _invitationsManager.AddInvitationReply(invitation, isAccepted);

                Player firstPlayer = _playersManager.GetPlayer(fromPlayer);
                Player secondPlayer = _playersManager.GetPlayer(playerId);
                firstPlayer.IsPlaying = isAccepted;
                secondPlayer.IsPlaying = isAccepted;

                if (isAccepted)
                {
                    // Create new game
                    _gamesManager.Add(firstPlayer, secondPlayer, invitation.GameLayout);
                    // Create invitation reply for invitation.FromPlayer
                    return invitation.GameLayout;
                }
            }
            return null;
        }

        public bool? ReadInvitationReply(int playerId)
        {
            return _invitationsManager.GetInvitationReply(playerId);
        }

        public bool GetMoveWasCompleted(int playerId)
        {
            return _gamesManager.GetGame(playerId).MoveWasCompleted;
        }

        public int GetPlayerOnTurn(int playerId)
        {
            return _gamesManager.GetGame(playerId).PlayerIdOnTurn;
        }

        public int[] GetScores(int playerId)
        {
            return _gamesManager.GetGame(playerId).Scores;
        }

        public bool GetWasSuccessfulMove(int playerId)
        {
            return _gamesManager.GetGame(playerId).WasSuccessfulMove;
        }

        public int GetWinner(int playerId)
        {
            return _gamesManager.GetGame(playerId).Winner;
        }

        public bool IsEndOfGame(int playerId)
        {
            return _gamesManager.GetGame(playerId).IsEndOfGame();
        }

        public bool IsPlayerOnTheTurn(int playerId, int playerNumber)
        {
            return _gamesManager.GetGame(playerId).IsPlayerOnTheTurn(playerNumber);
        }

        public int NextMove(int playerId, int row, int column)
        {
            try
            {
                int cardNumber = _gamesManager.GetGame(playerId).NextMove(playerId, row, column);

                _gamesManager.GetGameOfPlayers(playerId).AddOpponentsMove(row, column, cardNumber);

                return cardNumber;
            }
            catch (Exception e)
            {
                Trace.WriteLine("Wrong PLAYER !!!");
                Trace.WriteLine(e);
                return -1000;
            }
        }

        public List<OpponentsMove> ReadOpponentsMoves(int playerId)
        {
            Trace.WriteLine("Server - ReadFromServer with id: " + playerId);
            return _gamesManager.GetGameOfPlayers(playerId).PopAllOpponentsMove();
        }

        public void Dispose()
        {
            _playersManager.ToJsonFile(PlayerManagerFilePath);
        }

    }
}
