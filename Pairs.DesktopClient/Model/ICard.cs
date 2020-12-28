namespace Pairs.DesktopClient.Model
{
    interface ICard
    {
        int Row { get; }

        int Column { get; }

        void Show(object cardFrontFace);

        void Hide();

        void Remove();
    }
}
