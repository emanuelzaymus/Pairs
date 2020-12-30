using Pairs.DesktopClient.Presenter;
using Pairs.InterfaceLibrary;
using System.Collections.Generic;
using System.Windows;

namespace Pairs.DesktopClient.Views
{
    /// <summary>
    /// Interaction logic for NewGameWindow.xaml
    /// </summary>
    public partial class NewGameWindow : Window
    {
        private NewGameData NewGameData { get; } = new NewGameData();

        internal delegate void SendInvitationButtonClickedEventhandler(NewGameData newGameData);
        private event SendInvitationButtonClickedEventhandler SendInvitationButtonClicked;

        internal NewGameWindow(List<string> players, SendInvitationButtonClickedEventhandler sendInvitationEventhandler)
        {
            InitializeComponent();
            DataContext = NewGameData;
            GameLayoutsComboBox.ItemsSource = GameLayout.GetValues();
            PlayersComboBox.ItemsSource = players;
            SendInvitationButtonClicked = sendInvitationEventhandler;
        }

        private void SendInvitation_Click(object sender, RoutedEventArgs e)
        {
            if (NewGameData.Valid)
            {
                Close();
                SendInvitationButtonClicked(NewGameData);
            }
            else ShowAlertMessage();
        }

        private void ShowAlertMessage()
        {
            SelectAnOptionLabel.Visibility = Visibility.Visible;
        }

    }
}
