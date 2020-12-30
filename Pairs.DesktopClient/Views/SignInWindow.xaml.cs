using Pairs.DesktopClient.Presenter;
using System.Windows;

namespace Pairs.DesktopClient.Views
{
    /// <summary>
    /// Interaction logic for SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    {
        private NewPlayerCredentials NewPlayerCredentials { get; } = new NewPlayerCredentials();

        public delegate void SignInButtonClickedEventhandler(SignInWindow signInWindow, LogInWindow logInWindow, PlayerCredentials newPlayerCredentials);
        private event SignInButtonClickedEventhandler SignInButtonClicked;

        private LogInWindow LogInWindow { get; }

        public SignInWindow(SignInButtonClickedEventhandler signInEventHandler, LogInWindow logInWindow)
        {
            InitializeComponent();
            SignInButtonClicked = signInEventHandler;
            LogInWindow = logInWindow;
            DataContext = NewPlayerCredentials;
            Owner = logInWindow;
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewPlayerCredentials.Valid)
            {
                SignInButtonClicked(this, LogInWindow, NewPlayerCredentials);
            }
            else
            {
                ShowAlertMessage(NewPlayerCredentials.GetAlertMessage());
            }
        }

        private void ShowAlertMessage(string msg)
        {
            AlertMessage.Content = msg;
        }

        public void ShowNickAlreadyExistsMessage()
        {
            ShowAlertMessage("Player with such a Nick already exists.");
        }

        private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            NewPlayerCredentials.Password = NewPasswordBox.Password;
        }

        private void RepeatedPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            NewPlayerCredentials.RepeatedPassword = RepeatedPasswordBox.Password;
        }
    }
}
