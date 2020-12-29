using System.Runtime.Serialization;

namespace Pairs.InterfaceLibrary
{
    [DataContract]
    public class Player
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Nick { get; set; }

        public Player(int playerId, string nick)
        {
            Id = playerId;
            Nick = nick;
        }
    }
}
