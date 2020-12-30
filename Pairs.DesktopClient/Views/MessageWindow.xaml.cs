using System.Windows;

namespace Pairs.DesktopClient.Views
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public string Message
        {
            get => MessageLabel.Content.ToString();
            set => MessageLabel.Content = value;
        }

        public MessageWindow(string message, string title)
        {
            InitializeComponent();
            Message = message;
            Title = title;
        }
    }
}
