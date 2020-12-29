using Pairs.InterfaceLibrary;
using System.Collections.Generic;
using System.Linq;

namespace Pairs.Server
{
    class PlayersManager
    {
        private List<Player> _players = new List<Player>() { new Player(55, "Emo", "zzz") };

        internal Player GetPlayer(string nick, string encryptedPassword)
        {
            return _players.FirstOrDefault(x => x.Nick == nick && x.EncryptedPassword == encryptedPassword);
        }
    }
}
