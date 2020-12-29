using Pairs.DesktopClient.Model;
using Pairs.InterfaceLibrary.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pairs.DesktopClient.Presenter
{
    class PairsGamePresenter : IDisposable
    {
        private readonly PairsGameClient _pairsGameClient = new PairsGameClient();

        public delegate void MessageShownEventHander(string message);
        public event MessageShownEventHander MessageShown;
        protected virtual void OnMessageShown(string message) => MessageShown?.Invoke(message);

        public delegate void PlayerOnTurnUpdatedEventHandler(string playerOnTurn);
        public event PlayerOnTurnUpdatedEventHandler PlayerOnTurnUpdated;
        protected virtual void OnPlayerOnTurnUpdated(string playerOnTurn) => PlayerOnTurnUpdated?.Invoke(playerOnTurn);

        public delegate ICard GetCardHandler(int row, int column);
        public GetCardHandler GetCard;

        public PairsGamePresenter()
        {
            _pairsGameClient.CardShown += ShowCard;
            _pairsGameClient.CardsHidden += HideCardsAsync;
            _pairsGameClient.FoundPairRemoved += RemoveFoundPairAsync;
            _pairsGameClient.PlayerOnTurnChanged += ChangePlayerOnTurn;
            _pairsGameClient.ResultsEvaluated += ShowResults;

            _pairsGameClient.OpponentsCardShown += ShowOpponentsCard;
            _pairsGameClient.OpponentsCardsHidden += HideOpponentsCards;
            _pairsGameClient.OpponentsPairRemoved += RemoveOpponentsPair;
        }

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

        internal void NextMove(ICard card)
        {
            _pairsGameClient.NextMove(card);
        }

        private void ChangePlayerOnTurn(string playerNick)
        {
            OnPlayerOnTurnUpdated(playerNick != null ? playerNick : "---");
        }

        private async void HideCardsAsync(ICard card1, ICard card2)
        {
            OnMessageShown("FAILURE");
            await Wait();
            card1.Hide();
            card2.Hide();
        }

        private async void RemoveFoundPairAsync(ICard card1, ICard card2)
        {
            OnMessageShown("SUCCESS");
            await Wait();
            card1.Remove();
            card2.Remove();
        }

        private void ShowResults(string winner, int[] scores)
        {
            OnMessageShown((winner != null ? $"The winner is player {winner}." : $"It's draw.")
                + $" (First Pl: {scores[0]}, Second Pl: {scores[1]})");
        }

        private void ShowCard(ICard card, int cardNumber)
        {
            OnMessageShown($"{card} => {card.Row} {card.Column}");
            card.Show(cardNumber);
        }

        private void ShowOpponentsCard(Card card)
        {
            ICard c = GetCard(card.Row, card.Column);
            ShowCard(c, card.CardNumber);
        }

        private void HideOpponentsCards(Card card1, Card card2)
        {
            ICard c1 = GetCard(card1.Row, card1.Column);
            ICard c2 = GetCard(card2.Row, card2.Column);
            HideCardsAsync(c1, c2);
        }

        private void RemoveOpponentsPair(Card card1, Card card2)
        {
            ICard c1 = GetCard(card1.Row, card1.Column);
            ICard c2 = GetCard(card2.Row, card2.Column);
            RemoveFoundPairAsync(c1, c2);
        }

        private async Task Wait()
        {
            await Task.Delay(500);
        }

        public void Dispose()
        {
            _pairsGameClient.Dispose();
        }

    }
}
