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

        public string EncryptedPassword { get; }

        public Player(int playerId, string nick)
        {
            Id = playerId;
            Nick = nick;
        }

        public Player(int id, string nick, string encryptedPassword) : this(id, nick)
        {
            EncryptedPassword = encryptedPassword;
        }

    }
}
