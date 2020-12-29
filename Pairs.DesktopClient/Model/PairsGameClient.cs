using Pairs.DesktopClient.Presenter;
using Pairs.InterfaceLibrary;
using Pairs.InterfaceLibrary.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private Player _player;
        private GameLayout _gameLayout;

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

        public delegate void PlayerOnTurnChangedEventHandler(string playerNick);
        public event PlayerOnTurnChangedEventHandler PlayerOnTurnChanged;
        protected virtual void OnPlayerOnTurnChanged() => PlayerOnTurnChanged?.Invoke(_pairsGameService.GetPlayerOnTurn());

        public delegate void ResultsEvaluatedEventHandler(string winner, int[] scores);
        public event ResultsEvaluatedEventHandler ResultsEvaluated;
        protected virtual void OnResultsEvaluated() => ResultsEvaluated?.Invoke(_pairsGameService.GetWinner(), _pairsGameService.GetScores());

        public delegate void OpponentsCardShownEventHandler(Card card);
        public event OpponentsCardShownEventHandler OpponentsCardShown;
        protected virtual void OnOpponentsCardShown(Card card) => OpponentsCardShown?.Invoke(card);

        public delegate void OpponentsCardsHiddenEventHandler(Card card1, Card card2);
        public event OpponentsCardsHiddenEventHandler OpponentsCardsHidden;
        protected virtual void OnOpponentsCardsHidden(Card card1, Card card2) => OpponentsCardsHidden?.Invoke(card1, card2);

        public delegate void OpponentsPairRemovedEventHandler(Card card1, Card card2);
        public event OpponentsPairRemovedEventHandler OpponentsPairRemoved;
        protected virtual void OnOpponentsPairRemoved(Card card1, Card card2) => OpponentsPairRemoved?.Invoke(card1, card2);

        public PairsGameClient()
        {
            _channelFactory = new ChannelFactory<IPairsGameService>("PairsGameEndpoint");
            _pairsGameService = _channelFactory.CreateChannel();
        }

        internal bool TryToLogIn(string nick, string password)
        {
            // TODO: encrypt password !!!
            _player = _pairsGameService.TryToLogIn(nick, password);
            Trace.WriteLine(_player != null ? $"Logged in successfully. Id: {_player.Id}" : "Logging eas not successful.");
            return _player != null;
        }

        internal bool StartNewGame()
        {
            _gameLayout = GameLayout.ThreeTimesTwo;
            bool res = _pairsGameService.StartNewGame(_player.Id, 2, _gameLayout);
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
            // TODO: do not allow make a move when its mot my turn
            int cardNumber = _pairsGameService.NextMove(_player.Id, card.Row, card.Column);
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
                    ReadFromServiceAsync();
                }
            }
            else
            {
                _firstCard = card;
            }
        }

        // TODO: add cancelation token - when I close the app to and this taks in the Dispose() method
        private async void ReadFromServiceAsync()
        {
            Card firstOpponentCard = null;
            while (true)
            {
                await Wait();
                var opponentMoves = _pairsGameService.ReadFromService(_player.Id);
                Trace.WriteLine(_player.Id + " === " + opponentMoves + " COUNT: " + opponentMoves.Count);
                foreach (var move in opponentMoves)
                {
                    OnOpponentsCardShown(move.Card);

                    if (move.IsCompleted)
                    {
                        if (move.IsSuccessful.Value)
                        {
                            OnOpponentsPairRemoved(firstOpponentCard, move.Card);
                            if (_pairsGameService.IsEndOfGame())
                            {
                                OnResultsEvaluated();
                                OnPlayerOnTurnChanged();
                                return;
                            }
                        }
                        else
                        {
                            OnPlayerOnTurnChanged();
                            OnOpponentsCardsHidden(firstOpponentCard, move.Card);
                            return;
                        }
                    }
                    else
                    {
                        firstOpponentCard = move.Card;
                    }
                }
            }
        }

        private async Task Wait()
        {
            await Task.Delay(1000);
        }

        public void Dispose()
        {
            _channelFactory.Close();
        }

    }
}
