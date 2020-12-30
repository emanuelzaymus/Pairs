using System.Runtime.Serialization;

namespace Pairs.Server
{
    [DataContract]
    class Player
    {
        [DataMember(Order = 0)]
        public int Id { get; private set; }

        [DataMember(Order = 1)]
        public string Nick { get; private set; }

        [DataMember(Order = 2)]
        public string EncryptedPassword { get; private set; }

        public bool IsOnline { get; set; } = false;

        public bool IsPlaying { get; set; } = false;

        public Player(int id, string nick, string encryptedPassword)
        {
            Id = id;
            Nick = nick;
            EncryptedPassword = encryptedPassword;
        }

    }
}
