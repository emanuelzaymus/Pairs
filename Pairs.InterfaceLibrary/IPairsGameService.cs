using Pairs.InterfaceLibrary.Response;
using System.Collections.Generic;
using System.ServiceModel;

namespace Pairs.InterfaceLibrary
{
    [ServiceContract]
    public interface IPairsGameService
    {
        /// <returns>ID of logged in player.</returns>
        [OperationContract]
        int? TryToLogIn(string nick, string encryptedPassword);

        [OperationContract]
        bool TryToSignIn(string nick, string encryptedPassword);

        [OperationContract]
        bool LogOut(int playerId);

        [OperationContract]
        List<string> GetAvailablePlayers(int playerId);

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
        int[] GetScores(int playerId);

        [OperationContract]
        int GetPlayerOnTurn(int playerId);

        [OperationContract]
        int GetWinner(int playerId);

        [OperationContract]
        bool GetMoveWasCompleted(int playerId);

        [OperationContract]
        bool GetWasSuccessfulMove(int playerId);

        [OperationContract]
        bool IsEndOfGame(int playerId);

        [OperationContract]
        int NextMove(int playerId, int row, int column);

        [OperationContract]
        bool IsPlayerOnTheTurn(int playerId);

        [OperationContract]
        List<OpponentsMove> ReadOpponentsMoves(int playerId);

        [OperationContract]
        void EndGame(int playerId);
    }
}
