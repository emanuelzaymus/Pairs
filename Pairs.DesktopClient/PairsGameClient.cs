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

        private Card _firstCard;

        public delegate void CardShownEventHandler(Card card, int cardNumber);
        public event CardShownEventHandler CardShown;
        private void OnCardShown(Card card, int cardNumber) => CardShown?.Invoke(card, cardNumber);

        public delegate void CardsHiddenEventHandler(Card card1, Card card2);
        public event CardsHiddenEventHandler CardsHidden;
        private void OnCardsHidden(Card card1, Card card2) => CardsHidden?.Invoke(card1, card2);

        public delegate void PairRemovedEventHandler(Card card1, Card card2);
        public event PairRemovedEventHandler FoundPairRemoved;
        private void OnFoundPairRemoved(Card card1, Card card2) => FoundPairRemoved?.Invoke(card1, card2);

        public delegate void PlayerOnTurnChangedEventHandler(int playerNumber);
        public event PlayerOnTurnChangedEventHandler PlayerOnTurnChanged;
        private void OnPlayerOnTurnChanged(int playerNumber) => PlayerOnTurnChanged?.Invoke(playerNumber);

        public delegate void WinnerSetEventHandler(int winner);
        public event WinnerSetEventHandler WinnerSet;
        private void OnWinnerSet(int winner) => WinnerSet?.Invoke(winner);

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

        //internal int NextMove(int row, int column)
        //{
        //    return _pairsGameService.NextMove(row, column);
        //}

        internal void NextMove(Card card)
        {
            int cardNumber = _pairsGameService.NextMove(card.Row, card.Column);
            OnCardShown(card, cardNumber);

            if (_pairsGameService.GetMoveWasCompleted())
            {
                if (_pairsGameService.GetWasSuccessfulMove())
                {
                    //MessageShown("SUCCESS");
                    if (_pairsGameService.IsEndOfGame())
                    {
                        //ShowWinner();
                        OnWinnerSet(_pairsGameService.GetWinner());
                        //UpdatePlayerOnTurn();
                    }
                    //RemoveFoundPairAsync(_firstCard, card); // Makes found pair invisible
                    OnFoundPairRemoved(_firstCard, card);
                }
                else
                {
                    //MessageShown("FAILURE");
                    //UpdatePlayerOnTurn();
                    OnPlayerOnTurnChanged(_pairsGameService.GetPlayerOnTurn());
                    //HideCardsAsync(_firstCard, card); // Hides front faces and enables all cards
                    OnCardsHidden(_firstCard, card);
                }
            }
            else
            {
                _firstCard = card;
            }
        }


        //internal bool GetMoveWasCompleted()
        //{
        //    return _pairsGameService.GetMoveWasCompleted();
        //}

        //internal bool GetWasSuccessfulMove()
        //{
        //    return _pairsGameService.GetWasSuccessfulMove();
        //}

        internal bool IsEndOfGame()
        {
            return _pairsGameService.IsEndOfGame();
        }

        //internal int GetWinner()
        //{
        //    return _pairsGameService.GetWinner();
        //}

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
