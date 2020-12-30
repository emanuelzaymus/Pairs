using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Pairs.Server
{
    [DataContract]
    class PlayersManager
    {
        [DataMember]
        private int _nextPlayerId = 1;

        [DataMember]
        private readonly List<Player> _players = new List<Player>();

        public List<Player> OnlinePlayers => _players.Where(p => p.IsOnline).ToList();

        internal static PlayersManager FromJsonFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var serializer = new DataContractJsonSerializer(typeof(PlayersManager));
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    return (PlayersManager)serializer.ReadObject(stream);
                }
            }
            else return new PlayersManager();
        }

        internal void ToJsonFile(string filePath)
        {
            var serializer = new DataContractJsonSerializer(typeof(PlayersManager));
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                serializer.WriteObject(stream, this);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="nick"></param>
        /// <param name="encryptedPassword"></param>
        /// <returns>Returns ID of logged in player.</returns>
        internal int? LogInPlayer(string nick, string encryptedPassword)
        {
            var player = GetPlayer(nick, encryptedPassword);
            if (player != null)
            {
                player.IsOnline = true;
                return player.Id;
            }
            return null;
        }

        internal bool AddPlayer(string nick, string encryptedPassword)
        {
            if (_players.Any(x => x.Nick == nick))
            {
                return false;
            }
            _players.Add(new Player(_nextPlayerId++, nick, encryptedPassword));
            return true;
        }

        internal bool LogOut(int playerId)
        {
            var player = GetPlayer(playerId);
            if (player != null)
            {
                player.IsOnline = false;
                return true;
            }
            return false;
        }

        public Player GetPlayer(int playerId)
        {
            return _players.FirstOrDefault(p => p.Id == playerId);
        }

        public Player GetPlayer(string nick)
        {
            return _players.FirstOrDefault(p => p.Nick == nick);
        }

        private Player GetPlayer(string nick, string encryptedPassword)
        {
            return _players.FirstOrDefault(p => p.Nick == nick && p.EncryptedPassword == encryptedPassword);
        }

    }
}
