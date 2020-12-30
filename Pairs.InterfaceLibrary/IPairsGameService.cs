using Pairs.InterfaceLibrary.Response;
using System.Collections.Generic;
using System.ServiceModel;

namespace Pairs.InterfaceLibrary
{
    [ServiceContract]
    public interface IPairsGameService
    {
        /// <summary>
        /// </summary>
        /// <param name="nick"></param>
        /// <param name="encryptedPassword"></param>
        /// <returns>Returns ID of logged in player.</returns>
        [OperationContract]
        int? TryToLogIn(string nick, string encryptedPassword);

        [OperationContract]
        bool TryToSignIn(string nick, string encryptedPassword);

        [OperationContract]
        bool LogOut(int playerId);

        [OperationContract]
        List<string> GetAvailablePlayers(int playerId);

        /// <summary>
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="toPlayer"></param>
        /// <param name="gameLayout"></param>
        /// <returns>Whether an invitation for <paramref name="toPlayer"/> was created.</returns>
        [OperationContract]
        bool SendInvitation(int playerId, string toPlayer, GameLayout gameLayout);

        [OperationContract]
        string ReceiveInvitation(int playerId);

        [OperationContract]
        GameLayout AcceptInvitation(int playerId, string fromPlayer, bool isAccepted);

        [OperationContract]
        bool? ReadInvitationReply(int playerId);

        [OperationContract]
        bool StartNewGame(int playerId, int withPlayerId, GameLayout gameLayout);

        [OperationContract]
        int[] GetScores();

        [OperationContract]
        int GetRowCount();

        [OperationContract]
        int GetColumnCount();

        [OperationContract]
        string GetPlayerOnTurn();

        [OperationContract]
        string GetWinner();

        [OperationContract]
        bool GetMoveWasCompleted();

        [OperationContract]
        bool GetWasSuccessfulMove();

        [OperationContract]
        bool IsEndOfGame();

        [OperationContract]
        int NextMove(int playerId, int row, int column);

        [OperationContract]
        bool IsPlayerOnTheTurn(int playerNumber);

        [OperationContract]
        List<OpponentsMove> ReadFromService(int playerId);
    }
}
