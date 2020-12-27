using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

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

    //[DataContract]
    //public class CompositeType
    //{
    //    bool boolValue = true;
    //    string stringValue = "Hello ";

    //    [DataMember]
    //    public bool BoolValue
    //    {
    //        get { return boolValue; }
    //        set { boolValue = value; }
    //    }

    //    [DataMember]
    //    public string StringValue
    //    {
    //        get { return stringValue; }
    //        set { stringValue = value; }
    //    }
    //}
}
