using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pairs.DesktopClient
{
    class PairsGamePresenter : IDisposable
    {
        private readonly PairsGameClient _pairsGameClient = new PairsGameClient();


        internal bool StartNewGame()
        {
            return _pairsGameClient.StartNewGame();
        }

        internal int GetRowCount()
        {
            return _pairsGameClient.GetRowCount();
        }

        internal int GetColumnCount()
        {
            return _pairsGameClient.GetColumnCount();
        }

        internal int NextMove(int row, int column)
        {
            return _pairsGameClient.NextMove(row, column);
        }

        internal bool GetMoveWasCompleted()
        {
            return _pairsGameClient.GetMoveWasCompleted();
        }

        internal bool GetWasSuccessfulMove()
        {
            return _pairsGameClient.GetWasSuccessfulMove();
        }

        internal bool IsEndOfGame()
        {
            return _pairsGameClient.IsEndOfGame();
        }

        internal int GetWinner()
        {
            return _pairsGameClient.GetWinner();
        }

        internal int[] GetScores()
        {
            return _pairsGameClient.GetScores();
        }

        internal object GetPlayerOnTurn()
        {
            return _pairsGameClient.GetPlayerOnTurn();
        }

        public void Dispose()
        {
            _pairsGameClient.Dispose();
        }
    }
}
