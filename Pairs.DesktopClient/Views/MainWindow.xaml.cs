using Pairs.DesktopClient.Presenter;
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

namespace Pairs.DesktopClient.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PairsGamePresenter _pairsGamePresenter = new PairsGamePresenter();

        public MainWindow()
        {
            InitializeComponent();

            _pairsGamePresenter.MessageShown += ShowMessage;
            _pairsGamePresenter.PlayerOnTurnUpdated += UpdatePlayerOnTurn;

            _pairsGamePresenter.GetCard = GetCard;

            ShowLogInWindow();
        }

        private void ShowLogInWindow()
        {
            new LogInWindow(LogIn, ShowSignInWindow).ShowDialog();
        }

        private void ShowSignInWindow(LogInWindow logInWindow)
        {
            new SignInWindow(SignIn, logInWindow).ShowDialog();
        }

        private void LogIn(LogInWindow logInWindow, PlayerCredentials playerCredentials)
        {
            bool success = _pairsGamePresenter.TryToLogIn(playerCredentials);
            if (success)
            {
                logInWindow.ExitAppOnClose = false;
                logInWindow.Close();
            }
            else
                logInWindow.ShowUnsuccessMessage();
        }

        private void SignIn(SignInWindow signInWindow, LogInWindow logInWindow, PlayerCredentials playerCredentials)
        {
            bool success = _pairsGamePresenter.TryToSignIn(playerCredentials);
            if (success)
            {
                signInWindow.Close();
                LogIn(logInWindow, playerCredentials);
            }
            else signInWindow.ShowNickAlreadyExistsMessage();
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            var availablePlayers = _pairsGamePresenter.GetAvailablePlayers();
            new NewGameWindow(availablePlayers) { Owner = this }.ShowDialog();
        }

        private void StartNewGame(NewGameWindow newGameWindow, NewGame newGame)
        {
            ClearGameBoard();

            _pairsGamePresenter.StartNewGame();

            int rowCount = _pairsGamePresenter.GetRowCount();
            int columnCount = _pairsGamePresenter.GetColumnCount();
            SetNewGameBoard(rowCount, columnCount);
        }

        private void SetNewGameBoard(int rowCount, int columnCount)
        {
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
                CardButton card = new CardButton(i / columnCount, i % columnCount);
                Grid.SetRow(card, i / columnCount);
                Grid.SetColumn(card, i % columnCount);
                PairGrid.Children.Add(card);
                card.Click += CardButton_Click;
            }
        }

        private void ClearGameBoard()
        {
            PairGrid.Children.Clear();
            PairGrid.RowDefinitions.Clear();
            PairGrid.ColumnDefinitions.Clear();

            ShowMessage(null);
            UpdatePlayerOnTurn(null);
        }

        private void CardButton_Click(object sender, RoutedEventArgs e)
        {
            CardButton card = (CardButton)sender;
            _pairsGamePresenter.NextMove(card);
        }

        private void ShowMessage(string msg)
        {
            Message.Content = msg;
        }

        private void UpdatePlayerOnTurn(string playerOnTurn)
        {
            PlayerOnTurn.Content = playerOnTurn;
        }

        private CardButton GetCard(int row, int column)
        {
            foreach (CardButton cardBtn in PairGrid.Children)
            {
                if (cardBtn.Row == row && cardBtn.Column == column)
                    return cardBtn;
            }
            return null;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _pairsGamePresenter.Dispose();
            base.OnClosing(e);
        }

    }
}
