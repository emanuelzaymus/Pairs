using Pairs.InterfaceLibrary.Response;
using System.Collections.Generic;
using System.ServiceModel;

namespace Pairs.InterfaceLibrary
{
    [ServiceContract]
    public interface IPairsGameService
    {
        [OperationContract]
        Player TryToLogIn(string nick, string encryptedPassword);

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
