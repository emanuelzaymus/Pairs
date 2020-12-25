using System.Windows;
using System.Windows.Controls;

namespace Pairs.DesktopClient
{
    class Card : Button
    {
        private const char _cardBackFace = '#';

        public int Row { get; }
        public int Column { get; }

        public Card(int row, int column)
        {
            Row = row;
            Column = column;
            Hide();
            Margin = new Thickness(5);
        }

        public void Show(int cardNumber)
        {
            Content = cardNumber;
            IsEnabled = false;
        }

        public void Hide()
        {
            Content = _cardBackFace;
            IsEnabled = true;
        }

        public void Remove()
        {
            Visibility = Visibility.Hidden;
        }

    }
}
