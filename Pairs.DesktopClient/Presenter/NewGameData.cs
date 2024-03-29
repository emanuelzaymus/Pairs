﻿using Pairs.InterfaceLibrary;

namespace Pairs.DesktopClient.Presenter
{
    class NewGameData
    {
        public GameLayout GameLayout { get; set; }

        public string WithPlayer { get; set; }

        public bool Valid => GameLayout != null && WithPlayer != null;
    }
}
