using Pairs.DesktopClient.Presenter;
using Pairs.InterfaceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pairs.DesktopClient.Model
{
    class PairsGameClient : IDisposable
    {
        private readonly ChannelFactory<IPairsGameService> _channelFactory;
        private readonly IPairsGameService _pairsGameService;

        private ICard _firstCard;

        public delegate void CardShownEventHandler(ICard card, int cardNumber);
        public event CardShownEventHandler CardShown;
        protected virtual void OnCardShown(ICard card, int cardNumber) => CardShown?.Invoke(card, cardNumber);

        public delegate void CardsHiddenEventHandler(ICard card1, ICard card2);
        public event CardsHiddenEventHandler CardsHidden;
        protected virtual void OnCardsHidden(ICard card1, ICard card2) => CardsHidden?.Invoke(card1, card2);

        public delegate void PairRemovedEventHandler(ICard card1, ICard card2);
        public event PairRemovedEventHandler FoundPairRemoved;
        protected virtual void OnFoundPairRemoved(ICard card1, ICard card2) => FoundPairRemoved?.Invoke(card1, card2);

        public delegate void PlayerOnTurnChangedEventHandler(int playerNumber);
        public event PlayerOnTurnChangedEventHandler PlayerOnTurnChanged;
        protected virtual void OnPlayerOnTurnChanged() => PlayerOnTurnChanged?.Invoke(_pairsGameService.GetPlayerOnTurn());

        public delegate void ResultsEvaluatedEventHandler(int winner, int[] scores);
        public event ResultsEvaluatedEventHandler ResultsEvaluated;
        protected virtual void OnResultsEvaluated() => ResultsEvaluated?.Invoke(_pairsGameService.GetWinner(), _pairsGameService.GetScores());

        public PairsGameClient()
        {
            _channelFactory = new ChannelFactory<IPairsGameService>("PairsGameEndpoint");
            _pairsGameService = _channelFactory.CreateChannel();
        }

        internal bool StartNewGame()
        {
            bool res = _pairsGameService.StartNewGame();
            OnPlayerOnTurnChanged();
            return res;
        }

        internal int GetRowCount()
        {
            return _pairsGameService.GetRowCount();
        }

        internal int GetColumnCount()
        {
            return _pairsGameService.GetColumnCount();
        }

        internal void NextMove(ICard card)
        {
            int cardNumber = _pairsGameService.NextMove(card.Row, card.Column);
            OnCardShown(card, cardNumber);

            if (_pairsGameService.GetMoveWasCompleted())
            {
                if (_pairsGameService.GetWasSuccessfulMove())
                {
                    OnFoundPairRemoved(_firstCard, card);
                    if (_pairsGameService.IsEndOfGame())
                    {
                        OnResultsEvaluated();
                        OnPlayerOnTurnChanged();
                    }
                }
                else
                {
                    OnPlayerOnTurnChanged();
                    OnCardsHidden(_firstCard, card);
                }
            }
            else
            {
                _firstCard = card;
            }
        }

        public void Dispose()
        {
            _channelFactory.Close();
        }

    }
}
