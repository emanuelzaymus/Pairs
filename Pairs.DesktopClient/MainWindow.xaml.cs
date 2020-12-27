using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pairs.DesktopClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PairsGamePresenter _pairsGamePresenter = new PairsGamePresenter();

        private Card _firstCard;

        public MainWindow()
        {
            _pairsGamePresenter.StartNewGame();

            Trace.WriteLine(_pairsGamePresenter.GetColumnCount());
            Thread.Sleep(1000);
            Trace.WriteLine(_pairsGamePresenter.GetColumnCount());
            Thread.Sleep(1000);
            Trace.WriteLine(_pairsGamePresenter.GetColumnCount());

            InitializeComponent();

            int rowCount = _pairsGamePresenter.GetRowCount();
            int columnCount = _pairsGamePresenter.GetColumnCount();

            for (int i = 0; i < rowCount; i++)
            {
                PairGrid.RowDefinitions.Add(new RowDefinition() { MinHeight = 100 });
            }
            for (int i = 0; i < columnCount; i++)
            {
                PairGrid.ColumnDefinitions.Add(new ColumnDefinition() { MinWidth = 100 });
            }

            for (int i = 0; i < rowCount * columnCount; i++)
            {
                Card card = new Card(i / columnCount, i % columnCount);
                Grid.SetRow(card, i / columnCount);
                Grid.SetColumn(card, i % columnCount);
                PairGrid.Children.Add(card);
                card.Click += ClickButton;
            }

            UpdatePlayerOnTurn();
        }

        private void ClickButton(object sender, RoutedEventArgs e)
        {
            Card card = (Card)sender;

            ShowMessage($"{card} => {card.Row} {card.Column}");
            int cardNumber = _pairsGamePresenter.NextMove(card.Row, card.Column);
            ShowCard(card, cardNumber); // Shows front face and disables card

            if (_pairsGamePresenter.GetMoveWasCompleted())
            {
                if (_pairsGamePresenter.GetWasSuccessfulMove())
                {
                    ShowMessage("SUCCESS");
                    if (_pairsGamePresenter.IsEndOfGame())
                    {
                        ShowWinner();
                        UpdatePlayerOnTurn();
                    }
                    RemoveFoundPairAsync(_firstCard, card); // Makes found pair invisible
                }
                else
                {
                    ShowMessage("FAILURE");
                    UpdatePlayerOnTurn();
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
            int winner = _pairsGamePresenter.GetWinner();
            if (winner >= 0)
            {
                int[] scores = _pairsGamePresenter.GetScores();
                ShowMessage($"The winner is player {winner}. (P0: {scores[0]}, P1: {scores[1]})");
            }
            else
            {
                ShowMessage($"It's draw.");
            }
        }

        private void ShowMessage(string msg)
        {
            Message.Content = msg;
        }

        private void UpdatePlayerOnTurn()
        {
            if (!_pairsGamePresenter.IsEndOfGame())
            {
                PlayerOnTurn.Content = _pairsGamePresenter.GetPlayerOnTurn();
            }
            else
            {
                PlayerOnTurn.Content = null;
            }
        }

        private void ShowCard(Card card, int cardNumber)
        {
            card.Show(cardNumber);
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

        protected override void OnClosing(CancelEventArgs e)
        {
            _pairsGamePresenter.Dispose();
            base.OnClosing(e);
        }

    }
}
