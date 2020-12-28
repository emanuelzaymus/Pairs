using System.ServiceModel;

namespace Pairs.InterfaceLibrary
{
    [ServiceContract]
    public interface IPairsGameService
    {
        [OperationContract]
        Player GetPlayer();

        [OperationContract]
        bool StartNewGame(GameLayout gameLayout);

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
        int NextMove(int row, int column);

        [OperationContract]
        bool IsPlayerOnTheTurn(int playerNumber);
    }
}
