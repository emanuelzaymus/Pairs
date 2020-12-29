﻿using Pairs.InterfaceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pairs.Server
{
    class PlayersManager
    {
        private int _nextPlayerId = 1;

        private List<Player> _players = new List<Player>() { new Player(55, "Emo", "zzz") };

        internal Player GetPlayer(string nick, string encryptedPassword)
        {
            return _players.FirstOrDefault(x => x.Nick == nick && x.EncryptedPassword == encryptedPassword);
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

    }
}
