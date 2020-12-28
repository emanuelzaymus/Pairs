using Pairs.DesktopClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pairs.DesktopClient.Presenter
{
    class PairsGamePresenter : IDisposable
    {
        private readonly PairsGameClient _pairsGameClient;

        public delegate void MessageShownEventHander(string message);
        public event MessageShownEventHander MessageShown;
        protected virtual void OnMessageShown(string message) => MessageShown?.Invoke(message);

        public delegate void PlayerOnTurnUpdatedEventHandler(string playerOnTurn);
        public event PlayerOnTurnUpdatedEventHandler PlayerOnTurnUpdated;
        protected virtual void OnPlayerOnTurnUpdated(string playerOnTurn) => PlayerOnTurnUpdated?.Invoke(playerOnTurn);

        public PairsGamePresenter()
        {
            _pairsGameClient = new PairsGameClient();
            _pairsGameClient.CardShown += ShowCard;
            _pairsGameClient.CardsHidden += HideCardsAsync;
            _pairsGameClient.FoundPairRemoved += RemoveFoundPairAsync;
            _pairsGameClient.PlayerOnTurnChanged += ChangePlayerOnTurn;
            _pairsGameClient.ResultsEvaluated += ShowResults;
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

        private void ShowCard(ICard card, int cardNumber)
        {
            OnMessageShown($"{card} => {card.Row} {card.Column}");
            card.Show(cardNumber);
        }

        private void ShowResults(int winner, int[] scores)
        {
            OnMessageShown((winner >= 0 ? $"The winner is player {winner}." : $"It's draw.") + $" (P0: {scores[0]}, P1: {scores[1]})");
        }

        private void ChangePlayerOnTurn(int playerNumber)
        {
            OnPlayerOnTurnUpdated(playerNumber >= 0 ? playerNumber.ToString() : "---");
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

        private async Task Wait()
        {
            await Task.Delay(1000);
        }

        public void Dispose()
        {
            _pairsGameClient.Dispose();
        }

    }
}
