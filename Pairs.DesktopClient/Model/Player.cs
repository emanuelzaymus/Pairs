namespace Pairs.DesktopClient.Model
{
    class Player
    {
        public int Id { get; set; }

        public string Nick { get; set; }

        public Player(int id, string nick)
        {
            Id = id;
            Nick = nick;
        }
    }
}
