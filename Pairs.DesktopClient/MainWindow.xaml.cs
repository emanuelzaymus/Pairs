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

namespace Pairs.DesktopClient
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

            _pairsGamePresenter.StartNewGame();


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
                CardButton card = new CardButton(i / columnCount, i % columnCount);
                Grid.SetRow(card, i / columnCount);
                Grid.SetColumn(card, i % columnCount);
                PairGrid.Children.Add(card);
                card.Click += Card_Click;
            }
        }

        private void Card_Click(object sender, RoutedEventArgs e)
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

        protected override void OnClosing(CancelEventArgs e)
        {
            _pairsGamePresenter.Dispose();
            base.OnClosing(e);
        }

    }
}
