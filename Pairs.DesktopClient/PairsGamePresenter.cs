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

        public delegate void ShowMessageMethod(string message);
        public event ShowMessageMethod ShowMessage;

        public delegate void UpdatePlayerOnTurnMethod(string playerOnTurn);
        public event UpdatePlayerOnTurnMethod UpdatePlayerOnTurn;

        private Card _firstCard;

        internal bool StartNewGame()
        {
            bool res = _pairsGameClient.StartNewGame();
            OnUpdatePlayerOnTurn();
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

        //internal int NextMove(int row, int column)
        //{
        //    return _pairsGameClient.NextMove(row, column);
        //}

        internal void NextMove(Card card)
        {
            ShowMessage($"{card} => {card.Row} {card.Column}");
            int cardNumber = _pairsGameClient.NextMove(card.Row, card.Column);
            card.Show(cardNumber);
            //ShowCard(card, cardNumber); // Shows front face and disables card

            if (_pairsGameClient.GetMoveWasCompleted())
            {
                if (_pairsGameClient.GetWasSuccessfulMove())
                {
                    ShowMessage("SUCCESS");
                    if (_pairsGameClient.IsEndOfGame())
                    {
                        ShowWinner();
                        OnUpdatePlayerOnTurn();
                    }
                    RemoveFoundPairAsync(_firstCard, card); // Makes found pair invisible
                }
                else
                {
                    ShowMessage("FAILURE");
                    OnUpdatePlayerOnTurn();
                    HideCardsAsync(_firstCard, card); // Hides front faces and enables all cards
                }
            }
            else
            {
                _firstCard = card;
            }
        }

        private void ShowWinner()
        {
            int winner = _pairsGameClient.GetWinner();
            if (winner >= 0)
            {
                int[] scores = _pairsGameClient.GetScores();
                ShowMessage($"The winner is player {winner}. (P0: {scores[0]}, P1: {scores[1]})");
            }
            else
            {
                ShowMessage($"It's draw.");
            }
        }

        private void OnUpdatePlayerOnTurn()
        {
            if (!_pairsGameClient.IsEndOfGame())
            {
                UpdatePlayerOnTurn(_pairsGameClient.GetPlayerOnTurn().ToString());
            }
            else
            {
                UpdatePlayerOnTurn(null);
            }
        }

        private async void HideCardsAsync(Card card1, Card card2)
        {
            await Wait();
            card1.Hide();
            card2.Hide();
        }

        private async void RemoveFoundPairAsync(Card card1, Card card2)
        {
            await Wait();
            card1.Remove();
            card2.Remove();
        }

        private async Task Wait()
        {
            await Task.Delay(1000);
        }

        //internal bool GetMoveWasCompleted()
        //{
        //    return _pairsGameClient.GetMoveWasCompleted();
        //}

        //internal bool GetWasSuccessfulMove()
        //{
        //    return _pairsGameClient.GetWasSuccessfulMove();
        //}

        //internal bool IsEndOfGame()
        //{
        //    return _pairsGameClient.IsEndOfGame();
        //}

        //internal int GetWinner()
        //{
        //    return _pairsGameClient.GetWinner();
        //}

        //internal int[] GetScores()
        //{
        //    return _pairsGameClient.GetScores();
        //}

        //internal object GetPlayerOnTurn()
        //{
        //    return _pairsGameClient.GetPlayerOnTurn();
        //}

        public void Dispose()
        {
            _pairsGameClient.Dispose();
        }

    }
}
