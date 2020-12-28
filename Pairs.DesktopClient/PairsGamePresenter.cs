using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pairs.DesktopClient
{
    class PairsGamePresenter : IDisposable
    {
        private readonly PairsGameClient _pairsGameClient;

        public delegate void MessageShownMethod(string message);
        public event MessageShownMethod MessageShown;

        public delegate void PlayerOnTurnUpdatedMethod(string playerOnTurn);
        public event PlayerOnTurnUpdatedMethod PlayerOnTurnUpdated;

        //private Card _firstCard;

        public PairsGamePresenter()
        {
            _pairsGameClient = new PairsGameClient();
            _pairsGameClient.CardShown += ShowCard;
            _pairsGameClient.CardsHidden += HideCardsAsync;
            _pairsGameClient.FoundPairRemoved += RemoveFoundPairAsync;
            _pairsGameClient.PlayerOnTurnChanged += ChangePlayerOnTurn;
            _pairsGameClient.WinnerSet += SetWinner;
        }

        internal bool StartNewGame()
        {
            bool res = _pairsGameClient.StartNewGame();
            UpdatePlayerOnTurn();
            return res;
        }

        internal int GetRowCount()
        {
            return _pairsGameClient.GetRowCount();
        }

        internal int GetColumnCount()
        {
            return _pairsGameClient.GetColumnCount();
        }

        internal void NextMove(Card card)
        {
            _pairsGameClient.NextMove(card);


            //MessageShown($"{card} => {card.Row} {card.Column}");
            //int cardNumber = _pairsGameClient.NextMove(card.Row, card.Column);
            //ShowCard(card, cardNumber);

            //if (_pairsGameClient.GetMoveWasCompleted())
            //{
            //    if (_pairsGameClient.GetWasSuccessfulMove())
            //    {
            //        MessageShown("SUCCESS");
            //        if (_pairsGameClient.IsEndOfGame())
            //        {
            //            ShowWinner();
            //            UpdatePlayerOnTurn();
            //        }
            //        RemoveFoundPairAsync(_firstCard, card); // Makes found pair invisible
            //    }
            //    else
            //    {
            //        MessageShown("FAILURE");
            //        UpdatePlayerOnTurn();
            //        HideCardsAsync(_firstCard, card); // Hides front faces and enables all cards
            //    }
            //}
            //else
            //{
            //    _firstCard = card;
            //}
        }

        private void ShowCard(Card card, int cardNumber)
        {
            MessageShown($"{card} => {card.Row} {card.Column}");
            card.Show(cardNumber);
        }

        //private void ShowWinner()
        //{
        //    int winner = _pairsGameClient.GetWinner();
        //    if (winner >= 0)
        //    {
        //        int[] scores = _pairsGameClient.GetScores();
        //        MessageShown($"The winner is player {winner}. (P0: {scores[0]}, P1: {scores[1]})");
        //    }
        //    else
        //    {
        //        MessageShown($"It's draw.");
        //    }
        //}

        private void SetWinner(int winner)
        {
            if (winner >= 0)
            {
                int[] scores = _pairsGameClient.GetScores();
                MessageShown($"The winner is player {winner}. (P0: {scores[0]}, P1: {scores[1]})");
            }
            else
            {
                MessageShown($"It's draw.");
            }
            ChangePlayerOnTurn(-100);
        }

        private void UpdatePlayerOnTurn()
        {
            if (!_pairsGameClient.IsEndOfGame())
            {
                PlayerOnTurnUpdated(_pairsGameClient.GetPlayerOnTurn().ToString());
            }
            else
            {
                PlayerOnTurnUpdated(null);
            }
        }

        private void ChangePlayerOnTurn(int playerNumber)
        {
            PlayerOnTurnUpdated(playerNumber.ToString());
        }

        private async void HideCardsAsync(Card card1, Card card2)
        {
            MessageShown("FAILURE");
            await Wait();
            card1.Hide();
            card2.Hide();
        }

        private async void RemoveFoundPairAsync(Card card1, Card card2)
        {
            MessageShown("SUCCESS");
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
