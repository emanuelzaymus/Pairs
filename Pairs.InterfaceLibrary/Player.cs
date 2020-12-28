using System.Runtime.Serialization;

namespace Pairs.InterfaceLibrary
{
    [DataContract]
    public class Player
    {
        [DataMember]
        public int PlayerId { get; set; }

        [DataMember]
        public string Nick { get; set; }

        public Player(int playerId, string nick)
        {
            PlayerId = playerId;
            Nick = nick;
        }
    }
}
