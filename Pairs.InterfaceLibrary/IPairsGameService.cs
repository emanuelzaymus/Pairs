using System.ServiceModel;

namespace Pairs.InterfaceLibrary
{
    [ServiceContract]
    public interface IPairsGameService
    {
        [OperationContract]
        bool StartNewGame();

        [OperationContract]
        int[] GetScores();

        [OperationContract]
        int GetRowCount();

        [OperationContract]
        int GetColumnCount();

        [OperationContract]
        int GetPlayerOnTurn();

        [OperationContract]
        int GetWinner();

        [OperationContract]
        bool GetMoveWasCompleted();

        [OperationContract]
        bool GetWasSuccessfulMove();

        [OperationContract]
        bool IsEndOfGame();

        [OperationContract]
        int NextMove(int row, int column);

        [OperationContract]
        bool IsPlayerOnTheTurn(int playerNumber);
    }
}
