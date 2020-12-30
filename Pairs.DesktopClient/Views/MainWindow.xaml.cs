using Pairs.DesktopClient.Presenter;
using Pairs.InterfaceLibrary;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Pairs.DesktopClient.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PairsGamePresenter _pairsGamePresenter = new PairsGamePresenter();

        private MessageWindow _messageWindow;

        public MainWindow()
        {
            InitializeComponent();

            _pairsGamePresenter.PlayerOnTurnUpdated += UpdatePlayerOnTurn;

            _pairsGamePresenter.GetCard = GetCard;

            _pairsGamePresenter.InvitationReceived += ReceiveInvitation;
            _pairsGamePresenter.InvitationReplyReceived += ReceiveInvitationReply;
            _pairsGamePresenter.AcceptedGameStarted += StartNewGame;

            _pairsGamePresenter.ScoreAdded += AddPoint;
            _pairsGamePresenter.ResultsEvaluated += ShowResults;

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
                YourNickLabel.Content = playerCredentials.Nick;
            }
            else logInWindow.ShowUnsuccessMessage();
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
            new NewGameWindow(availablePlayers, SendInvitation) { Owner = this }.ShowDialog();
        }

        private void SendInvitation(NewGameData newGameData)
        {
            bool succes = _pairsGamePresenter.SendInvitation(newGameData);
            if (succes)
                ShowMessageWindow("Wait for the game to start...", "Invitation sent");
            else
                MessageBox.Show(this, $"Player {newGameData.WithPlayer} is busy now. Try again later.", "Player busy", MessageBoxButton.OK);
        }

        private void ShowMessageWindow(string message, string title)
        {
            _messageWindow = new MessageWindow(message, title) { Owner = this };
            _messageWindow.ShowDialog();
        }

        private bool ReceiveInvitation(string fromPlayer)
        {
            var messageBoxResult = MessageBox.Show(this, $"You are invited for a game by player {fromPlayer}. Accept?.", "Game Invitation", MessageBoxButton.YesNo);
            return messageBoxResult == MessageBoxResult.Yes;
        }

        private void ReceiveInvitationReply(bool isAccepted, string opponent, GameLayout gameLayout)
        {
            _messageWindow.Close();
            if (isAccepted)
                StartNewGame(opponent, gameLayout);
            else
                MessageBox.Show(this, $"Your invitation was denied.", "Invitation Reply", MessageBoxButton.OK);
        }

        private void StartNewGame(string opponent, GameLayout gameLayout)
        {
            ClearGameBoard();
            SetNewGameBoard(gameLayout.RowCount, gameLayout.ColumnCount);
            OpponentLabel.Content = opponent;
        }

        private void AddPoint(bool forMe)
        {
            if (forMe)
                YourScoreLabel.Content = (int)YourScoreLabel.Content + 1;
            else
                OpponentScoreLabel.Content = (int)OpponentScoreLabel.Content + 1;
        }

        private void ShowResults(bool? youWon)
        {
            if (!youWon.HasValue)
                MessageBox.Show(this, $"It's draw.", "Results", MessageBoxButton.OK);
            else if (youWon.Value)
                MessageBox.Show(this, $"You won.", "Results", MessageBoxButton.OK);
            else
                MessageBox.Show(this, $"You lost.", "Results", MessageBoxButton.OK);
        }

        private void ClearGameBoard()
        {
            PairGrid.Children.Clear();
            PairGrid.RowDefinitions.Clear();
            PairGrid.ColumnDefinitions.Clear();

            YourScoreLabel.Content = 0;
            OpponentScoreLabel.Content = 0;
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

        private void CardButton_Click(object sender, RoutedEventArgs e)
        {
            CardButton card = (CardButton)sender;
            _pairsGamePresenter.NextMove(card);
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
