using Pairs.DesktopClient.Presenter;
using Pairs.InterfaceLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
