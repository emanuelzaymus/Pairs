using Pairs.DesktopClient.Model;
using System.Windows;
using System.Windows.Controls;

namespace Pairs.DesktopClient.Presenter
{
    class CardButton : Button, ICard
    {
        private const char _cardBackFace = '#';

        public int Row { get; }
        public int Column { get; }

        public CardButton(int row, int column)
        {
            Row = row;
            Column = column;
            Hide();
            Margin = new Thickness(5);
        }

        public void Show(object cardFrontFace)
        {
            Content = cardFrontFace;
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
