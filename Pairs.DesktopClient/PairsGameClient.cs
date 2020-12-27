using Pairs.InterfaceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pairs.DesktopClient
{
    class PairsGameClient : IDisposable
    {
        private readonly ChannelFactory<IPairsGameService> _channelFactory;
        private readonly IPairsGameService _pairsGameService;

        public PairsGameClient()
        {
            _channelFactory = new ChannelFactory<IPairsGameService>("PairsGameEndpoint");
            _pairsGameService = _channelFactory.CreateChannel();
        }

        internal bool StartNewGame()
        {
            return _pairsGameService.StartNewGame();
        }

        internal int GetRowCount()
        {
            return _pairsGameService.GetRowCount();
        }

        internal int GetColumnCount()
        {
            return _pairsGameService.GetColumnCount();
        }

        internal int NextMove(int row, int column)
        {
            return _pairsGameService.NextMove(row, column);
        }

        internal bool GetMoveWasCompleted()
        {
            return _pairsGameService.GetMoveWasCompleted();
        }

        internal bool GetWasSuccessfulMove()
        {
            return _pairsGameService.GetWasSuccessfulMove();
        }

        internal bool IsEndOfGame()
        {
            return _pairsGameService.IsEndOfGame();
        }

        internal int GetWinner()
        {
            return _pairsGameService.GetWinner();
        }

        internal int[] GetScores()
        {
            return _pairsGameService.GetScores();
        }

        internal object GetPlayerOnTurn()
        {
            return _pairsGameService.GetPlayerOnTurn();
        }

        public void Dispose()
        {
            _channelFactory.Close();
        }
    }
}
