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
        private NewGame NewGame { get; } = new NewGame();

        public NewGameWindow(List<string> players)
        {
            InitializeComponent();
            DataContext = NewGame;
            GameLayoutsComboBox.ItemsSource = GameLayout.GetValues();
            PlayersComboBox.ItemsSource = players;
        }

        private void SendInvitation_Click(object sender, RoutedEventArgs e)
        {
            if (!NewGame.Valid)
            {
                ShowAlertMessage();
            }
            Trace.WriteLine($"{NewGame.GameLayout}  {NewGame.WithPlayer}");
        }

        private void ShowAlertMessage()
        {
            SelectAnOptionLabel.Visibility = Visibility.Visible;
        }

    }
}
