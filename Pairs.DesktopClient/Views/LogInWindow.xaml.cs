using Pairs.DesktopClient.Presenter;
using System.Windows;

namespace Pairs.DesktopClient.Views
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        private PlayerCredentials PlayerCredentials { get; } = new PlayerCredentials();

        public delegate void LogInButtonClickedEventhandler(LogInWindow logInWindow, PlayerCredentials playerCredentials);
        private event LogInButtonClickedEventhandler LogInButtonClicked;

        public delegate void ShowSignInWindowEventHandler(LogInWindow logInWindow);
        private event ShowSignInWindowEventHandler ShowSignInWindow;

        public LogInWindow(LogInButtonClickedEventhandler logInEventHandler, ShowSignInWindowEventHandler showSignInWindowEventHandler)
        {
            InitializeComponent();
            LogInButtonClicked = logInEventHandler;
            ShowSignInWindow = showSignInWindowEventHandler;
            DataContext = PlayerCredentials;
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            LogInButtonClicked(this, PlayerCredentials);
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            ShowSignInWindow(this);
        }

        internal void ShowUnsuccessMessage()
        {
            WrongCredentialsLabel.Visibility = Visibility.Visible;
        }
    }
}
