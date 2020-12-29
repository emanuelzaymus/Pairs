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

        public LogInWindow(LogInButtonClickedEventhandler logInEventHandler)
        {
            InitializeComponent();
            LogInButtonClicked = logInEventHandler;
            DataContext = PlayerCredentials;
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            LogInButtonClicked(this, PlayerCredentials);
        }

        internal void ShowUnsuccessMessage()
        {
            WrongCredentialsLabel.Visibility = Visibility.Visible;
        }
    }
}
